/****** Object:  Stored Procedure dbo.TerrariumGrabLatestSpeciesData    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabLatestSpeciesData    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabLatestSpeciesData] (
    @SpeciesName VARCHAR(50)
)
AS
    SELECT
        *
    FROM
        DailyPopulation
    WHERE
        SpeciesName = @SpeciesName AND
        SampleDateTime = (
            SELECT
                Max(SampleDateTime)
            FROM
                DailyPopulation
            WHERE
                SpeciesName = @SpeciesName
        )