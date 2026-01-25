using SupermarketTech.ViewModels;
using System.Windows;

namespace SupermarketTech.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CartTab.Content = new CartView()
            {
                DataContext = new CartViewModel(App.CartService)
            };
        }
    }
}