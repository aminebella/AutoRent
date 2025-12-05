using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Imports
using CarRentalApp.Backend.Models;
using MySql.Data.MySqlClient;

namespace backendclienttesting.Backend.Helper
{
    public class HelperRequest
    {
        public static Request MapRequest(MySqlDataReader reader)
        {
            return new Request
            {
                Id = reader.GetInt32("id"),
                ClientId = reader.GetInt32("client_id"),
                CarId = reader.GetInt32("car_id"),
                StartDate = reader.GetDateTime("start_date"),
                EndDate = reader.GetDateTime("end_date"),
                Status = reader.GetString("status"),
                Message = reader["message"] == DBNull.Value ? null : reader.GetString("message"),
                CreatedAt = reader["created_at"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime("created_at"),
                UpdatedAt = reader["updated_at"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime("updated_at")
            };
        }
    }
}
