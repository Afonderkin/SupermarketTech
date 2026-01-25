using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SupermarketTech.Models;

namespace SupermarketTech.DataAccess
{
    internal class ProductRepository
    {
        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Category, Price, Stock, Description, ImagePath FROM Products";
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
                                ImagePath = rdr.IsDBNull(rdr.GetOrdinal("ImagePath")) ? null : rdr.GetString("ImagePath")
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
                    cmd.CommandText = "INSERT INTO Products (Name, Category, Price, Stock, Description, ImagePath) VALUES (@n,@c,@pr,@s,@d,@ip)";
                    cmd.Parameters.AddWithValue("@n", p.Name);
                    cmd.Parameters.AddWithValue("@c", p.Category);
                    cmd.Parameters.AddWithValue("@pr", p.Price);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@d", p.Description);
                    cmd.Parameters.AddWithValue("@ip", p.ImagePath);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Product p)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Products SET Name=@n,Category=@c,Price=@pr,Stock=@s,Description=@d,ImagePath=@ip WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@n", p.Name);
                    cmd.Parameters.AddWithValue("@c", p.Category);
                    cmd.Parameters.AddWithValue("@pr", p.Price);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@d", p.Description);
                    cmd.Parameters.AddWithValue("@ip", p.ImagePath);
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
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
        }
    }
}
