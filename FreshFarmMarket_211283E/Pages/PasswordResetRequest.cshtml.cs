using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Twilio.Jwt.AccessToken;

namespace FreshFarmMarket_211283E.Pages
{
	[AllowAnonymous]
	public class PasswordResetRequestModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly MessageService _messageService;

		public PasswordResetRequestModel(UserManager<ApplicationUser> userManager, MessageService messageService)
		{
			_userManager = userManager;
			_messageService = messageService;

		}

		[BindProperty]
		public string? Email { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.FindByEmailAsync(Email);
			if (user == null)
			{
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "SMS has successfully sent to your phone number if your email is valid";
                return Redirect("/PasswordResetRequest");
            }

			if(await _userManager.IsLockedOutAsync(user))
			{
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Your account is lockedout so you are not allow to reset";

                return Page();
            }
			var code = await _userManager.GeneratePasswordResetTokenAsync(user);
			var userId = user.Id;
			var callbackUrl = Url.Page(
				"/ResetPassword",
				pageHandler: null,
				values: new { code, userId},
				protocol: Request.Scheme);

            await _messageService.SendSmsAsync(
				user.PhoneNumber,
                $"Please reset your password by visiting this URL: {callbackUrl}");

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "SMS has successfully sent to your phone number if your email is valid";
            return Redirect("/PasswordResetRequest");
		}
	}
}
