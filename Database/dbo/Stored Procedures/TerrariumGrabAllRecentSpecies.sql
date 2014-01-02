/****** Object:  Stored Procedure dbo.TerrariumGrabAllRecentSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllRecentSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllRecentSpecies]
    @Version varchar(255)
AS
    DECLARE @RecentDate DATETIME
    SELECT @RecentDate = DATEADD(dd, -5, getutcdate());
    
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
        VERSION=@Version AND
        BlackListed=0 AND
        (
                (ExtinctDate > @RecentDate AND Extinct = 1) OR
                Extinct = 0
        )