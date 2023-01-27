using FreshFarmMarket_211283E.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.Pages
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        private  readonly IWebHostEnvironment _environment;


        private readonly IDataProtector _protector;

        [BindProperty]
        public ApplicationUser RModel { get; set; }

        [BindProperty]
        [Required]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9])(?=.*[a-z]).{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least one uppercase letter, one special character, one number, and one lowercase letter.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [BindProperty]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Invalid Phone Number Format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public RegisterModel(UserManager<ApplicationUser> userManager, 
                            SignInManager<ApplicationUser> signInManager, 
                            IWebHostEnvironment environment,
                            IDataProtectionProvider provider
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            _protector = provider.CreateProtector("CardNumberProtector");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload == null)
                {
                    return Page();
                }
                if (Upload.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                    return Page();
                }

                // Check for JPG Only, 
                var fileExtension = Path.GetExtension(Upload.FileName).ToLower();
                if (fileExtension != ".jpg")
                {
                    // file is a JPG, process it
                    ModelState.AddModelError("Upload", "JPG Only");
                    return Page();

                }

                var uploadsFolder = "uploads";
                var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await Upload.CopyToAsync(fileStream);
                var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                var secureCreditCardNumber = _protector.Protect(RModel.CreditCardNumber);

                var user = new ApplicationUser
                {
                    UserName = Email,
                    Photo = ImageURL,
                    FullName = RModel.FullName,
                    Gender = RModel.Gender,
                    Email = Email,
                    PhoneNumber = "+65" + PhoneNumber,
                    DeliveryAddress = RModel.DeliveryAddress,
                    CreditCardNumber = secureCreditCardNumber,
                    AboutMe = RModel.AboutMe,
                    TwoFactorEnabled = true,
                    PhoneNumberConfirmed= true
                };


                var result = await userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = "You have successfully registed for an account";
                    return RedirectToPage("/Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }
            return Page();
        }
        
    }
}
