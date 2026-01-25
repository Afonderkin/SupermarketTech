using System;
using System.Windows;
using System.Windows.Input;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Services;

namespace SupermarketTech.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService = new AuthService();

        private string _login;
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { Set(ref _error, value); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(o => ExecuteLogin(o));
        }

        private void ExecuteLogin(object parameter)
        {
            var passBox = parameter as System.Windows.Controls.PasswordBox;
            var password = passBox?.Password ?? string.Empty;

            var user = _authService.Authenticate(Login, password);
            if (user == null)
            {
                Error = "Неверный логин или пароль";
                return;
            }

            var mw = new Views.MainWindow(user);
            mw.DataContext = new CatalogViewModel(user);
            mw.Show();

            // закрыть окно логина
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.LoginView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
