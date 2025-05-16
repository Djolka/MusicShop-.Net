using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShop.Models
{
    public class Favourite
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "CustomerID is required.")]
        public required string CustomerId { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [ForeignKey("ProductId")]
        public required Product Product { get; set; } 
    }
}