using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicShop.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public required double Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public required string Type { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public required string Color { get; set; }

        [Required(ErrorMessage = "Manufacturer is required.")]
        public required string Manufacturer { get; set; }

        public required string CountryOfOrigin { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
        public required IEnumerable<string> Picture { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public required int Quantity { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
    }
}