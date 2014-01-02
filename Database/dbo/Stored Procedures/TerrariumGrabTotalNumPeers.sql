CREATE PROCEDURE [dbo].[TerrariumGrabTotalNumPeers]
AS
    SELECT
        Count(*)
    FROM
        Peers