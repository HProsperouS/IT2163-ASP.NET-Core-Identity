using FreshFarmMarket_211283E.Google;
using FreshFarmMarket_211283E.Models;
using FreshFarmMarket_211283E.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FreshFarmMarket_211283E.Pages
{
    [Authorize(Roles = "Admin")]
    public class ViewLogsModel : PageModel
    {
        private readonly LogServices _logService;
        private UserManager<ApplicationUser> userManager { get; }
        private RoleManager<IdentityRole> roleManager { get; }
        public ViewLogsModel(LogServices logServices, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logService = logServices;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public List<Log> LogsList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            LogsList = await _logService.RetrieveAllLogs();

            //await roleManager.CreateAsync(new IdentityRole("Admin"));

            //var userId = (await userManager.GetUserAsync(HttpContext.User)).Id;
            //var user = await userManager.Users.FirstAsync(u => u.Id == userId);
            //if (!await userManager.IsInRoleAsync(user, "Admin"))
            //{
            //    // User is not in the "Admin" role
            //    var result = await userManager.AddToRoleAsync(user, "Admin");

            //    if (result.Succeeded)
            //    {
            //        // User was successfully added to the "Admin" role
            //        TempData["FlashMessage.Type"] = "success";
            //        TempData["FlashMessage.Text"] = "user has added to Admin";
            //        return RedirectToPage("/Index");
            //    }
            //    else
            //    {
            //        // There was an error adding the user to the "Admin" role
            //    }
            //}
            return Page();
        }

    }
}
