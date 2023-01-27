using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FreshFarmMarket_211283E.Models;

namespace FreshFarmMarket_211283E.Pages;

public class GoogleLoginModel : PageModel
{
	private readonly SignInManager<ApplicationUser> _signInManager;

	public GoogleLoginModel(SignInManager<ApplicationUser> signInManager)
	{
		_signInManager = signInManager;
	}

	public async Task<IActionResult> OnGet(bool isPersistent)
	{
		var data = await _signInManager.GetExternalLoginInfoAsync();
		var result = await _signInManager.ExternalLoginSignInAsync(data.LoginProvider, data.ProviderKey, isPersistent, true);

		if (result.Succeeded)
		{
			return Redirect("/Index");
		}

		var email = data.Principal.FindFirstValue(ClaimTypes.Email);
		if (email is not null)
		{
			var user = await _signInManager.UserManager.FindByEmailAsync(email);
			if (user is null)
			{
				user = new ApplicationUser
				{
					UserName = data.Principal.FindFirstValue(ClaimTypes.Email),
					Email = data.Principal.FindFirstValue(ClaimTypes.Email),
				};
				await _signInManager.UserManager.CreateAsync(user);
			}
			await _signInManager.UserManager.AddLoginAsync(user, data);
			await _signInManager.SignInAsync(user, isPersistent);
		}
		return Redirect("/Index");
	}
}