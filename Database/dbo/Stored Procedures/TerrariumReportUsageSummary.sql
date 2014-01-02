CREATE PROCEDURE [dbo].[TerrariumReportUsageSummary]

AS

DECLARE @Peers INT

SELECT @Peers = (SELECT count(*) FROM Peers)

INSERT	UsageSummary
       (Peers,
    SummaryDateTime)
VALUES (@Peers,
    GETDATE())

    