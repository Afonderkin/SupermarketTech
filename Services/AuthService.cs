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
        private readonly UserRepository _userRepo = new UserRepository();

        public User Authenticate(string login, string password)
        {
            var u = _userRepo.GetByLogin(login);
            if (u == null) return null;
            if (u.Password == password) return u;
            return null;
        }
    }
}
