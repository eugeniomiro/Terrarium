/****** Object:  Stored Procedure dbo.TerrariumGrabNumPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabNumPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabNumPeers]
    @Version VARCHAR(255),
    @Channel VARCHAR(255)
AS
    SELECT
        Count(*)
    FROM
        Peers
    WHERE
        Version=@Version AND
        Channel=@Channel