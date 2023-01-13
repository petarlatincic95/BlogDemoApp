using BlogWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebApp.Pages
{
    public class RegisterModel : PageModel
    {
        //We have to inject UserManager class to enable Identity to register new user
        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        [BindProperty]
        public Register RegisterViewModel { get; set; }
        public UserManager<IdentityUser> userManager { get; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = RegisterViewModel.Username,
                    Email = RegisterViewModel.Email,
                };

                var identityResult = await userManager.CreateAsync(user, RegisterViewModel.Password);



                if (identityResult.Succeeded)
                {
                    var addRolesResult = await userManager.AddToRoleAsync(user, "User");
                    if (addRolesResult.Succeeded)
                    {
                        ViewData["Notification"] = new Notification
                        {
                            Type = Enums.NotificationType.Success,
                            Message = "User registered succesfully"
                        };

                    }
                    return Page();
                }

                else
                {
                    ViewData["Notification"] = new Notification
                    {
                        Type = Enums.NotificationType.Error,
                        Message = "User registration unsuccesfull!!"
                    };
                    return Page();



                }

            }
            else 
            {
                return Page();
            }

        }
    }
}
