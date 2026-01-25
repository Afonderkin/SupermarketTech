using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SupermarketTech.Models;

namespace SupermarketTech.DataAccess
{
    internal class UserRepository
    {
        public User GetByLogin(string login)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Login, Password, Role FROM Users WHERE Login = @login LIMIT 1";
                    cmd.Parameters.AddWithValue("@login", login);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            return new User
                            {
                                Id = rdr.GetInt32("Id"),
                                Login = rdr.GetString("Login"),
                                Password = rdr.GetString("Password"),
                                Role = rdr.GetString("Role")
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
