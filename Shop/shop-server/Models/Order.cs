using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MusicShop.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public required string CustomerId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required double TotalPrice { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

}