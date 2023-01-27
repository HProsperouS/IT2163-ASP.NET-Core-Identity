using FreshFarmMarket_211283E.Google;
using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;
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
		private readonly IHttpContextAccessor contxt;
        private readonly LogServices _logService ;

		public LoginModel(SignInManager<ApplicationUser> signInManager, 
                            GoogleCaptchaService googleCaptchaService, 
                            IHttpContextAccessor httpContextAccessor,
                            LogServices logServices
                            )
        {
            this.signInManager = signInManager;
            _googleCaptchaService= googleCaptchaService;
            contxt = httpContextAccessor;
            _logService = logServices;
        }

        public IActionResult OnGet()
        {
			return Page();
        }
        public async Task<IActionResult> OnPostNormal()
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
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,true, true);

                if (identityResult.Succeeded)
                {
					TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = "You have successfully LogIn";
                    await _logService.RecordLogs("Login",LModel.Email);

                    return RedirectToPage("Index");
                }
                else if (identityResult.IsLockedOut)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = "Your account have benn locked out";
                    return Page();
                }
                else if(identityResult.RequiresTwoFactor)
                {
					return RedirectToPage("Sms2FA");
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = "Invalid username or password";
                    return Page();
                }
            }
            return Page();
        }
		public IActionResult OnPostGoogleLogin()
		{
			return Challenge(signInManager.ConfigureExternalAuthenticationProperties("Google", "/GoogleLogin"), "Google");
		}

	}
}
