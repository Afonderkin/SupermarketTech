using Microsoft.Win32;
using SupermarketTech.DataAccess;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SupermarketTech.ViewModels
{
    internal class EditProductViewModel : BaseViewModel
    {
        private readonly ProductRepository _repo = App.ProductRepo;

        public Product Product { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectImageCommand { get; }

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
                ImageFileName = product.ImageFileName
            };
            SelectImageCommand = new RelayCommand(o => SelectImage());
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
        private void SelectImage()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            if (dlg.ShowDialog() == true)
            {
                string sourcePath = dlg.FileName;
                string fileName = Path.GetFileName(sourcePath);

                string targetDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Assets", "Images");
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                string targetPath = Path.Combine(targetDir, fileName);

                if (!File.Exists(targetPath))
                    File.Copy(sourcePath, targetPath);

                Product.ImageFileName = fileName;
                OnPropertyChanged(nameof(Product));
            }
        }
    }
}
