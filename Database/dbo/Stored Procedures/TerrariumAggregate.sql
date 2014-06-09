CREATE PROCEDURE [dbo].[TerrariumAggregate] (
    @Expiration_Error INT OUT,
    @Rollup_Error INT OUT,
    @Timeout_Add_Error INT OUT,
    @Timeout_Delete_Error INT OUT,
    @Extinction_Error INT OUT
)
AS
-- Timing Code
    -- Declarations
    DECLARE @Last DATETIME;
    DECLARE @Now DATETIME;
    DECLARE @Timeout DATETIME;
    -- Initialization
    SELECT @Last=Max(SampleDateTime) FROM DailyPopulation
    SET @Now = GETUTCDATE();
    SET @Timeout = DATEADD(hh, -48, @Now);
BEGIN TRANSACTION
-- Expire Peer Leases
    INSERT INTO
        ShutdownPeers (
            [Guid],
            Channel,
            IPAddress,
            FirstContact,
            LastContact,
            [Version],
            UnRegister
        )
    SELECT
        [Guid],
        Channel,
        IPAddress,
        FirstContact,
        GETUTCDATE(),
        [Version],
        0
    FROM
        Peers
    WHERE
        Lease < @Now
    DELETE FROM
        Peers
    WHERE
        Lease < @Now
    SET @Expiration_Error = @@ERROR;
IF @Expiration_Error = 0
    COMMIT TRAN
ELSE
    ROLLBACK TRAN
-- TRANSACTION 1
-- Reporting and Timeouts
BEGIN TRANSACTION
    -- Timeouts
        -- Add Timed Out Nodes
        INSERT INTO
            TimedOutNodes (
                [GUID],
                TimeOutDate
            )
        SELECT
            [GUID],
            @Timeout AS Expr1
        FROM
            NodeLastContact
        WHERE
            LastContact < @Timeout
        SET @Timeout_Add_Error = @@ERROR;
        -- Removed Timed Out Nodes from Current Nodes
        DELETE FROM
            NodeLastContact
        WHERE
            GUID IN (
                SELECT
                    TimedOutNodes.[GUID]
                FROM
                    NodeLastContact INNER JOIN
                    TimedOutNodes ON
                        (NodeLastContact.[GUID] = TimedOutNodes.[GUID])
            )
        SET @Timeout_Delete_Error = @@ERROR;
    IF @Timeout_Delete_Error = 0
        COMMIT TRAN
    ELSE
        ROLLBACK TRAN
-- TRANSACTION 2
BEGIN TRANSACTION
    -- Mark Extinct Animals
        CREATE TABLE #ExtinctSpecies (Name varchar(250) collate SQL_Latin1_General_CP1_CI_AS)
        IF @@ERROR = 0
        BEGIN
                INSERT INTO
                        #ExtinctSpecies
                SELECT
                    Name
                FROM
                    Species
                        WHERE
                            Species.Name NOT IN (
                                SELECT
                                    SpeciesName
                                FROM
                                    DailyPopulation
                                WHERE
                                    SampleDateTime = (SELECT Max(SampleDateTime) FROM DailyPopulation)
                            ) AND
                            Species.Name NOT IN (
                                SELECT
                                    Name
                                FROM
                                    Species INNER JOIN
                                    NodeLastContact ON
                                        (Species.ReintroductionNode = NodeLastContact.[GUID])
                                WHERE
                                    NodeLastContact.LastContact < Species.LastReintroduction
                            ) AND
                            Species.Name NOT IN (
                                SELECT DISTINCT
                                        SpeciesName
                                    FROM
                                        History
                            ) AND
                        Species.DateAdded < @Last AND
                        Extinct = 0;
                IF @@ERROR = 0
                BEGIN
                        UPDATE
                                Species
                        SET
                                Extinct = 1,
                                Species.ExtinctDate = @Now
                        WHERE
                                Name IN (
                                        SELECT Name From #ExtinctSpecies
                                );
                        IF @@ERROR = 0
                        BEGIN
                            INSERT INTO
                                        DailyPopulation (
                                            SampleDateTime,
                                            SpeciesName,
                                            [Population],
                                            BirthCount,
                                            StarvedCount,
                                            KilledCount,
                                            ErrorCount,
                                            SecurityViolationCount,
                                            TimeoutCount,
                                            SickCount,
                                            OldAgeCount
                                        )
                                SELECT
                                        @Now AS Expr1,
                                        Name, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                FROM
                                        #ExtinctSpecies    
                        END
                END
        END
        SET @Extinction_Error = @@ERROR;
    IF @Extinction_Error = 0
        COMMIT TRAN
    ELSE
        ROLLBACK TRAN
        -- Mark Non Extinct Animals
        UPDATE
                Species
        SET
                Species.Extinct = 0
        WHERE
                Species.Name IN (SELECT DISTINCT SpeciesName FROM History)
-- TRANSACTION 3
    -- Rollup
    INSERT INTO
        DailyPopulation (
            SampleDateTime,
            SpeciesName,
            [Population],
            BirthCount,
            StarvedCount,
            KilledCount,
            ErrorCount,
            SecurityViolationCount,
            TimeoutCount,
            SickCount,
            OldAgeCount
        )
    SELECT
        @Now AS Expr1,
        History.SpeciesName,
        Sum(History.[Population]) AS SumOfPopulation,
        Sum(History.BirthCount) AS SumOfBirthCount,
        Sum(History.StarvedCount) AS SumOfStarvedCount,
        Sum(History.KilledCount) AS SumOfKilledCount,
        Sum(History.ErrorCount) AS SumOfErrors,
        Sum(History.SecurityViolationCount) AS SumOfSecurityViolations,
        Sum(History.TimeoutCount) AS SumOfTimeouts,
        Sum(History.SickCount) AS SumOfSickCount,
        Sum(History.OldAgeCount) AS SumOfOldAgeCount
    FROM
        NodeLastContact INNER JOIN
        History ON
            (NodeLastContact.LastTick = History.TickNumber) AND
            (NodeLastContact.[GUID] = History.[GUID]) INNER JOIN
        Species ON
            (Species.Name = History.SpeciesName)
    WHERE
        Species.Extinct = 0
    GROUP BY
        History.SpeciesName
    SET @Rollup_Error = @@ERROR;
        DELETE
                History
        FROM
                NodeLastContact INNER JOIN
                History ON (
                        NodeLastContact.LastTick > History.TickNumber AND
                        NodeLastContact.[GUID] = History.[GUID]
                )