using Models.Dto.BlogPostDto;
using Models.Entity.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services.BlogPostServiceBll
{
    public interface IBlogPostService 
    {
        IEnumerable<BlogPost> Get();

        BlogPost GetById(Guid id);

        BlogPost Add(CreateBlogPost user);

        BlogPost Update(UpdateBlogPost user, Guid id);

        bool Delete(Guid id);

        bool DeleteFromDatabaseAsync(Guid id);
    }
}
