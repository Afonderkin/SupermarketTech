using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SupermarketTech.Models;

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
    }
}
