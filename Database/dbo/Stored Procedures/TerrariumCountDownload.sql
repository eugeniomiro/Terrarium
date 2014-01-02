CREATE PROCEDURE [dbo].[TerrariumCountDownload]

@Filename VARCHAR(255)

AS

DECLARE @DownloadCount	INT;

SELECT @DownloadCount = (SELECT DownloadCount FROM Downloads WHERE FILENAME = @Filename)

SELECT @DownloadCount = @DownloadCount + 1

IF EXISTS( SELECT Filename FROM Downloads WHERE FILENAME = @Filename)
    BEGIN
        UPDATE 	Downloads
        SET	DownloadCount = @DownloadCount,
            LastDownloadDate = GETDATE()
        WHERE	FILENAME = @Filename
    END
ELSE
    BEGIN
        INSERT 	Downloads
        VALUES (@FileName,
            1,
            GETDATE())
    END

SELECT DownloadCount = @DownloadCount