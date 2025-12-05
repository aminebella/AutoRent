using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CarRentalApp.Backend.Database
{
    public class DbConnection
    {
        private static string connectionstring = "server=localhost;database=manage_carsdb;uid=root;pwd=;";
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionstring);
        }
    }
}
