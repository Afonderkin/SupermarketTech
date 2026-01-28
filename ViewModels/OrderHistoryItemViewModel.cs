using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

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
                if (App.Current.MainWindow != null &&
                    System.Windows.MessageBox.Show("Вы точно хотите отменить заказ?", "Подтверждение", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning)
                    == System.Windows.MessageBoxResult.Yes)
                {
                    App.OrderService.DeleteOrder(OrderId);
                }
            });
        }
    }
}
