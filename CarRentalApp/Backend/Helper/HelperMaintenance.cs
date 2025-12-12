using backendclienttesting.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace backendclienttesting.Backend.Helper
{
    public static class HelperMaintenance
    {
        public static Maintenance MapMaintenance(MySqlDataReader r)
        {
            return new Maintenance
            {
                Id = r.GetInt32("id"),
                CarId = r.GetInt32("car_id"),
                StartDate = r.GetDateTime("start_date"),
                EndDate = r.GetDateTime("end_date"),
                Description = r.GetString("description"),
                Status = r.GetString("status"),
                CreatedAt = r.GetDateTime("created_at"),
                UpdatedAt = r.GetDateTime("updated_at")
            };
        }
    }

}
