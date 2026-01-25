using System;
using System.Windows;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.DataAccess;

namespace SupermarketTech.ViewModels
{
    internal class EditProductViewModel : BaseViewModel
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public Product Product { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditProductViewModel(Product product)
        {
            Product = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                ImagePath = product.ImagePath
            };

            SaveCommand = new RelayCommand(o =>
            {
                _repo.Update(Product);
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
