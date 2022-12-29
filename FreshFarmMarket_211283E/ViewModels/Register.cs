using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreshFarmMarket_211283E.ViewModels
{
    public class Register:IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender{ get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [MaxLength(50)]
        public string? Photo { get; set; }

        [Required]
        public string AboutMe { get; set; }

    }


}
