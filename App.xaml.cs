using SupermarketTech.DataAccess;
using SupermarketTech.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SupermarketTech
{
    public partial class App : Application
    {
        public static CartService CartService { get; } = new CartService();
        public static ProductRepository ProductRepo { get; } = new ProductRepository();
        public static UserRepository UserRepo { get; } = new UserRepository();
    }
}
