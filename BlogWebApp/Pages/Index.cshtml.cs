using BlogWebApp.Models.Domain;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagrepository tagRepository;

        public List<BlogPost> Blogs { get; set; }
        public List<Tag> Tags { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IBlogPostRepository blogPostRepository,
                         ITagrepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            
        }

        public async Task<IActionResult> OnGet()
        {
           Blogs= (await blogPostRepository.GetAllAsync()).ToList();
           Tags = (List<Tag>)await tagRepository.GetAllAsync();
           return Page();
        }
    }
}