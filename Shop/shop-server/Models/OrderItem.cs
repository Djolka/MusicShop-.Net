//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace MusicShop.Models
//{
//    public class OrderItem
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public required string OrderId { get; set; }
//        public Order Order { get; set; }

//        [Required]
//        public required string ProductId { get; set; }
//        public Product Product { get; set; }

//        [Required]
//        public int Quantity { get; set; }

//        // Optional: Save the price at the time of purchase
//        [Required]
//        public double PriceAtOrderTime { get; set; }
//    }
//}