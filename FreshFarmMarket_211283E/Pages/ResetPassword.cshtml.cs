using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Twilio.Jwt.AccessToken;

namespace FreshFarmMarket_211283E.Pages
{
    public class ResetPasswordModel : PageModel
    {
		private UserManager<ApplicationUser> _userManager { get; }
        private PasswordResetServices _passwordResetServices;

		public ResetPasswordModel(UserManager<ApplicationUser> userManager, PasswordResetServices passwordResetServices)
        {
            _userManager = userManager;
            _passwordResetServices = passwordResetServices;
        }

		[BindProperty]
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9])(?=.*[a-z]).{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least one uppercase letter, one special character, one number, and one lowercase letter.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Code { get; set; }

        [BindProperty]
        public string UserID { get; set; }

        public IActionResult OnGet(string code, string userId)
        {

            
            
            Code = code;
            UserID = userId;
            return Page();
            

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserID);
            string oldPassword = user.PasswordHash;

            if (user == null)
            {
                return NotFound($"unable to load user with id '{UserID}'.");
            }

            var testResult = _passwordResetServices.VerifyPasswordProcess(user, Password);

            if (!testResult.Data)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = testResult.Message;
                return Page();
            }
            else
            {
                var result = await _userManager.ResetPasswordAsync(user, Code, Password);
                if (result.Succeeded)
                {
                    user.lastPasswordChange = DateTime.UtcNow;

                    // Store the new password hash as the most recent password hash
                    user.PreviousPasswordHash1 = oldPassword;

                    // Shift the second most recent password hash to be the previous password hash
                    user.PreviousPasswordHash2 = user.PreviousPasswordHash1;

                    // Save the changes to the user
                    await _userManager.UpdateAsync(user);

                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = "You have successfully changed your password";
                    return RedirectToPage("/Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
             

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return Page();
        }
    }
}
