using CarRentalApp.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Helper
{
    public class HelperPayment
    {
        public static Payment MapPayment(MySqlDataReader reader)
        {
            return new Payment
            {
                Id = reader.GetInt32("id"),
                ReservationId = reader.GetInt32("reservation_id"),
                Amount = reader.GetDecimal("amount"),
                PaymentDate = reader.GetDateTime("created_at"),
                PaymentMethod = reader.GetString("method"),
                Status = reader.GetString("status"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}


