using Microsoft.Data.SqlClient;
using Models.Dto.BlogPostDto;
using Models.Entity.PostModels;
using Models.Entity.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository.BlogpostDal
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly string ConnectionString = "Server=PCZAC\\DATAVIZ;Database=ExoAPI2024;User Id=PCZAC\\boual;Password= ;TrustServerCertificate=True;Trusted_Connection=True";


        public BlogPost Add(CreateBlogPost entity)
        {
            BlogPost blogPost = null;

            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        // Définir le nom de la procédure stockée
                        cmd.CommandText = "AddBlogPost";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Ajouter les paramètres nécessaires
                        cmd.Parameters.AddWithValue("@Title", blogPost.Title);
                        cmd.Parameters.AddWithValue("@Content", blogPost.Content);
                        cmd.Parameters.AddWithValue("@CreatedAt", blogPost.CreatedAt);
                        cmd.Parameters.AddWithValue("@UserId", blogPost.UserId);

                        _connection.Open();

                        var Id = (Guid)cmd.ExecuteScalar();

                        BlogPost blogPostFromDb = null;
                        if (Id != null)
                        {
                            blogPostFromDb = GetById(Id);
                            return blogPostFromDb;
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
                throw new Exception("An error occurred while adding the blogPost : " + blogPost.Id, ex);
            }
        }


        public IEnumerable<BlogPost> Get()
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM [BlogPost]";
                        List<BlogPost> blogs = new List<BlogPost>();
                        _connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BlogPost blog = new BlogPost
                                {
                                    Id = reader.IsDBNull("Id") ? Guid.Empty : reader.GetGuid("Id"),
                                    Title = reader.IsDBNull("Title") ? string.Empty : reader.GetString("Title"),
                                    Content = reader.IsDBNull("Content") ? string.Empty : reader.GetString("Content"),
                                    CreatedAt = reader.IsDBNull("CreatedAt") ? DateTime.Now : reader.GetDateTime("CreatedAt"),
                                    UserId = reader.IsDBNull("Email") ? Guid.Empty : reader.GetGuid("UserId"),
                                };
                                blogs.Add(blog);
                            }
                        }

                        return blogs;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving BlogPost list: {ex.Message}");
                return new List<BlogPost>(); // Return an empty list on error
            }
        }


        public BlogPost GetById(Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM BlogPost WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        BlogPost blog = null;
                        _connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                blog = new BlogPost
                                {
                                    Id = reader.IsDBNull("Id") ? Guid.Empty : reader.GetGuid("Id"),
                                    Title = reader.IsDBNull("Title") ? string.Empty : reader.GetString("Title"),
                                    Content = reader.IsDBNull("Content") ? string.Empty : reader.GetString("Content"),
                                    CreatedAt = reader.IsDBNull("CreatedAt") ? DateTime.Now : reader.GetDateTime("CreatedAt"),
                                    UserId = reader.IsDBNull("Email") ? Guid.Empty : reader.GetGuid("UserId"),
                                };
                            }
                        }

                        return blog;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving blogPost with ID {id}: {ex.Message}");
                return null;
            }
        }


        public BlogPost Update(UpdateBlogPost blogPost, Guid id)
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
                            cmd.CommandText = "UpdateBlogPost";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Title", blogPost.Title);
                            cmd.Parameters.AddWithValue("@Content", blogPost.Content);
                            cmd.Parameters.AddWithValue("@CreatedAt", blogPost.CreatedAt);
                            cmd.Parameters.AddWithValue("@UserId", blogPost.UserId);


                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                transaction.Commit();

                                BlogPost? updatedBlogPost = GetById(id);
                                return updatedBlogPost;
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
                throw new Exception("An error occurred while updating the BlogPost." + id, ex);
            }
        }


        public bool Delete(Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [BlogPost] SET IsActive = 0 WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        _connection.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred blogPost : {id} not deleted : {ex.Message} Dal");
                return false;
            }
        }


        public bool DeleteFromDatabaseAsync(Guid id)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(ConnectionString))
                {
                    _connection.Open();
                    using (SqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM BlogPost WHERE Id  = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);

                        var result = cmd.ExecuteNonQuery();

                        //add begin and rollback
                        if ((result != 0) && (result < 2))
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
                throw new Exception("An error occurred while deleting the blogPost." + id, ex);
            }
        }
    }
}
