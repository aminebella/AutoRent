using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;

using CarRentalApp.Backend.Models;



namespace CarRentalApp.Backend.Helper
{
    public class HelperClient
    {
        public static User MapUser(MySqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("first_name"), // or reader["first_name"].ToString(),
                LastName = reader.GetString("last_name"), // reader["last_name"].ToString(),
                Phone = reader.GetString("phone"), // reader["phone"].ToString(),
                Role = reader.GetString("role"), // reader["role"].ToString(),
                Email = reader.GetString("email"), // reader["email"].ToString(),
                Password = reader.GetString("password") // reader["password"].ToString()
            };
        }
    }
}
