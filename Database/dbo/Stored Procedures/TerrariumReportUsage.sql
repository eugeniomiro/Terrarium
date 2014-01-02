CREATE  PROCEDURE [dbo].[TerrariumReportUsage]
    @Alias VARCHAR(255),
    @Domain VARCHAR(255),
    @IPAddress VARCHAR(25),
    @GameVersion VARCHAR(255),
    @PeerChannel VARCHAR(255),
    @PeerCount INT,
    @AnimalCount INT,
    @MaxAnimalCount INT,
    @WorldWidth INT,
    @WorldHeight INT,
    @MachineName VARCHAR(255),
    @OSVersion VARCHAR(255),
    @ProcessorCount INT,
    @ClrVersion VARCHAR(255),
    @WorkingSet INT,
    @MaxWorkingSet INT,
    @MinWorkingSet INT,
    @ProcessorTime INT,
    @ProcessStartTime datetime

AS 

DECLARE @LastTickTime DATETIME
DECLARE @CurrentTickTime DATETIME

SELECT @CurrentTickTime = GETDATE()

SELECT @LastTickTime = (SELECT TOP 1 TickTime FROM Usage WHERE Alias = @Alias ORDER BY TickTime DESC)
IF (@LastTickTime is null)
    SELECT @LastTickTime = '01/01/1901'

IF (DATEDIFF(minute,@LastTickTime, @CurrentTickTime) >= 60)
    BEGIN
    
        INSERT	Usage
        VALUES (@Alias,
            @Domain,
            @CurrentTickTime,
            60,
            @IPAddress,
            @GameVersion,
            @PeerChannel,
            @PeerCount,
            @AnimalCount,
            @MaxAnimalCount,
            @WorldHeight,
            @WorldWidth,
            @MachineName,
            @OSVersion,
            @ProcessorCount,
            @ClrVersion,
            @WorkingSet,
            @MaxWorkingSet,
            @MinWorkingSet,
            @ProcessorTime,
            @ProcessStartTime)
    END
ELSE
    BEGIN
        RETURN 50000
    END