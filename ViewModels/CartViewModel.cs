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
        public ObservableCollection<OrderItem> Items => _cart.Items;

        public decimal Total => _cart.Total;

        public ICommand RemoveCommand { get; }
        public ICommand CheckoutCommand { get; }

        public CartViewModel() : this(App.CartService) { }

        public CartViewModel(CartService cart)
        {
            _cart = cart;
            RemoveCommand = new RelayCommand(o =>
            {
                var item = o as OrderItem;
                if (item != null) _cart.Remove(item);
                OnPropertyChanged(nameof(Total));
            });

            CheckoutCommand = new RelayCommand(o =>
            {
                // открыть окно оформления заказа
                var wnd = new Views.OrderView();
                wnd.DataContext = new OrderViewModel(_cart);
                wnd.ShowDialog();
                OnPropertyChanged(nameof(Total));
            });
            Items.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Total));
        }
    }
}
