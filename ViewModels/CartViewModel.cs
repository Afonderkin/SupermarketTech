using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;

namespace SupermarketTech.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private readonly CartService _cart;
        private readonly User _currentUser;
        public ObservableCollection<OrderItem> Items => _cart.Items;

        public decimal Total => Items.Sum(i => i.Total);

        public ICommand RemoveCommand { get; }
        public ICommand CheckoutCommand { get; }

        public CartViewModel(User currentUser, CartService cart = null)
        {
            _currentUser = currentUser;
            _cart = cart ?? App.CartService;

            foreach (var item in _cart.Items)
                item.PropertyChanged += Item_PropertyChanged;

            Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (OrderItem item in e.NewItems)
                        item.PropertyChanged += Item_PropertyChanged;
                }

                OnPropertyChanged(nameof(Total));
            };

            RemoveCommand = new RelayCommand(o =>
            {
                if (o is OrderItem item)
                    _cart.Remove(item);
                OnPropertyChanged(nameof(Total));
            });

            CheckoutCommand = new RelayCommand(o =>
            {
                var wnd = new Views.OrderView();
                wnd.DataContext = new OrderViewModel(_cart, _currentUser);
                wnd.ShowDialog();
                OnPropertyChanged(nameof(Total));
            });
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItem.Quantity) || e.PropertyName == nameof(OrderItem.Total))
            {
                OnPropertyChanged(nameof(Total));
            }
        }
    }
}
