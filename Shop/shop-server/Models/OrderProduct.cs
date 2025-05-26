using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicShop.Models
{
    public class OrderProduct
    {
        public string? OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }

        public string? ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public required int Quantity { get; set; }
    }
}
