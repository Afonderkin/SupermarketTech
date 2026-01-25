using SupermarketTech.DataAccess;
using SupermarketTech.Infrastructure;
using SupermarketTech.Models;
using SupermarketTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SupermarketTech.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        private readonly User _user;
        private readonly UserRepository _userRepo;
        public string Login
        {
            get => _user.Login;
            set { _user.Login = value; OnPropertyChanged(); }
        }

        public string Password { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand LogoutCommand { get; }

        public UserSettingsViewModel(User user)
        {
            _user = user;
            _userRepo = App.UserRepo;

            SaveCommand = new RelayCommand(o => Save(o));
            LogoutCommand = new RelayCommand(o => Logout(o));
        }

        private void Save(object parameter)
        {
            var win = parameter as Window;

            if (win != null)
            {
                var pwBox = win.FindName("PasswordBox") as PasswordBox;
                string newPassword = pwBox?.Password;

                if (string.IsNullOrWhiteSpace(Login))
                {
                    MessageBox.Show("Логин не может быть пустым!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    _user.Password = newPassword;
                }

                try
                {
                    _userRepo.UpdateUser(_user);

                    MessageBox.Show("Данные успешно сохранены!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    win.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Logout(object parameter)
        {
            var win = parameter as Window;
            win?.Close();

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.Close();
            }

            var windowsToClose = Application.Current.Windows.OfType<Window>().ToList();
            foreach (var window in windowsToClose)
            {
                if (window != win && window != mainWindow)
                    window.Close();
            }

            var loginWindow = new LoginView();
            loginWindow.Show();
        }
    }
}
