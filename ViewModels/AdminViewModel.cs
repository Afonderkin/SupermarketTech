using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SupermarketTech.DataAccess;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;

namespace SupermarketTech.ViewModels
{
    internal class AdminViewModel : BaseViewModel
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public AdminViewModel()
        {
            RefreshCommand = new RelayCommand(o => Load());
            AddCommand = new RelayCommand(o =>
            {
                var wnd = new Views.AddProductView();
                wnd.DataContext = new AddProductViewModel();
                wnd.ShowDialog();
                Load();
            });

            EditCommand = new RelayCommand(o =>
            {
                var p = o as Product;
                if (p == null) return;
                var wnd = new Views.EditProductView();
                wnd.DataContext = new EditProductViewModel(p);
                wnd.ShowDialog();
                Load();
            });

            DeleteCommand = new RelayCommand(o =>
            {
                var p = o as Product;
                if (p == null) return;
                if (MessageBox.Show($"Удалить '{p.Name}'?", "Подтвердите", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _repo.Delete(p.Id);
                    Load();
                }
            });

            Load();
        }

        public void Load()
        {
            Products.Clear();
            foreach (var p in _repo.GetAll()) Products.Add(p);
        }
    }
}
