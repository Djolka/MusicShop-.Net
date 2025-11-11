using System;
using System.ComponentModel.DataAnnotations;

namespace MusicShop.Models
{
    public class UserLoginDTO
    {
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}