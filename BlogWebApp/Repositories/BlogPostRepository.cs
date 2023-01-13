using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public BlogPostRepository(BlogWebDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public BlogWebDbContext dbContext { get; }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.FindAsync(id);
            if (existingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return true;
            }
                return false;
          
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string tagName)
        {
            return await dbContext.BlogPosts.Include(x => x.Tags)
                         .Where(b=>b.Tags.Any(y=>y.Name==tagName)).ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task<BlogPost> GetAsync(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(y => y.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;

               
                if (blogPost.Tags != null && blogPost.Tags.Any())
                {
                    //Delete the existing tags
                    dbContext.Tags.RemoveRange(existingBlogPost.Tags);
                    //Add new tags
                    blogPost.Tags.ToList().ForEach(x => x.BlogPostId = existingBlogPost.Id);
                    await dbContext.Tags.AddRangeAsync(blogPost.Tags);
                }
              
            }
            await dbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
