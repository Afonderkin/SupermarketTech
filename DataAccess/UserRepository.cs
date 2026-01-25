using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SupermarketTech.Models;

namespace SupermarketTech.DataAccess
{
    public class UserRepository
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

        public void UpdateUser(User user)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Users SET Login = @login, Password = @password WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddUser(User user)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Users (Login, Password, Role) VALUES (@login, @password, @role)";
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@role", user.Role ?? "user");
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
