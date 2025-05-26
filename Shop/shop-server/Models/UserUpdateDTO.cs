using System;
using System.ComponentModel.DataAnnotations;

namespace MusicShop.Models
{
    public class UserUpdateDTO
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
    }
}