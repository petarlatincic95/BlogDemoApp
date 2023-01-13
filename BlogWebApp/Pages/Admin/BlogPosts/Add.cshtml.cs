using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using BlogWebApp.Models.Domain.VievModels;
using BlogWebApp.Models.ViewModels;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BlogWebApp.Pages.Admin.BlogPosts
{
    [Authorize(Roles ="Admin")]
    public class AddModel : PageModel
    {
        private readonly IBlogPostRepository blogPostRepository;
      
        [BindProperty]
        public AddBlogPost AddBlogPostRequest { get; set; }
       
        [BindProperty]
        public IFormFile? FeaturedImage { get; set; }

        [BindProperty]
        [Required]
        public string Tags { get; set; }

        public AddModel(IBlogPostRepository _blogPostRepository)
        {

            blogPostRepository = _blogPostRepository;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult>OnPost()
        {
            
            //ValidateAddBlogPost();
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost()
                {
                    Heading = AddBlogPostRequest.Heading,
                    PageTitle = AddBlogPostRequest.PageTitle,
                    Content = AddBlogPostRequest.Content,
                    ShortDescription = AddBlogPostRequest.ShortDescription,
                    FeaturedImageUrl = AddBlogPostRequest.FeaturedImageUrl,
                    UrlHandle = AddBlogPostRequest.UrlHandle,
                    PublishedDate = AddBlogPostRequest.PublishedDate,
                    Author = AddBlogPostRequest.Author,
                    Visible = AddBlogPostRequest.Visible,
                    Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim() }))
                };
                await blogPostRepository.AddAsync(blogPost);
                var notification = new Notification
                {
                    Type = Enums.NotificationType.Success,
                    Message = "New Blog Post created"
                };

                TempData["Notification"] = JsonSerializer.Serialize(notification);      //We have to make it like this because we cannot send complex data over TempData[]!
                return RedirectToPage("/Admin/BlogPosts/List");

            }
            return Page();
           
        }
        private void ValidateAddBlogPost()
        {
            if (AddBlogPostRequest.PublishedDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("AddBlogPostRequest.PublishedDate",
                    $"PublishedDate can only be todays date or future date");
            }
        }
    }
}
