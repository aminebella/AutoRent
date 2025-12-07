using CarRentalApp.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backendclienttesting.Backend.Helper
{
    public class HelperReservation
    {
        public static Reservation MapReservation(MySqlDataReader reader)
        {
            return new Reservation
            {
                Id = reader.GetInt32("id"),
                CarId = reader.GetInt32("car_id"),
                UserId = reader.GetInt32("user_id"),

                StartDate = reader.GetDateTime("start_date"),
                EndDate = reader.IsDBNull(reader.GetOrdinal("end_date"))
                    ? (DateTime?)null
                    : reader.GetDateTime("end_date"),

                TotalPrice = reader.GetDecimal("total_price"),

                Status = reader.GetString("status"),
                PaymentStatus = reader.GetString("payment_status"),

                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            };
        }
    }
}
