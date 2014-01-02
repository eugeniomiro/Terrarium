/****** Object:  Stored Procedure dbo.TerrariumReintroduceSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumReintroduceSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumReintroduceSpecies]
    @Name VARCHAR(255),
    @ReintroductionNode UNIQUEIDENTIFIER,
    @LastReintroduction DATETIME
AS
    UPDATE
        Species
    SET
        Extinct = 0,
        ReintroductionNode=@ReintroductionNode,
        LastReintroduction=@LastReintroduction
    WHERE
        Name = @Name;
