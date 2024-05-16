CREATE PROCEDURE [dbo].[UpdateUser]
	@Id uniqueidentifier,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50)
AS
	UPDATE [User]
	SET 
	FirstName = @FirstName,
	LastName = @LastName,
	Email = @Email,
	Password = @Password
	WHERE Id = @Id

RETURN 0
