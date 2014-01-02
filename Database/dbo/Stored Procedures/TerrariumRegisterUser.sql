/****** Object:  Stored Procedure dbo.TerrariumRegisterUser    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumRegisterUser    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumRegisterUser]
    @Email VARCHAR(255),
    @IPAddress VARCHAR(50)
AS
    SELECT
        NULL
    FROM
        UserRegister
    WHERE
        IPAddress = @IPAddress
    IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO
                UserRegister(
                    IPAddress,
                    Email
                )
                VALUES(
                    @IPAddress,
                    @Email
                )
        END
    ELSE
        BEGIN
            UPDATE
                UserRegister
            SET
                Email = @Email
            WHERE
                IPAddress = @IPAddress
        END