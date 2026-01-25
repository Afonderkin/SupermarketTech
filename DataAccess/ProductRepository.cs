using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SupermarketTech.Models;

namespace SupermarketTech.DataAccess
{
    public class ProductRepository
    {
        public event Action RepositoryChanged;
        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Category, Price, Stock, Description, ImageFileName FROM Products";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new Product
                            {
                                Id = rdr.GetInt32("Id"),
                                Name = rdr.GetString("Name"),
                                Category = rdr.GetString("Category"),
                                Price = rdr.GetDecimal("Price"),
                                Stock = rdr.GetInt32("Stock"),
                                Description = rdr.IsDBNull(rdr.GetOrdinal("Description")) ? "" : rdr.GetString("Description"),
                                ImageFileName = rdr.IsDBNull(rdr.GetOrdinal("ImageFileName")) ? null : rdr.GetString("ImageFileName")
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void Add(Product p)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Products (Name, Category, Price, Stock, Description, ImageFileName) VALUES (@n,@c,@pr,@s,@d,@ip)";
                    cmd.Parameters.AddWithValue("@n", p.Name);
                    cmd.Parameters.AddWithValue("@c", p.Category);
                    cmd.Parameters.AddWithValue("@pr", p.Price);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@d", p.Description);
                    cmd.Parameters.AddWithValue("@ip", p.ImageFileName);
                    cmd.ExecuteNonQuery();
                }
            }
            RepositoryChanged?.Invoke();
        }

        public void Update(Product p)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Products SET Name=@n,Category=@c,Price=@pr,Stock=@s,Description=@d,ImageFileName=@ip WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@n", p.Name);
                    cmd.Parameters.AddWithValue("@c", p.Category);
                    cmd.Parameters.AddWithValue("@pr", p.Price);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@d", p.Description);
                    cmd.Parameters.AddWithValue("@ip", p.ImageFileName);
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            RepositoryChanged?.Invoke();
        }

        public void Delete(int id)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Products WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            RepositoryChanged?.Invoke();
        }
    }
}
