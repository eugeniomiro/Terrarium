CREATE PROCEDURE [dbo].[TerrariumTopAnimals]

    @Count			INT,
    @Version		VARCHAR(25),
    @SpeciesType	VARCHAR(50)

AS

SET ROWCOUNT @Count

SELECT	Species.Name AS Name, 
    Species.Author AS AuthorName, 
    DailyPopulation.Population Population 
FROM	SPECIES INNER JOIN DailyPopulation ON (Species.Name = DailyPopulation.SpeciesName)
WHERE 	DailyPopulation.SampleDateTime = (SELECT Max(SampleDateTime) FROM DailyPopulation)
    AND Species.Version = @Version 
    AND Species.Type = @SpeciesType 
    AND DailyPopulation.SecurityViolationCount = 0 
    ORDER BY DailyPopulation.Population DESC

SET ROWCOUNT 0