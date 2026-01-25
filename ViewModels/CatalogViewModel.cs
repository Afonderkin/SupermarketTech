using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;
using SupermarketTech.DataAccess;
using System.Collections.Generic;

namespace SupermarketTech.ViewModels
{
    internal class CatalogViewModel : BaseViewModel
    {
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly CartService _cartService = new CartService();
        public CartService Cart => _cartService;

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();

        private string _search;
        public string Search
        {
            get { return _search; }
            set { Set(ref _search, value); ApplyFilters(); }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { Set(ref _selectedCategory, value); ApplyFilters(); }
        }

        private decimal _maxPrice = 100000;
        public decimal MaxPrice
        {
            get { return _maxPrice; }
            set { Set(ref _maxPrice, value); ApplyFilters(); }
        }

        private List<Product> _all = new List<Product>();

        private readonly Models.User _user;
        public Models.User CurrentUser => _user;

        public bool IsAdmin => _user?.Role == "Admin";

        public ICommand AddToCartCommand { get; }

        public CatalogViewModel(Models.User user)
        {
            _user = user;
            AddToCartCommand = new RelayCommand(o =>
            {
                var p = o as Product;
                if (p != null) _cartService.Add(p, 1);
            });

            LoadProducts();
        }

        public void LoadProducts()
        {
            _all = _productRepo.GetAll();
            Products.Clear();
            foreach (var p in _all) Products.Add(p);

            Categories.Clear();
            Categories.Add("Все");
            foreach (var c in _all.Select(x => x.Category).Distinct())
            {
                Categories.Add(c);
            }
            SelectedCategory = "Все";
        }

        private void ApplyFilters()
        {
            var filtered = _all.Where(p =>
                (SelectedCategory == "Все" || string.IsNullOrEmpty(SelectedCategory) || p.Category == SelectedCategory) &&
                (string.IsNullOrEmpty(Search) || p.Name.ToLower().Contains(Search.ToLower())) &&
                (p.Price <= MaxPrice)
            ).ToList();

            Products.Clear();
            foreach (var p in filtered) Products.Add(p);
        }
    }
}
