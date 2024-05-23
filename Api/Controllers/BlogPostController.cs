using Bll.Services.BlogPostServiceBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dto.BlogPostDto;
using Models.Entity.PostModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public IEnumerable<BlogPost> Get()
        {
            return _blogPostService.Get();
        }

        [HttpGet("{id:Guid}")]
        public BlogPost GetById(Guid id)
        {
            return _blogPostService.GetById(id);
        }

        [HttpPost]
        public BlogPost Add(CreateBlogPost post)
        {
            return _blogPostService.Add(post);
        }

        [HttpPut]
        public BlogPost Update(UpdateBlogPost post,Guid id) 
        {
            return _blogPostService.Update(post,id);
        }

        [HttpDelete]
        public bool Delete(Guid id)
        {
            return _blogPostService.Delete(id);
        }

    }
}
