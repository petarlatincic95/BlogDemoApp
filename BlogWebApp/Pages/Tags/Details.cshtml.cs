using BlogWebApp.Models.Domain;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebApp.Pages.Tags
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        public List<BlogPost> Blogs { get; set; }

        public IBlogPostRepository blogPostRepository { get; }

        public async Task<IActionResult> OnGet(string tagName)
        {
           Blogs= (List<BlogPost>)await blogPostRepository.GetAllAsync(tagName);
           return Page();
        }
    }
}
