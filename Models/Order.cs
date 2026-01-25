using System;
using System.Collections.Generic;

namespace SupermarketTech.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}