using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreshFarmMarket_211283E.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Disallow special characters")]
        [DataType(DataType.Text)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.CreditCard)]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "PLease enter your credit card number in visa format, 12 digits")]
        public string CreditCardNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string Gender{ get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Disallow special characters")]
        [DataType(DataType.Text)]
        public string DeliveryAddress { get; set; } = string.Empty;


        [MaxLength(50)]
        public string? Photo { get; set; } = string.Empty;

        [Required]
        public string AboutMe { get; set; } = string.Empty;

		public DateTime? lastPasswordChange { get; set; } 

		public string? PreviousPasswordHash1 { get; set; }

		public string? PreviousPasswordHash2 { get; set; }

	}


}


