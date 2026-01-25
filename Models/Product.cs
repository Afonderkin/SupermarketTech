using System;

namespace SupermarketTech.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public string ImagePath
        {
            get
            {
                var file = string.IsNullOrEmpty(ImageFileName) ? "placeholder.png" : ImageFileName;
                return $"pack://application:,,,/SupermarketTech;component/Resources/Assets/Images/{file}";
            }
        }
    }
}