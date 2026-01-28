using MySql.Data.MySqlClient;
using SupermarketTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SupermarketTech.DataAccess
{
    internal class OrderRepository
    {
        public int CreateOrder(Order order)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "INSERT INTO Orders (UserId, Total, OrderDate, DeliveryDate) VALUES (@uid,@total,@od,@dd)";
                            cmd.Parameters.AddWithValue("@uid", order.UserId);
                            cmd.Parameters.AddWithValue("@total", order.Total);
                            cmd.Parameters.AddWithValue("@od", order.OrderDate);
                            cmd.Parameters.AddWithValue("@dd", order.DeliveryDate);
                            cmd.ExecuteNonQuery();
                            var orderId = (int)cmd.LastInsertedId;

                            foreach (var item in order.Items)
                            {
                                using (var cmdItem = conn.CreateCommand())
                                {
                                    cmdItem.Transaction = tran;
                                    cmdItem.CommandText = "INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES (@oid,@pid,@q,@p)";
                                    cmdItem.Parameters.AddWithValue("@oid", orderId);
                                    cmdItem.Parameters.AddWithValue("@pid", item.ProductId);
                                    cmdItem.Parameters.AddWithValue("@q", item.Quantity);
                                    cmdItem.Parameters.AddWithValue("@p", item.Price);
                                    cmdItem.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();
                        return 1;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            var orders = new Dictionary<int, Order>();

            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        o.Id AS OrderId,
                        o.OrderDate,
                        o.DeliveryDate,
                        oi.Quantity,
                        oi.Price,
                        p.*
                    FROM Orders o
                    JOIN OrderItems oi ON oi.OrderId = o.Id
                    JOIN Products p ON p.Id = oi.ProductId
                    WHERE o.UserId = @uid
                    ORDER BY o.OrderDate DESC";

                cmd.Parameters.AddWithValue("@uid", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var orderId = reader.GetInt32("OrderId");

                        if (!orders.TryGetValue(orderId, out var order))
                        {
                            order = new Order
                            {
                                Id = orderId,
                                OrderDate = reader.GetDateTime("OrderDate"),
                                DeliveryDate = reader.GetDateTime("DeliveryDate")
                            };
                            orders.Add(orderId, order);
                        }

                        order.Items.Add(new OrderItem
                        {
                            Quantity = reader.GetInt32("Quantity"),
                            Product = new Product
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Description = reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                ImageFileName = reader.GetString("ImageFileName")
                            }
                        });
                    }
                }
            }

            return orders.Values.ToList();
        }

        public void DeleteOrder(int orderId)
        {
            using (var conn = MySqlConnectionFactory.CreateConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "DELETE FROM OrderItems WHERE OrderId = @oid";
                            cmd.Parameters.AddWithValue("@oid", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "DELETE FROM Orders WHERE Id = @oid";
                            cmd.Parameters.AddWithValue("@oid", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
