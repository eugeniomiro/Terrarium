/****** Object:  Stored Procedure dbo.TerrariumRegisterPeer    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumRegisterPeer    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE     PROCEDURE [dbo].[TerrariumRegisterPeer]
    @Version VARCHAR(255),
    @FullVersion VARCHAR(255),
    @Channel VARCHAR(255),
    @IPAddress VARCHAR(50),
    @Guid UNIQUEIDENTIFIER,
    @Disabled_Error BIT OUTPUT
AS
    DECLARE @Disabled bit;
    DECLARE @Lease DATETIME;
    SET @Lease = DATEADD(mi, 15, GETUTCDATE());
    SELECT
        @Disabled=Disabled
    FROM
        VersionedSettings
    WHERE
        Version=@FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                @Disabled=Disabled
            FROM
                VersionedSettings
            WHERE
                Version=@Version
            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT
                        @Disabled=Disabled
                    FROM    
                        VersionedSettings
                    WHERE
                        Version='Default'
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @Version,
                            @Disabled
                        )
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
            ELSE
                BEGIN
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
        END
    IF @Disabled = 1
        BEGIN
            Set @Disabled_Error = 1;
        END
    ELSE
        BEGIN
            SELECT
                NULL
            FROM
                Peers
            WHERE
                Channel = @Channel AND
                IPAddress = @IPAddress
        
            IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO
                        Peers(
                            Channel,
                            Version,
                            IPAddress,
                            Lease,
                            Guid,
                            FirstContact
                        )
                        VALUES(
                            @Channel,
                            @Version,
                            @IPAddress,
                            @Lease,
                            @Guid,
                            GETUTCDATE()
                        )
                END
            ELSE
                BEGIN
                    UPDATE
                        Peers
                    SET
                        Lease = @Lease,
                        Version = @Version
                    WHERE
                        Channel = @Channel AND
                        IPAddress = @IPAddress
                END
            Set @Disabled_Error = 0
        END