using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        public BlogPostLikeRepository(BlogWebDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        private BlogWebDbContext dbContext { get; }

        public async Task AddLikeForBlog(Guid blogPostId, Guid userId)
        {
            var like = new BlogPostLike
            {
                Id = Guid.NewGuid(),
                BlogPostId = blogPostId,
                UserId = userId
            };
            await dbContext.BlogPostLikes.AddAsync(like);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
           return  await dbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikesForBlog(Guid blogPostId)
        {
             return (await dbContext.BlogPostLikes.CountAsync(x=>x.BlogPostId==blogPostId));
        }
    }
}
