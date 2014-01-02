CREATE  PROCEDURE [dbo].[TerrariumInsertWatson]
    @LogType varchar(50),
    @MachineName varchar(255),
    @OSVersion varchar(50),
    @GameVersion varchar(50),
    @CLRVersion varchar(50),
    @ErrorLog text,
    @UserComment text,
    @UserEmail varchar(255)
AS
    INSERT INTO
        Watson(
            [LogType],
            [MachineName],
            [OSVersion],
            [GameVersion],
            [CLRVersion],
            [ErrorLog],
            [UserComment],
            [UserEmail]
        )
        VALUES(
            @LogType,
            @MachineName,
            @OSVersion,
            @GameVersion,
            @CLRVersion,
            @ErrorLog,
            @UserComment,
            @UserEmail
        )
