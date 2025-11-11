using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MusicShop.Models
{
    public class OrderDTO
    {
        [Required]
        public required string CustomerId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required double TotalPrice { get; set; }

        public ICollection<OrderProductDTO> OrderProducts { get; set; } = new List<OrderProductDTO>();
    }

}