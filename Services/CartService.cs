using System.Collections.ObjectModel;
using System.Linq;
using SupermarketTech.Models;

namespace SupermarketTech.Services
{
    public class CartService
    {
        public ObservableCollection<OrderItem> Items { get; } = new ObservableCollection<OrderItem>();

        public void Add(Product product, int quantity = 1)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                Items.Add(new OrderItem { Product = product, ProductId = product.Id, Quantity = quantity });
            }
        }

        public void Remove(OrderItem item)
        {
            if (Items.Contains(item)) Items.Remove(item);
        }

        public decimal Total => Items.Sum(i => i.Total);

        public void Clear() => Items.Clear();
    }
}
