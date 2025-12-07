using CarRentalApp.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MySql.Data.MySqlClient;
//using CarRentalApp.Backend.Models;

namespace backendclienttesting.Backend.Helper
{

    public class HelperCar
    {
        public static Car MapCar(MySqlDataReader reader)
        {
            return new Car
            {
                Id = reader.GetInt32("id"),
                Brand = reader.GetString("brand"),
                Model = reader.GetString("model"),
                Year = reader.GetInt32("year"),
                Color =reader.IsDBNull(reader.GetOrdinal("color"))
                    ? null
                    : reader.GetString("color"),
                PricePerDay = reader.GetDecimal("price_per_day"),
                Status = reader.GetString("status"),
                CategoryName = reader.GetString("category_name")
                //CategoryId = reader.GetInt32("category_id"),
                //CategoryName = reader["name"] != DBNull.Value ? reader.GetString("name") : null
            }; 
        }
    }
}
