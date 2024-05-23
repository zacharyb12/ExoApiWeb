
CREATE PROCEDURE [dbo].[AddUser]

	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50)
	

AS
	BEGIN


	DECLARE @Salt nvarchar(60)
	DECLARE @HashedPassword VARBINARY(64)
	DECLARE @Pepper NVARCHAR(100)

	SET @Salt = (CONVERT(VARCHAR(60),NEWID()))
	SET @Pepper = [dbo].GetPepper()
	SET @HashedPassword = HASHBYTES('SHA2_512',CONCAT(@Password,@Salt,@Pepper))



	INSERT INTO [User]  
	(FirstName,LastName,Email,Password,Salt)
	OUTPUt inserted.Id
	VALUES 
	(@FirstName,@LastName,@Email,@HashedPassword,@Salt)


	END

RETURN 0
