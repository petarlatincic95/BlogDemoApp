using BlogWebApp.Models.Domain;

namespace BlogWebApp.Repositories
{
    public interface ITagrepository
    {
       Task<IEnumerable<Tag>> GetAllAsync();

    }
}
