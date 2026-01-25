using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupermarketTech.DataAccess;
using SupermarketTech.Models;

namespace SupermarketTech.Services
{
    internal class AuthService
    {
        private readonly UserRepository _userRepo = App.UserRepo;

        public User Authenticate(string login, string password)
        {
            var u = _userRepo.GetByLogin(login);
            if (u == null) return null;
            if (u.Password == password) return u;
            return null;
        }

        public bool Register(string login, string password, out string error, string role = "user")
        {
            error = null;

            if (string.IsNullOrWhiteSpace(login))
            {
                error = "Логин не может быть пустым";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                error = "Пароль не может быть пустым";
                return false;
            }

            var existing = _userRepo.GetByLogin(login);
            if (existing != null)
            {
                error = "Пользователь с таким логином уже существует";
                return false;
            }

            try
            {
                var user = new User { Login = login, Password = password, Role = role };
                _userRepo.AddUser(user);
                return true;
            }
            catch (Exception ex)
            {
                error = $"Ошибка при регистрации: {ex.Message}";
                return false;
            }
        }
    }
}
