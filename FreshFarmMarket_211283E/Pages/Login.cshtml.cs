using FreshFarmMarket_211283E.Google;
using FreshFarmMarket_211283E.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket_211283E.Pages
{
    public class LoginModel : PageModel
    {
		private readonly GoogleCaptchaService _googleCaptchaService;


		[BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager, GoogleCaptchaService googleCaptchaService)
        {
            this.signInManager = signInManager;
            _googleCaptchaService= googleCaptchaService;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
			//Verify response token with Google
			var captchaResult = await _googleCaptchaService.VerifyToken(LModel.Token);
            Console.Write(captchaResult);
            if (!captchaResult)
            {
                return Page();
            }

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
