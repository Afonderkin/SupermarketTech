using System;
using System.Linq;
using SupermarketTech.DataAccess;
using SupermarketTech.Models;

namespace SupermarketTech.Services
{
    internal class OrderService
    {
        private readonly OrderRepository _repo = new OrderRepository();

        public void PlaceOrder(Order order)
        {
            order.Total = order.Items.Sum(i => i.Total);
            order.OrderDate = DateTime.Now;
            order.DeliveryDate = DateTime.Now.Date.AddDays(1);
            _repo.CreateOrder(order);
        }
    }
}
