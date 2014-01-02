/****** Object:  Stored Procedure dbo.TerrariumDeletePeer    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumDeletePeer    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumDeletePeer]
    @Channel VARCHAR(255),
    @IPAddress VARCHAR(50),
    @Guid UNIQUEIDENTIFIER
AS
    INSERT INTO
        ShutdownPeers (
            Guid,
            Channel,
            IPAddress,
            FirstContact,
            LastContact,
            Version,
            UnRegister
        )
    SELECT
        Guid,
        Channel,
        IPAddress,
        FirstContact,
        GETUTCDATE(),
        Version,
        1
    FROM
        Peers
    WHERE
        Channel = @Channel AND
        IPAddress = @IPAddress AND
        Guid = @Guid
    DELETE FROM
        Peers
    WHERE
        Channel = @Channel AND
        IPAddress = @IPAddress AND
        Guid = @Guid