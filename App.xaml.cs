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
    }
}
