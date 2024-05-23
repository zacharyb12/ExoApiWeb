using Microsoft.Data.SqlClient;
using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.User;
using Models.Entity.UserModels;
using System.Data;
using System.Text;
using System.Transactions;

namespace Dal.Repository.UserDal
{
    public class UserRepository : IUserRepository
    {

        private readonly string ConnectionString = "Server=PCZAC\\DATAVIZ;Database=ExoAPI2024;User Id=PCZAC\\boual;Password= ;TrustServerCertificate=True;Trusted_Connection=True";

        /// <summary>
        /// Get all , need property on query
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {

            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM [User]";
                        List<User> users = new List<User>();
                        _connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while ( reader.Read())
                            {
                                User user = new User
                                {
                                    Id = reader.GetGuid("Id"),
                                    FirstName = reader.IsDBNull("FirstName") ? string.Empty : reader.GetString("FirstName"),
                                    LastName = reader.IsDBNull("LastName") ? string.Empty : reader.GetString("LastName"),
                                    Email = reader.IsDBNull("Email") ? string.Empty : reader.GetString("Email"),
                                };
                                users.Add(user);
                            }
                        }

                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving users: {ex.Message}");
                return new List<User>(); // Return an empty list on error
            }

        }


        /// <summary>
        /// Get , need more details to query property
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        public User? GetById(Guid id)
        {

            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT Id,FirstName,LastName,Email FROM [User] WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        User user = null;
                        _connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                    FirstName = reader.IsDBNull("FirstName") ? string.Empty : reader.GetString("FirstName"),
                                    LastName = reader.IsDBNull("LastName") ? string.Empty : reader.GetString("LastName"),
                                    Email = reader.IsDBNull("Email") ? string.Empty : reader.GetString("Email"),
                                };
                            }
                        }

                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving user with ID {id}: {ex.Message}");
                return null;
            }

        }


        /// <summary>
        /// New user , stored procedure need verification IsActive(Delete)
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User</returns>
        public User? AddUser(CreateUser user)
        {
            User userAdded = null;

            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        // Définir le nom de la procédure stockée
                        cmd.CommandText = "AddUser";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Ajouter les paramètres nécessaires
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        _connection.Open();

                        var Id = (Guid)cmd.ExecuteScalar();

                        User userFromDb = null;
                        if (Id != null)
                        {
                            userFromDb = GetById(Id);
                            return userFromDb;
                        }
                        else
                        {
                            return null; // Indicates failure to insert the user
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }


        /// <summary>
        /// Update value need verification
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Id"></param>
        public User? UpdateUser(UpdateUser user, Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    _connection.Open();
                    using (SqlTransaction transaction = _connection.BeginTransaction())
                    {
                        using (SqlCommand cmd = _connection.CreateCommand())
                        {
                            cmd.CommandText = "UpdateUser";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", user.LastName);
                            cmd.Parameters.AddWithValue("@Email", user.Email);
                            cmd.Parameters.AddWithValue("@Id", id);


                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                transaction.Commit();

                                User? updatedUser = GetById(id);
                                return updatedUser;
                            }
                            else
                            {
                                transaction.Rollback();
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework) and rethrow or handle accordingly
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }


        /// <summary>
        /// Desactive user
        /// </summary>
        /// <param name="Id"></param>
        public bool DeleteUser(Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [User] SET IsActive = 0 WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        _connection.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Delete user need begin rollback
        /// </summary>
        /// <param name="Id"></param>
        public bool DeleteUserFromDatabase(Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    _connection.Open();
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM [User] WHERE Id  = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        var result = cmd.ExecuteNonQuery();

                        //add begin and rollback
                        if (( result!= 0) && (result < 2))
                        {
                            return true;
                        }
                        else
                        { 
                            return false; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework) and rethrow or handle accordingly
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }


        /// <summary>
        /// simple database  verification on email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>User</returns>
        public User? Login(UserLogin user)
        {

            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Login]", _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        _connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = reader.GetGuid("Id"),
                                    FirstName = reader.IsDBNull("FirstName") ? string.Empty : reader.GetString("FirstName"),
                                    LastName = reader.IsDBNull("LastName") ? string.Empty : reader.GetString("LastName"),
                                    Email = reader.IsDBNull("Email") ? string.Empty : reader.GetString("Email"),
                                };
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework) and rethrow or handle accordingly
                throw new Exception("An error occurred while logging in the user.", ex);
            }
        }


        /// <summary>
        /// Verification if existing
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Exist(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(Id) FROM [User] WHERE (FirstName LIKE @Username OR Email LIKE @Email) AND Id NOT LIKE @Id";
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);

                        connection.Open();
                        int nbrOccurrence = (int)cmd.ExecuteScalar();

                        return nbrOccurrence > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while checking existence: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Update value need verification
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Id"></param>
        public User? UpdatePassword(UserNewPassword user, Guid id)
        {
            UserLogin u = new UserLogin() { Email = user.Email ,Password = user.Password};

            var userVerification = Login(u);

            if(userVerification != null) 
            { 
                try
                {
                    using (SqlConnection _connection = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = _connection.CreateCommand())
                        {
                            cmd.CommandText = "UpdatePassword";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@Password", user.NewPassword);

                            _connection.OpenAsync();
                            cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (using a logging framework) and rethrow or handle accordingly
                    throw new Exception("An error occurred while updating the user password.", ex);
                }
            }
            return Login(u);
        }
    }
}