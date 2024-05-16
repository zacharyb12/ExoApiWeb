CREATE PROCEDURE [dbo].[AddUser]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50)
AS
	INSERT INTO [User]  
	(FirstName,LastName,Email,Password)
	VALUES 
	(@FirstName,@LastName,@Email,@Password)

RETURN 0
