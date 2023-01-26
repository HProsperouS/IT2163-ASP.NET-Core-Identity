using FreshFarmMarket_211283E.Services;
using FreshFarmMarket_211283E.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.Pages
{
    public class Sms2FAModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MessageService _messageService;

        public Sms2FAModel(UserManager<ApplicationUser> userManager, MessageService messageService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _messageService = messageService;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public string OTP { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "PhoneSMS");
            await _messageService.SendSmsAsync(user.PhoneNumber, token);

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _signInManager.TwoFactorSignInAsync("PhoneSMS", OTP,false,false);
            if (result.Succeeded)
            {

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "You have successfully LogIn";

                return Redirect("/Index");
            }
            else if (result.IsLockedOut)
            {
                return Redirect("/LockedOut");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Invalid OTP";
                return Page();
            }
        }
    }
}
