
CREATE PROCEDURE [dbo].[Login]

	@Email nvarchar(50),
	@Password nvarchar(50)
AS

BEGIN

if EXISTS(SELECT * FROM [User] WHERE Email = @Email)

	DECLARE @Pepper NVARCHAR(100)
	SET @Pepper = [dbo].GetPepper();

	SELECT *
	FROM [User]
	WHERE Email = @Email
	AND
	Password =  HASHBYTES('SHA2_512',CONCAT(@Password,Salt,@Pepper))

END
RETURN 0
