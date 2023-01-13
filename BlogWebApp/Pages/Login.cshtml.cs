using BlogWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebApp.Pages
{
    public class LoginModel : PageModel
    {
        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        [BindProperty]
        public Login LoginViewModel { get; set; }
        public SignInManager<IdentityUser> signInManager { get; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //ReturnUrl(must be named like that) is parameter we get from Url if user want to go to
                //some page, but app asks for login, and based on that parameter we redirect to that page
                var signInResult = await signInManager.PasswordSignInAsync(
                      LoginViewModel.Username, LoginViewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return RedirectToPage(ReturnUrl);
                    }
                    else
                        return RedirectToPage("Index");
                }
                else
                {
                    ViewData["Notification"] = new Notification
                    {
                        Type = Enums.NotificationType.Error,
                        Message = "Unsuccesfull login atempt!"
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
