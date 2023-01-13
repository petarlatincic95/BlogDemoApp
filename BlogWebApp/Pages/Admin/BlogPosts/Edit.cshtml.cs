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
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    { 
      
        private readonly IBlogPostRepository blogPostRepository;

        [BindProperty]
        public EditBlogPostReuest BlogPost { get; set; }

        [BindProperty]
        public IFormFile? FeaturedImage { get; set; }

        public EditModel(IBlogPostRepository _blogPostRepository)
        {

            blogPostRepository = _blogPostRepository;
        }
        [BindProperty]
        [Required]
        public string Tags { get; set; }


        public async Task OnGet(Guid id)
        {
            var blogPostDomainModel = await blogPostRepository.GetAsync(id);
            if(blogPostDomainModel!=null&&blogPostDomainModel.Tags!=null)
            {
                BlogPost = new EditBlogPostReuest
                {
                    Id = blogPostDomainModel.Id,
                    Heading = blogPostDomainModel.Heading,
                    PageTitle = blogPostDomainModel.PageTitle,
                    Content = blogPostDomainModel.Content,
                    ShortDescription = blogPostDomainModel.ShortDescription,
                    FeaturedImageUrl = blogPostDomainModel.FeaturedImageUrl,
                    UrlHandle = blogPostDomainModel.UrlHandle,
                    PublishedDate = blogPostDomainModel.PublishedDate,
                    Author = blogPostDomainModel.Author,
                    Visible = blogPostDomainModel.Visible,
                   
            };
                Tags = String.Join(',', blogPostDomainModel.Tags.Select(x => x.Name));

            }
            
        }
        public async Task<IActionResult> OnPostEdit()
        {
             ValidateEditBlogPost();
           
            if (ModelState.IsValid)
            {
                try
                {

                    var blogPostDomainModel = new BlogPost
                    {

                        Id = BlogPost.Id,
                        Heading = BlogPost.Heading,
                        PageTitle = BlogPost.PageTitle,
                        Content = BlogPost.Content,
                        ShortDescription = BlogPost.ShortDescription,
                        FeaturedImageUrl = BlogPost.FeaturedImageUrl,
                        UrlHandle = BlogPost.UrlHandle,
                        PublishedDate = BlogPost.PublishedDate,
                        Author = BlogPost.Author,
                        Visible = BlogPost.Visible,
                        Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim()}))

                    };

                    await blogPostRepository.UpdateAsync(blogPostDomainModel);

                    ViewData["Notification"] = new Notification
                    {
                        Message = "Record updated successfully!",
                        Type = Enums.NotificationType.Success
                    };

                }
                catch (Exception ex)
                {
                  

                    ViewData["Notification"] = new Notification
                    {
                        Message = "Something went wrong!",
                        Type = Enums.NotificationType.Error
                    };
                }

              

            }
            return Page();
            
        }
        public async Task<IActionResult> OnPostDelete()
        {
           var deleted= await blogPostRepository.DeleteAsync(BlogPost.Id);
            if (deleted)
            {
                var notification = new Notification
                {
                    Type = Enums.NotificationType.Success,
                    Message = "Blog was deleted successfully"
                };

                TempData["Notification"] = JsonSerializer.Serialize(notification);      //We have to make it like this because we cannot send complex data over TempData[]!
                return RedirectToPage("/Admin/BlogPosts/List");
            }
            return Page();
        }
        private void ValidateEditBlogPost()
        {
            if (!string.IsNullOrEmpty(BlogPost.Heading))
            {
                //check for min and max length--just example of custom validation
                if (BlogPost.Heading.Length < 10 || BlogPost.Heading.Length > 100)
                {
                        ModelState.AddModelError("BlogPost.Heading",
                        "Heading can only be between 10 and 72 characters");
                
                }
            
            }
        
        }
    }
}
