using System.ComponentModel.DataAnnotations;

namespace MusicShop.Models
{
    public class UserSignupDTO
    {

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }
    }
};