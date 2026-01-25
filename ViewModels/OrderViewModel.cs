using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;

namespace SupermarketTech.ViewModels
{
    internal class OrderViewModel : BaseViewModel
    {
        private readonly CartService _cart;
        private readonly OrderService _orderService = new OrderService();

        public Order Order { get; }

        public decimal Total => _cart.Total;
        public DateTime OrderDate { get; } = DateTime.Now;
        public DateTime DeliveryDate { get; } = DateTime.Now.Date.AddDays(1);

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public OrderViewModel(CartService cart)
        {
            _cart = cart;
            Order = new Order
            {
                Items = _cart.Items.Select(i => new OrderItem { ProductId = i.ProductId, Product = i.Product, Quantity = i.Quantity }).ToList(),
                Total = _cart.Total,
                OrderDate = OrderDate,
                DeliveryDate = DeliveryDate
            };

            ConfirmCommand = new RelayCommand(o => ExecuteConfirm(o));
            CancelCommand = new RelayCommand(o =>
            {
                // Закрыть окно (View)
                var win = o as Window;
                win?.Close();
            });
        }

        private void ExecuteConfirm(object parameter)
        {
            // Для примера можно задать UserId=1
            Order.UserId = 1;
            _orderService.PlaceOrder(Order);
            _cart.Clear();

            MessageBox.Show("Заказ подтверждён и сохранён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var win = parameter as Window;
            win?.Close();
        }
    }
}
