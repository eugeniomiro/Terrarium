/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesExtinct    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesExtinct    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumCheckSpeciesExtinct]
    @Name varchar(255)
AS
    SELECT
        Count(Name)
    FROM
        Species
    WHERE
        Name=@Name
            AND
        Extinct = 1
            AND
        BlackListed = 0