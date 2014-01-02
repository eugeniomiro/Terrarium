/****** Object:  Stored Procedure dbo.TerrariumInsertSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumInsertSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumInsertSpecies]
    @Name VARCHAR(255),
    @Version VARCHAR(255),
    @Type VARCHAR(50),
    @Author VARCHAR(255),
    @AuthorEmail VARCHAR(255),
    @Extinct TINYINT,
    @DateAdded DATETIME,
    @AssemblyFullName TEXT,
    @BlackListed BIT
AS
    INSERT INTO
        Species(
            [Name],
            [Version],
            [Type],
            [Author],
            [AuthorEmail],
            [Extinct],
            [DateAdded],
            [AssemblyFullName],
            [BlackListed]
        )
        VALUES(
            @Name,
            @Version,
            @Type,
            @Author,
            @AuthorEmail,
            @Extinct,
            @DateAdded,
            @AssemblyFullName,
            @BlackListed
        )