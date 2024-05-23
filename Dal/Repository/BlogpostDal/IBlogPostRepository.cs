using Models.Dto.BlogPostDto;
using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.PostModels;
using Models.Entity.User;

namespace Dal.Repository.BlogpostDal
{
    public interface IBlogPostRepository 
    {
        IEnumerable<BlogPost> Get();

        BlogPost GetById(Guid id);

        BlogPost Add(CreateBlogPost user);

        BlogPost Update(UpdateBlogPost user, Guid id);

        bool Delete(Guid id);

        bool DeleteFromDatabaseAsync(Guid id);

    }
}
