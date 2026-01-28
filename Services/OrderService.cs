using SupermarketTech.DataAccess;
using SupermarketTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SupermarketTech.Services
{
    public class OrderService
    {
        private readonly OrderRepository _repo = new OrderRepository();

        public event Action OrderPlaced;
        public event Action<int> OrderDeleted;

        public void PlaceOrder(Order order)
        {
            order.Total = order.Items.Sum(i => i.Total);
            order.OrderDate = DateTime.Now;
            order.DeliveryDate = DateTime.Now.Date.AddDays(1);
            _repo.CreateOrder(order);

            OrderPlaced?.Invoke();
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            return _repo.GetOrdersByUser(userId);
        }

        public void DeleteOrder(int orderId)
        {
            _repo.DeleteOrder(orderId);
            OrderDeleted?.Invoke(orderId);
        }
    }
}
