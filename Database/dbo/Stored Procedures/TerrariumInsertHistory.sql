CREATE PROCEDURE [dbo].[TerrariumInsertHistory]
    @Guid uniqueidentifier,
    @TickNumber INT,
    @SpeciesName VARCHAR(255),
    @ContactTime DATETIME,
    @ClientTime DATETIME,
    @CorrectTime TINYINT,
    @Population INT,
    @BirthCount INT,
    @TeleportedToCount INT,
    @StarvedCount INT,
    @KilledCount INT,
    @TeleportedFromCount INT,
    @ErrorCount INT,
    @TimeoutCount INT,
    @SickCount INT,
    @OldAgeCount INT,
    @SecurityViolationCount INT,
    @BlackListed INT OUT
AS
    SELECT
        @BlackListed=BlackListed
    FROM
        Species
    WHERE
        Name=@SpeciesName
    INSERT INTO
        History(
            GUID,
            TickNumber,
            SpeciesName,
            ContactTime,
            ClientTime,
            CorrectTime,
            Population,
            BirthCount,
            TeleportedToCount,
            StarvedCount,
            KilledCount,
            TeleportedFromCount,
            ErrorCount,
            TimeoutCount,
            SickCount,
            OldAgeCount,
            SecurityViolationCount
        )
        VALUES(
            @Guid,
            @TickNumber,
            @SpeciesName,
            @ContactTime,
            @ClientTime,
            @CorrectTime,
            @Population,
            @BirthCount,
            @TeleportedToCount,
            @StarvedCount,
            @KilledCount,
            @TeleportedFromCount,
            @ErrorCount,
            @TimeoutCount,
            @SickCount,
            @OldAgeCount,
            @SecurityViolationCount
        )