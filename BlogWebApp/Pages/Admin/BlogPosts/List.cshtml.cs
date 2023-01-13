using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using BlogWebApp.Models.ViewModels;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlogWebApp.Pages.Admin.BlogPosts
{
    [Authorize(Roles = "Admin")]
    public class ListModel : PageModel
    {
        public List<BlogPost> BlogPosts { get; set; }
        public ListModel(IBlogPostRepository _blogPostRepository)
        {
           
            blogPostRepository = _blogPostRepository;
        }

        public BlogWebDbContext dbContext { get; }
        public IBlogPostRepository blogPostRepository { get; }

        public async Task OnGet()
        {
           var notificationJson = (string)TempData["Notification"];
            if (notificationJson != null)
            {
                ViewData["Notification"] = JsonSerializer.Deserialize<Notification>(notificationJson);
            
            }
           BlogPosts=(await blogPostRepository.GetAllAsync())?.ToList();   //Our method returns IEnumerable, and BlogPosts is List, so we have to check if conversion is possible
        }
    }
}
