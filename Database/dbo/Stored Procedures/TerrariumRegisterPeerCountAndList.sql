CREATE PROCEDURE [dbo].[TerrariumRegisterPeerCountAndList]
    @Version        VARCHAR(255),
    @FullVersion    VARCHAR(255),
    @Channel        VARCHAR(255),
    @IPAddress      VARCHAR(50),
    @Guid           UNIQUEIDENTIFIER,
    @Disabled_Error BIT OUTPUT,
    @PeerCount      INT OUTPUT
AS
    DECLARE @Disabled   BIT;
    DECLARE @Lease      DATETIME;
    DECLARE @Total      INT
    DECLARE @BelowCount INT
    DECLARE @AboveCount INT

    SET @PeerCount  = 0;
    SET @Lease      = DATEADD(mi, 15, GETUTCDATE());

    SELECT @Disabled = [Disabled]
        FROM    VersionedSettings
        WHERE   [Version] = @FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT  @Disabled = [Disabled]
                FROM    VersionedSettings
                WHERE   [Version] = @Version

            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT  @Disabled = [Disabled]
                        FROM    VersionedSettings
                        WHERE   [Version] = 'Default'
                    INSERT INTO VersionedSettings ([Version], [Disabled])
                                           VALUES (@Version, @Disabled)
                    INSERT INTO VersionedSettings ([Version], [Disabled])
                                           VALUES (@FullVersion, @Disabled)
                END
            ELSE
                BEGIN
                    INSERT INTO VersionedSettings ([Version], [Disabled])
                                           VALUES (@FullVersion, @Disabled)
                END
        END
    IF @Disabled = 1
        BEGIN
            SET @Disabled_Error = 1;
            SELECT TOP 0 Lease, IPAddress
                FROM Peers
        END
    ELSE
        BEGIN
            DECLARE @Self INT;
            SELECT @Self = count(*)
                FROM Peers
                WHERE Channel = @Channel
                  AND IPAddress = @IPAddress
        
            IF @Self = 0
                BEGIN
                    BEGIN TRAN
                        INSERT INTO Peers(Channel, [Version], IPAddress, Lease, [GUID], FirstContact)
                                   VALUES(@Channel, @Version, @IPAddress, @Lease, @Guid, GETUTCDATE())
                    COMMIT TRAN
                END
            ELSE
                BEGIN
                    BEGIN TRAN
                    UPDATE Peers
                        SET     Lease = @Lease,
                                [Version] = @Version
                        WHERE   Channel = @Channel AND
                                IPAddress = @IPAddress
                    COMMIT TRAN
                END
            Set @Disabled_Error = 0
        SELECT @PeerCount = Count(*)
            FROM Peers
            WHERE [Version] = @Version
              AND Channel = @Channel
            SELECT @Total = count(*) 
                FROM Peers 
                WHERE [Version] = @Version
                  AND Channel = @Channel
            IF @Total > 30
                BEGIN
                    SELECT @AboveCount = count(*)
                        FROM Peers
                        WHERE [Version] = @Version 
                          AND Channel = @Channel 
                          AND IPAddress > @IPAddress
        
                    SELECT @BelowCount = count(*)
                        FROM Peers
                        WHERE [Version] = @Version
                          AND Channel = @Channel 
                          AND IPAddress < @IPAddress
        
                    IF @BelowCount < 10
                        BEGIN
                            SELECT IPAddress,Lease
                                FROM Peers
                                WHERE [Version] = @Version 
                                  AND Channel = @Channel
                                  AND (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel = @Channel AND IPAddress > @IPAddress ORDER BY IPAddress ASC) OR
                                       IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel = @Channel AND IPAddress < @IPAddress ORDER BY IPAddress DESC) OR
                                       IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel = @Channel AND IPAddress < '255.255.255.255' ORDER BY IPAddress DESC)
                                      )
                        END
                    ELSE
                        BEGIN
                            IF @AboveCount < 10
                                BEGIN
                                    SELECT IPAddress,Lease
                                        FROM Peers
                                        WHERE [Version] = @Version 
                                          AND Channel=@Channel 
                                          AND (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                                               IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                               IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version] = @Version AND Channel=@Channel AND IPAddress>'0.0.0.0' ORDER BY IPAddress ASC)
                                              )
                                END
                            ELSE
                                BEGIN
                                    SELECT IPAddress,Lease
                                        FROM Peers
                                        WHERE [Version] = @Version 
                                          AND Channel = @Channel 
                                          AND (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                                               IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC)
                                              )
                                END
                        END
                END
            ELSE
                BEGIN
                    SELECT IPAddress, Lease FROM Peers WHERE [Version] = @Version AND Channel=@Channel
                END
        END