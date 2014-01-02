/****** Object:  Stored Procedure dbo.TerrariumIsVersionDisabled    Script Date: 1/4/2005 6:12:18 PM ******/

create  procedure [dbo].[TerrariumIsVersionDisabled]

    @Version	VARCHAR(255),
    @FullVersion	VARCHAR(255)
as

        DECLARE @Disabled BIT;
    DECLARE @Message VARCHAR(255);

    SELECT
        @Disabled=Disabled,
        @Message=Message
    FROM
        VersionedSettings
    WHERE
        Version=@FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT
        @Disabled=Disabled,
        @Message=Message
            FROM
                VersionedSettings
            WHERE
                Version=@Version
            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT
            @Disabled=Disabled,
            @Message=Message
                    FROM    
                        VersionedSettings
                    WHERE
                        Version='Default'
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @Version,
                            @Disabled
                        )
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
            ELSE
                BEGIN
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
        END	

SELECT Disabled = @Disabled, Message = @Message