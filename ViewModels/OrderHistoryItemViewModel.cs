using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace SupermarketTech.ViewModels
{
    public class OrderHistoryItemViewModel
    {
        public int OrderId { get; }
        public DateTime DeliveryDate { get; }
        public ObservableCollection<OrderHistoryProductViewModel> Items { get; }
        public ICommand CancelOrderCommand { get; }
        public decimal TotalPrice => Items.Sum(i => i.Product.Price * i.Quantity);

        public OrderHistoryItemViewModel(Order order)
        {
            OrderId = order.Id;
            DeliveryDate = order.DeliveryDate;

            Items = new ObservableCollection<OrderHistoryProductViewModel>(
                order.Items.Select(i => new OrderHistoryProductViewModel(i))
            );

            CancelOrderCommand = new RelayCommand(o =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (MessageBox.Show("Вы точно хотите отменить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                        == MessageBoxResult.Yes)
                    {
                        App.OrderService.DeleteOrder(OrderId);
                    }
                });
            });
        }
    }
}
