using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;

namespace FreshFarmMarket_211283E.Pages;

public class GoogleLoginModel : PageModel
{
	private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly LogServices _logService;

    public GoogleLoginModel(SignInManager<ApplicationUser> signInManager, LogServices logServices)
	{
		_signInManager = signInManager;
		_logService= logServices;
	}

	public async Task<IActionResult> OnGet(bool isPersistent)
	{
		var data = await _signInManager.GetExternalLoginInfoAsync();
		var result = await _signInManager.ExternalLoginSignInAsync(data.LoginProvider, data.ProviderKey, isPersistent, true);

        var email = data.Principal.FindFirstValue(ClaimTypes.Email);

        if (result.Succeeded)
		{
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "You have successfully LogIn";
            await _logService.RecordLogs(Actions.Login, email);
            HttpContext.Session.SetString("SessionEmail", email);
            return RedirectToPage("/Index");
		}

		if (email is not null)
		{
			var user = await _signInManager.UserManager.FindByEmailAsync(email);
			if (user is null)
			{
				user = new ApplicationUser
				{
                    UserName = data.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = data.Principal.FindFirstValue(ClaimTypes.Email),
                    PhoneNumber = data.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                    FullName = data.Principal.FindFirstValue(ClaimTypes.Name)
                };
				await _signInManager.UserManager.CreateAsync(user);

            }
            await _logService.RecordLogs(Actions.Login, user.Email);
            await _signInManager.UserManager.AddLoginAsync(user, data);
			await _signInManager.SignInAsync(user, isPersistent);

            await _logService.RecordLogs(Actions.Login, email);
            HttpContext.Session.SetString("SessionEmail", email);
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "You have successfully LogIn";
        }

        return RedirectToPage("/Index");
	}
}