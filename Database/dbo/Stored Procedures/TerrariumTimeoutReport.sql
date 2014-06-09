CREATE PROCEDURE [dbo].[TerrariumTimeoutReport]
    @Guid           UNIQUEIDENTIFIER,
    @LastContact    DATETIME,
    @LastTick       INT,
    @ReturnCode     INT OUTPUT
AS
    DECLARE @MaxTick int
    SELECT @ReturnCode=0
    SELECT
        @MaxTick=Max(LastTick)
    FROM
        NodeLastContact
    WHERE
        [Guid]=@Guid
    -- Check to see if we have a record in the NodeLastContact
    -- Table
    IF @MaxTick IS NOT NULL
        BEGIN
            -- Now check to see if that record is newer
            -- than the current record from the client
            IF @MaxTick >= @LastTick
                BEGIN
                    Select @ReturnCode = 2
                END
            ELSE
                BEGIN
                    UPDATE
                        NodeLastContact
                    SET
                        LastTick = @LastTick,
                        LastContact = @LastContact
                    WHERE
                        [GUID]=@Guid
                END
        END
    ELSE
        BEGIN
            DECLARE @TimeoutCount DATETIME
            SELECT
                @TimeoutCount=Max(TimeoutDate)
            FROM
                TimedOutNodes
            WHERE
                [GUID] = @Guid
            IF @TimeoutCount IS NULL
                BEGIN
                    INSERT INTO
                        NodeLastContact(
                            LastTick,
                            LastContact,
                            [GUID]
                        )
                        VALUES(
                            @LastTick,
                            @LastContact,
                            @Guid
                        )
                END
            ELSE
                BEGIN
                    Select @ReturnCode=1
                END
        END