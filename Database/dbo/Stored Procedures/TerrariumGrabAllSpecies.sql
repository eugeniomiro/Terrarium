/****** Object:  Stored Procedure dbo.TerrariumGrabAllSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllSpecies]
    @Version varchar(255)
AS
    SELECT
        Name,
        Type,
        Author,
        AuthorEmail,
        DateAdded,
        AssemblyFullName,
        Extinct,
        LastReintroduction,
        ReintroductionNode,
        Version,
        BlackListed
    FROM
        Species
    WHERE
        Version=@Version
            AND
        BlackListed=0