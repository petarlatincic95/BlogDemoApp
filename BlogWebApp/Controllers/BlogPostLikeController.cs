using BlogWebApp.Models.ViewModels;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostLikeController : Controller
    {
        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        public IBlogPostLikeRepository blogPostLikeRepository { get; set; }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddBlogPostLikeRequest addBlogPostLikeRequest)
        {
            await blogPostLikeRepository.AddLikeForBlog(addBlogPostLikeRequest.BlogPostId,
                                                           addBlogPostLikeRequest.UserId);
            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GtTotlLikes([FromRoute] Guid blogPostId)
        { 
            var totalLikes=await blogPostLikeRepository.GetTotalLikesForBlog(blogPostId);
            return Ok(totalLikes);
       
        }
    }
}
