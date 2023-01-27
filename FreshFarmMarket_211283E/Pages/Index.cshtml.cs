using FreshFarmMarket_211283E.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;


        public IndexModel(UserManager<ApplicationUser> userManager, IDataProtectionProvider provider)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("CardNumberProtector");

        }
        public ApplicationUser user { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            var decryptedCreditCardNumber = _protector.Unprotect(user.CreditCardNumber);
            user.CreditCardNumber = decryptedCreditCardNumber;
          
            return Page();
        }
       
        
    }
}