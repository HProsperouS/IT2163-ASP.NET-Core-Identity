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
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        // Visa Credit Card Format
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "PLease enter your credit card number in visa format, 12 digits")]
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender{ get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Invalid Phone Number Format")]
        public string PhoneNumber { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string? Photo { get; set; }

        [Required]
        public string AboutMe { get; set; }


        

    }


}


