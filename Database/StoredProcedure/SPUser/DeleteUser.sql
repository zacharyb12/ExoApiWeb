
CREATE PROCEDURE [dbo].[DeleteUser]

	@Id uniqueidentifier
AS
	UPDATE [User] Set isActive = 0 
	WHERE Id = @Id

RETURN 0
