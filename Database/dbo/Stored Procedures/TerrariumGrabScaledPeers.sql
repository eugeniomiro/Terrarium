/****** Object:  Stored Procedure dbo.TerrariumGrabScaledPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabScaledPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabScaledPeers]
    @Version VARCHAR(255),
    @Channel VARCHAR(255),
    @IPAddress VARCHAR(50)
AS
DECLARE @Total INT
DECLARE @BelowCount INT
DECLARE @AboveCount INT
SELECT @Total=count(*) FROM Peers WHERE [Version] = @Version AND Channel=@Channel
IF @Total > 30
    BEGIN
        SELECT @AboveCount=count(*)
        FROM Peers
        WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>@IPAddress
        
        SELECT @BelowCount=count(*)
        FROM Peers
        WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<@IPAddress
        
        IF @BelowCount < 10
            BEGIN
                SELECT IPAddress,Lease
                FROM Peers
                WHERE [Version]=@Version AND Channel=@Channel AND
                    (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                     IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                         IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<'255.255.255.255' ORDER BY IPAddress DESC)
                    )
            END
        ELSE
            BEGIN
                IF @AboveCount < 10
                    BEGIN
                        SELECT IPAddress,Lease
                        FROM Peers
                        WHERE [Version]=@Version AND Channel=@Channel AND
                            (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                             IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                                 IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>'0.0.0.0' ORDER BY IPAddress ASC)
                            )
                    END
                ELSE
                    BEGIN
                        SELECT IPAddress,Lease
                        FROM Peers
                        WHERE [Version]=@Version AND Channel=@Channel AND
                            (IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                             IPAddress IN (SELECT TOP 10 IPAddress FROM Peers WHERE [Version]=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC)
                            )
                    END
            END
    END
ELSE
    BEGIN
        SELECT IPAddress, Lease FROM Peers WHERE [Version]=@Version AND Channel=@Channel
    END