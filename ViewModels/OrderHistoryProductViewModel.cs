using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SupermarketTech.ViewModels
{
    public class OrderHistoryProductViewModel : INotifyPropertyChanged
    {
        public OrderItem OrderItem { get; }

        public Product Product => OrderItem.Product;

        public int Quantity => OrderItem.Quantity;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        public string ShortDescription =>
            string.IsNullOrWhiteSpace(Product.Description)
                ? ""
                : Product.Description.Length > 80
                    ? Product.Description.Substring(0, 80) + "..."
                    : Product.Description;

        public ICommand ToggleDescriptionCommand =>
            new RelayCommand(_ => IsExpanded = !IsExpanded);

        public OrderHistoryProductViewModel(OrderItem item)
        {
            OrderItem = item;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
