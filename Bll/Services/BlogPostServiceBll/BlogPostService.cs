using Dal.Repository.BlogpostDal;
using Models.Dto.BlogPostDto;
using Models.Entity.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services.BlogPostServiceBll
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostService(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public BlogPost Add(CreateBlogPost user)
        {
            return _blogPostRepository.Add(user);
        }

        public bool Delete(Guid id)
        {
            return _blogPostRepository.Delete(id);
        }

        public bool DeleteFromDatabaseAsync(Guid id)
        {
            return _blogPostRepository.DeleteFromDatabaseAsync(id);
        }

        public IEnumerable<BlogPost> Get()
        {
            return _blogPostRepository.Get();
        }

        public BlogPost GetById(Guid id)
        {
            return _blogPostRepository.GetById(id);

        }

        public BlogPost Update(UpdateBlogPost user, Guid id)
        {
            return _blogPostRepository.Update(user, id);
        }
    }
}
