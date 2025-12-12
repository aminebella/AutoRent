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
                Color = reader.GetString("color"),
                PricePerDay = reader.GetDecimal("price_per_day"),
                Status = reader.GetString("status"),
                CategoryName = reader.GetString("category_name"),
                LastMaintenanceDate = reader.GetDateTime("last_maintenance_date"),
                MaintenanceIntervalDays = reader.GetInt32("maintenance_interval_days")
            };
        }

    }

}
