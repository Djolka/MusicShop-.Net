using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicShop.Models
{
    public class OrderProductDTO
    {
        public string? ProductId { get; set; }

        public required int Quantity { get; set; }
    }
}
