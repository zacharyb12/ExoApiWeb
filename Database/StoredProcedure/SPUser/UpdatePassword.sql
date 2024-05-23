
CREATE PROCEDURE [dbo].[UpdatePassword]

	@Id uniqueidentifier,
	@Password nvarchar(50)
AS

	DECLARE @Salt nvarchar(60)
	DECLARE @HashedPassword VARBINARY(64)
	DECLARE @Pepper NVARCHAR(100)

	SET @Pepper = [dbo].GetPepper()
	SET @Salt = (CONVERT(VARCHAR(60),NEWID()))
	SET @HashedPassword = HASHBYTES('SHA2_512',CONCAT(@Password,@Salt,@Pepper))

	UPDATE [User]
	SET 
	Password = @HashedPassword,Salt = @Salt
	WHERE Id = @Id

RETURN 0