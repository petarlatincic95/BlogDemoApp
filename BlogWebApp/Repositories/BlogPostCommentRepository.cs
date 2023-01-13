using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        public BlogPostCommentRepository(BlogWebDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public BlogWebDbContext dbContext { get; }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
           await dbContext.BlogPostComments.AddAsync(blogPostComment); 
           await dbContext.SaveChangesAsync();
           return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetAllAsync(Guid blogPostId)
        {
           return await dbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
