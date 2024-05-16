using Microsoft.Data.SqlClient;
using Models.Entity;
using System;
using System.Collections.Generic;

namespace Dal.Repository.UserDal
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = new List<User>();

            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [User]";

                _connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                        };

                        users.Add(user);
                    }
                }

                _connection.Close();
            }

            return users;
        }

        public User GetById(Guid id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [User] WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    User user = new User();

                    while (reader.Read())
                    {

                        user.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                        user.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        user.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        user.Email = reader.GetString(reader.GetOrdinal("Email"));
                        user.Password = reader.GetString(reader.GetOrdinal("Password"));
                        

                    }
                        return user;

                }
            }
        }

        public void AddUser(CreateUser user)
        {
                _connection.Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [User] (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password)";
              
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                cmd.ExecuteNonQuery();
            }
                _connection.Close();
        }

        public void UpdateUser(CreateUser user , Guid Id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                _connection.Open();
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void DeleteUser(Guid Id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE [User] SET IsActive = 0 WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", Id);

                _connection.Open();
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
