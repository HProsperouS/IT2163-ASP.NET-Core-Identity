using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket_211283E.ViewModels
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty.ToString();
        public bool RememberMe { get; set; } 

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
