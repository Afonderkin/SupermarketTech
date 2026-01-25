using SupermarketTech.Infrastructure;
using SupermarketTech.Services;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SupermarketTech.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly AuthService _authService = new AuthService();

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        private string _login;

        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }
        private string _error;

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(o => Register(o));
        }

        private void Register(object parameter)
        {
            var pwBox = parameter as System.Windows.Controls.PasswordBox;
            string password = pwBox?.Password ?? string.Empty;

            string login = Login;

            if (_authService.Register(login, password, out string error))
            {
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                var win = System.Windows.Application.Current.Windows
                            .OfType<Window>()
                            .FirstOrDefault(w => w.DataContext == this);
                win?.Close();
            }
            else
            {
                Error = error;
            }
        }
    }
}
