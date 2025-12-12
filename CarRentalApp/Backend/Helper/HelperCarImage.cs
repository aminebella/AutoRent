using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Biblio
using MySql.Data.MySqlClient;

// Imports
using backendclienttesting.Backend.Models;


namespace backendclienttesting.Backend.Helper
{
    public class HelperCarImage
    {
        public static CarImage MapCarImage(MySqlDataReader r)
        {
            return new CarImage
            {
                Id = r.GetInt32("id"),
                CarId = r.GetInt32("car_id"),
                ImagePath = r.GetString("image_path"),
                CreatedAt = r.GetDateTime("created_at"),
                UpdatedAt = r.GetDateTime("updated_at"),
            };
        }
    }
}
