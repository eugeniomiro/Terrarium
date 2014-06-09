CREATE PROCEDURE [dbo].[TerrariumCountDownload]
    @Filename VARCHAR(255)
AS

DECLARE @DownloadCount	INT;

SELECT @DownloadCount = (SELECT DownloadCount FROM Downloads WHERE [Filename] = @Filename)

SELECT @DownloadCount = @DownloadCount + 1

IF EXISTS( SELECT [Filename] FROM Downloads WHERE [Filename] = @Filename)
    BEGIN
        UPDATE 	Downloads
        SET	DownloadCount = @DownloadCount,
            LastDownloadDate = GETDATE()
        WHERE	[Filename] = @Filename
    END
ELSE
    BEGIN
        INSERT 	Downloads
        VALUES (@FileName,
            1,
            GETDATE())
    END

SELECT DownloadCount = @DownloadCount