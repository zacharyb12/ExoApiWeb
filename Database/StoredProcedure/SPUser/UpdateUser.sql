
CREATE PROCEDURE [dbo].[UpdateUser]

	@Id uniqueidentifier,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50)

AS

	UPDATE [User]
	SET 
	FirstName = @FirstName,
	LastName = @LastName,
	Email = @Email

	WHERE Id = @Id

RETURN 0

