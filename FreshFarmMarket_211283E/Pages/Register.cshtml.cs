using FreshFarmMarket_211283E.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket_211283E.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<Register> userManager { get; }
        private SignInManager<Register> signInManager { get; }

        private IWebHostEnvironment _environment;

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public RegisterModel(UserManager<Register> userManager, SignInManager<Register> signInManager, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            
        }

        public void OnGet()
        { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                    var user = new Register
                    {
                        UserName = RModel.FullName,
                        Photo = ImageURL,
                        FullName = RModel.FullName,
                        Gender = RModel.Gender,
                        EmailAddress = RModel.EmailAddress,
                        PhoneNumber = RModel.PhoneNumber,
                        Password = RModel.Password,
                        ConfirmPassword = RModel.ConfirmPassword,
                        DeliveryAddress = RModel.DeliveryAddress,
                        CreditCardNumber = RModel.CreditCardNumber,
                        AboutMe = RModel.AboutMe,
                    };


                    var result = await userManager.CreateAsync(user, RModel.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToPage("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return Page();
        }
        
    }
}
