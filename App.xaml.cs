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
        public static OrderService OrderService { get; } = new OrderService();
        public static ProductRepository ProductRepo { get; } = new ProductRepository();
        public static UserRepository UserRepo { get; } = new UserRepository();

        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Произошла непойманная ошибка:\n\n{e.Exception.Message}\n\n{e.Exception.StackTrace}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(
                $"Непойманное исключение в фоновом потоке:\n\n{ex?.Message}\n\n{ex?.StackTrace}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
