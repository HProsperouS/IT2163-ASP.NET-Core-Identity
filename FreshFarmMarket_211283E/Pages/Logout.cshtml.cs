using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket_211283E.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly LogServices _logService;
		private readonly UserManager<ApplicationUser> _userManager;

		public LogoutModel(SignInManager<ApplicationUser> signInManager,
            LogServices logServices,
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            _logService = logServices;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
 
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToPage("/Login");
            }
			await _logService.RecordLogs(Actions.Logout, user.Email);
			await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
		public IActionResult OnPostDontLogout()
		{
			return RedirectToPage("/Index");
		}

	}
}
