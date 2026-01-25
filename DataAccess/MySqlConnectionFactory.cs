using System.Configuration;
using MySql.Data.MySqlClient;

namespace SupermarketTech.DataAccess
{
    internal static class MySqlConnectionFactory
    {
        internal static MySqlConnection CreateConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            var conn = new MySqlConnection(cs);
            return conn;
        }
    }
}