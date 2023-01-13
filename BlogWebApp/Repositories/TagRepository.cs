using BlogWebApp.Data;
using BlogWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Repositories
{
    public class TagRepository : ITagrepository
    {
        public TagRepository(BlogWebDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public BlogWebDbContext dbContext { get; }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var allTags = await dbContext.Tags.ToListAsync();
            var tags=allTags.DistinctBy(y => y.Name.ToLower()).ToList();
            return tags;
        }
    }
}
