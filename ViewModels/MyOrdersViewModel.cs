using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace SupermarketTech.ViewModels
{
    public class MyOrdersViewModel : BaseViewModel
    {
        private readonly User _currentUser;

        public ObservableCollection<OrderHistoryItemViewModel> Orders { get; }
            = new ObservableCollection<OrderHistoryItemViewModel>();

        public MyOrdersViewModel(User currentUser)
        {
            _currentUser = currentUser;
            LoadOrders();

            App.OrderService.OrderPlaced += OnOrderPlaced;
            App.OrderService.OrderDeleted += OnOrderDeleted;
        }

        private void LoadOrders()
        {
            Orders.Clear();
            var orders = App.OrderService.GetOrdersByUser(_currentUser.Id);

            foreach (var order in orders)
            {
                Orders.Add(new OrderHistoryItemViewModel(order));
            }
        }

        private void OnOrderPlaced()
        {
            LoadOrders();
        }

        private void OnOrderDeleted(int orderId)
        {
            var toRemove = Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (toRemove != null)
                Orders.Remove(toRemove);
        }
    }
}
