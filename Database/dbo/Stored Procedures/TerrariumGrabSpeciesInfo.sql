/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesInfo    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesInfo    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabSpeciesInfo]
AS
    SELECT
        Species.Name AS SpeciesName,
        Species.Author AS AuthorName,
        Species.AuthorEmail AS AuthorEmail,
        Species.Version AS Version,
        DailyPopulation.Population AS Population,
        Species.Type AS Type
    FROM
        Species INNER JOIN
        DailyPopulation ON (Species.Name = DailyPopulation.SpeciesName)
    WHERE
        DailyPopulation.SampleDateTime = (
            SELECT
                Max(SampleDateTime)
            FROM
                DailyPopulation
            WHERE
                DailyPopulation.SpeciesName = Species.Name
        ) AND
        BlackListed=0