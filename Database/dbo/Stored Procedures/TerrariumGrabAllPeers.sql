/****** Object:  Stored Procedure dbo.TerrariumGrabAllPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllPeers]
    @Version VARCHAR(255),
    @Channel VARCHAR(255)
AS
    SELECT
        *
    FROM
        Peers
    WHERE
        Version=@Version AND
        Channel=@Channel