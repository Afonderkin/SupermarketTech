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
    public class CatalogViewModel : BaseViewModel
    {
        private readonly ProductRepository _repo;
        public CartService Cart => App.CartService;

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();
        public CartViewModel CartVM { get; }

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
        private decimal _minPrice = 0;
        public decimal MinPrice
        {
            get { return _minPrice; }
            set { Set(ref _minPrice, value); ApplyFilters(); }
        }

        private List<Product> _all = new List<Product>();

        private readonly Models.User _user;
        public Models.User CurrentUser => _user;

        public bool IsAdmin => _user?.Role == "admin";

        public ICommand AddToCartCommand { get; }

        public CatalogViewModel(Models.User user)
        {
            _repo = App.ProductRepo;
            _user = user;

            App.ProductRepo.RepositoryChanged += LoadProducts;

            CartVM = new CartViewModel(_user, App.CartService);
            AddToCartCommand = new RelayCommand(o =>
            {
                var p = o as Product;
                if (p != null) Cart.Add(p, 1);
            });

            LoadProducts();
        }

        public void LoadProducts()
        {
            _all = _repo.GetAll();
            Products.Clear();
            foreach (var p in _all) 
            { 
                Products.Add(p);
                System.Diagnostics.Debug.WriteLine(p.ImagePath);
            } 

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
            string searchLower = string.IsNullOrEmpty(Search) ? "" : Search.ToLower();

            var filtered = _all.Where(p =>
                (SelectedCategory == "Все" || string.IsNullOrEmpty(SelectedCategory) || p.Category == SelectedCategory) &&
                (string.IsNullOrEmpty(Search) || p.Name.ToLower().Contains(searchLower)) &&
                (p.Price >= MinPrice && p.Price <= MaxPrice)
            ).ToList();

            Products.Clear();
            foreach (var p in filtered)
                Products.Add(p);
        }
    }
}
