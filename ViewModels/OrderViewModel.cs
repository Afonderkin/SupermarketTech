using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SupermarketTech.ViewModels
{
    internal class OrderViewModel : BaseViewModel
    {
        private readonly CartService _cart;
        private readonly User _currentUser;

        public ObservableCollection<OrderItem> Items { get; }

        public decimal Total => _cart.Total;
        public DateTime OrderDate { get; } = DateTime.Now;
        public DateTime DeliveryDate { get; } = DateTime.Now.Date.AddDays(1);

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public OrderViewModel(CartService cart, User currentUser)
        {
            _cart = cart;
            _currentUser = currentUser;

            Items = _cart.Items;

            ConfirmCommand = new RelayCommand(o => ExecuteConfirm(o));
            CancelCommand = new RelayCommand(o =>
            {
                var win = o as Window;
                win?.Close();
            });
        }


        private void ExecuteConfirm(object parameter)
        {
            var order = new Order
            {
                UserId = _currentUser.Id,
                Items = new System.Collections.Generic.List<OrderItem>(Items),
                Total = Total,
                OrderDate = OrderDate,
                DeliveryDate = DeliveryDate
            };

            var orderService = new OrderService();
            orderService.PlaceOrder(order);

            _cart.Clear();
            MessageBox.Show("Заказ подтверждён и сохранён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            var win = parameter as Window;
            win?.Close();
        }
    }
}
