/****** Object:  Stored Procedure dbo.TerrariumAdminEnableVersion    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumAdminEnableVersion    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumAdminEnableVersion]
    @Version VARCHAR(255)
AS
    SELECT
        *
    FROM
        VersionedSettings
    WHERE
        Version=@Version
    IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO
                VersionedSettings (
                    Version,
                    Disabled
                ) VALUES (
                    @Version,
                    0
                )
        END
    ELSE
        BEGIN
            UPDATE
                VersionedSettings
            SET
                Disabled=0
            WHERE
                Version=@Version
        END