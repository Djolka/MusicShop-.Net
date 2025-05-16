using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MusicShop.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "CustomerID is required.")]
        public required string CustomerId { get; set; }

        [Required(ErrorMessage = "Products are required.")]
        public required IEnumerable<Product> Products { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public required DateTime Date { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        public required double TotalPrice { get; set; }
    }
}