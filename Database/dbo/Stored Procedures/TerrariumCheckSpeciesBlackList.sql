/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesBlackList    Script Date: 1/4/2005 6:12:18 PM ******/
CREATE PROCEDURE [dbo].[TerrariumCheckSpeciesBlackList]

@Name	VARCHAR(255)

AS

    SELECT	Blacklisted
    FROM	Species
    WHERE	Name = @Name