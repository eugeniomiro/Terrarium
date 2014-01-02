/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabExtinctSpecies]
    @Version VARCHAR(255)
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
        Version=@Version AND
        Extinct=1 AND
        BlackListed=0