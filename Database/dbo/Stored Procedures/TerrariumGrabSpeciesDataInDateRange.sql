/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesDataInDateRange    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesDataInDateRange    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabSpeciesDataInDateRange] (
    @SpeciesName VARCHAR(50),
    @BeginDate DATETIME,
    @EndDate DATETIME
)
AS
    SELECT
        *
    FROM
        DailyPopulation
    WHERE
        SpeciesName = @SpeciesName AND
        SampleDateTime BETWEEN @BeginDate AND @EndDate
