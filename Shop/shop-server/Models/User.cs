using System.ComponentModel.DataAnnotations;

namespace MusicShop.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        public string Role {  get; set; } = string.Empty;
    }
}