using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var superAdminRoleId = "861cd6b8-8d70-4e83-8fb1-63ce4e8da7aa";              //Id should be static  because every time on model 
                                                                                        //creating run we want same super admin id for all apps and deployments
            var adminRoleId = "bc97fd2c - 0f04 - 489e-9ad3 - 9b421469ef24";
            var userRoleId = "df98e633-7d2f-4c7c-abb5-fa5ef83dd4ba";

            base.OnModelCreating(builder);

            //Seed Roles(User,Admin,Super Admin)
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },

                 new IdentityRole()
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },

                  new IdentityRole()
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed Super Admin User
            var superAdminId = "a5f85b91-e8e6-43dc-9886-5a3191c73855";
            var superAdminUser = new IdentityUser()
            {
                Id = superAdminId,
                UserName = "superadmin@blogweb.com",
                Email = "superadmin@blogweb.com",
                NormalizedEmail= "superadmin@blogweb.com".ToUpper(),
                NormalizedUserName= "superadmin@blogweb.com".ToUpper()
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().
                           HashPassword(superAdminUser, "superadmin123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All Roles to super Admin User
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                RoleId=superAdminRoleId,
                UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                RoleId=adminRoleId,
                UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                RoleId=userRoleId,
                UserId=superAdminId
                }



            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
            
    }
}
