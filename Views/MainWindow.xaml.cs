using SupermarketTech.Models;
using SupermarketTech.ViewModels;
using System.Windows;

namespace SupermarketTech.Views
{
    public partial class MainWindow : Window
    {
        public CatalogViewModel CatalogVM { get; }
        public MainWindow(User currentUser)
        {
            InitializeComponent();
            CatalogVM = new CatalogViewModel(currentUser);
            DataContext = CatalogVM;

            CartTab.Content = new CartView()
            {
                DataContext = new CartViewModel(currentUser, App.CartService)
            };

            var adminVM = new AdminViewModel();
            AdminViewControl.DataContext = adminVM;
        }
    }
}