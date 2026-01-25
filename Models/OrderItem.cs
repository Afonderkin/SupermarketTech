using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SupermarketTech.Models
{
    public class OrderItem : INotifyPropertyChanged
    {
        private int _quantity;
        private Product _product;

        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Total));
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }

        public decimal Price => Product?.Price ?? 0m;

        public decimal Total => Price * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
