using System;
using System.Windows;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.DataAccess;

namespace SupermarketTech.ViewModels
{
    internal class AddProductViewModel : BaseViewModel
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public Product Product { get; } = new Product();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddProductViewModel()
        {
            SaveCommand = new RelayCommand(o =>
            {
                _repo.Add(Product);
                var win = o as Window;
                win?.Close();
            });

            CancelCommand = new RelayCommand(o =>
            {
                var win = o as Window;
                win?.Close();
            });
        }
    }
}
