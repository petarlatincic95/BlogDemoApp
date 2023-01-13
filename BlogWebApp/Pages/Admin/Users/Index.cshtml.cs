using BlogWebApp.Data;
using BlogWebApp.Models.Domain.VievModels;
using BlogWebApp.Models.ViewModels;
using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebApp.Pages.Admin.Users
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        public IndexModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IUserRepository userRepository { get; }
        public List<User> Users { get; set; }

        [BindProperty]
        public AddUser AddUserRequest { get; set; }

        [BindProperty]
        public Guid SelectedUserId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await GetUsers();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = AddUserRequest.UserName,
                    Email = AddUserRequest.Email
                };
                var roles = new List<string> { "User" };
                if (AddUserRequest.AdminCheckBox == true)
                {
                    roles.Add("Admin");
                }
                var result = await userRepository.Add(identityUser, AddUserRequest.Password, roles);
                if (result == true)
                {
                    return RedirectToPage("/Admin/Users/Index");

                }
                return Page();

            }
            await GetUsers();
            return Page();

        }
        public async Task<IActionResult> OnPostDelete()
        {
            await userRepository.Delete(SelectedUserId);
            return RedirectToPage("/Admin/Users/Index");
        }
        private async Task GetUsers()
        {
            var users = await userRepository.GetAll();
            Users = new List<User>();
            foreach (var user in users)
            {
                Users.Add(new Models.Domain.VievModels.User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email

                });

            }

        }
    }
}
