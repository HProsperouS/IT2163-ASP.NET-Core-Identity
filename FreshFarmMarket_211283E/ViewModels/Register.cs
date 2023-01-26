using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreshFarmMarket_211283E.ViewModels
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.CreditCard)]
        // Visa Credit Card Format
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "PLease enter your credit card number in visa format, 12 digits")]
        public string CreditCardNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string Gender{ get; set; } = string.Empty;

        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;


        [MaxLength(50)]
        public string? Photo { get; set; } = string.Empty;

        [Required]
        public string AboutMe { get; set; } = string.Empty;




    }


}


