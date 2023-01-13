using BlogWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(AuthDbContext _dbContext,
            UserManager<IdentityUser> userManager)
        {
            this.authDbContext = _dbContext;
            this.userManager = userManager;
        }

        public AuthDbContext authDbContext { get; }
        private UserManager<IdentityUser> userManager { get; }

        public async Task<bool> Add(IdentityUser identityUser, string password, List<string> roles)
        {
            var identityResult = await userManager.CreateAsync(identityUser, password);
            if (identityResult.Succeeded)
            {
                identityResult=await userManager.AddToRolesAsync(identityUser, roles);
                if (identityResult.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Delete(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            { 
                await userManager.DeleteAsync(user);
            }
        }

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users=await authDbContext.Users.ToListAsync();
            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@blogweb.com");
            if (superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }
            return users;
            
        }
    }
}
