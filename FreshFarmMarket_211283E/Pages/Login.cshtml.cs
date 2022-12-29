using FreshFarmMarket_211283E.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket_211283E.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<Register> signInManager;
        public LoginModel(SignInManager<Register> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
               LModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }

    }
}
