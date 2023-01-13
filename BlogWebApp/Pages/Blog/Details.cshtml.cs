using BlogWebApp.Models.Domain;
using BlogWebApp.Models.Domain.VievModels;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public DetailsModel(IBlogPostRepository blogPostRepository,
                    IBlogPostLikeRepository blogPostLikeRepository,
                   SignInManager<IdentityUser> signInManager,
                   UserManager<IdentityUser> userManager,IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        private IBlogPostRepository blogPostRepository { get; }
        public IBlogPostLikeRepository blogPostLikeRepository { get; }
        public SignInManager<IdentityUser> signInManager { get; }
        public UserManager<IdentityUser> userManager { get; }

        [BindProperty]
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public string CommentDesciption { get; set; }

        [BindProperty]
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int BlogPostTotalLikes { get; set; }
        public List<BlogComment> Comments { get; set; }


        public bool isLiked { get; set; }
        public async Task<IActionResult> OnGet(string urlHandle)
        {
           await GetBlog(urlHandle);
            return Page();
        }
        public async Task<IActionResult> OnPost(string urlHandle)
        {

            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                if (signInManager.IsSignedIn(User) && !string.IsNullOrWhiteSpace(CommentDesciption))
                {
                    var comment = new BlogPostComment()
                    {
                        BlogPostId = BlogPostId,
                        Description = CommentDesciption,
                        DateAdded = DateTime.Now,
                        UserId = Guid.Parse(userId)
                    };
                    await blogPostCommentRepository.AddAsync(comment);

                }
                //Redirrecting to get method(Common practice )
                return RedirectToPage("/Blog/Details", new { urlHandle = urlHandle });

            }
            await GetBlog(urlHandle);
            return Page();
            
        }

        private async Task GetComments()
        {
            var blogPostComments = await blogPostCommentRepository.GetAllAsync(BlogPost.Id);
            var blogCommentsViewModel = new List<BlogComment>();
            foreach (var blogPostComment in blogPostComments)
            {
                blogCommentsViewModel.Add(new BlogComment
                {
                    DateAdded = blogPostComment.DateAdded,
                    Description = blogPostComment.Description,
                    UserName = (await userManager.FindByIdAsync(blogPostComment.UserId.ToString())).UserName
                });
            
            }
            Comments = blogCommentsViewModel;
        }

        private async Task GetBlog(string urlHandle)
        {

            BlogPost = await blogPostRepository.GetAsync(urlHandle);
            if (BlogPost != null)
            {
                BlogPostId = BlogPost.Id;
                if (signInManager.IsSignedIn(User))
                {
                    var likes = await blogPostLikeRepository.GetLikesForBlog(BlogPost.Id);
                    var userId = userManager.GetUserId(User);
                    isLiked = likes.Any(x => x.UserId == Guid.Parse(userId));
                    await GetComments();


                }

                BlogPostTotalLikes = await blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id);

            }
      

        }
          
         
    }
}

