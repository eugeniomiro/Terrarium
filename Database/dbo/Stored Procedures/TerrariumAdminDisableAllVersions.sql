CREATE PROCEDURE [dbo].[TerrariumAdminDisableAllVersions]
AS
    UPDATE
        VersionedSettings
    SET
        Disabled=1
