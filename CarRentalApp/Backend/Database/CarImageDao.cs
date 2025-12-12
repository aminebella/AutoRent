using backendclienttesting.Backend.Helper;
// Imports
using backendclienttesting.Backend.Models;
using CarRentalApp.Backend.Helper;
// Biblio
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Database
{
    public class CarImageDao
    {
        // Show: Get all images for a car
        public List<CarImage> GetImagesByCarId(int carId)
        {
            var list = new List<CarImage>();
            string q = "SELECT * FROM car_images WHERE car_id = @id";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", carId);

                var r = cmd.ExecuteReader();
                while (r.Read())
                    list.Add(HelperCarImage.MapCarImage(r));
            }
            return list;
        }

        // Get image by its ID (to retrieve the file path)
        public CarImage? GetImageById(int id)
        {
            string q = "SELECT * FROM car_images WHERE id = @id";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", id);

                var r = cmd.ExecuteReader();
                if (r.Read())
                    return HelperCarImage.MapCarImage(r);

                return null;
            }
        }

        // Add: Add image
        public bool AddImage(CarImage img)
        {
            string q = @"INSERT INTO car_images (car_id, image_path)
                         VALUES (@car_id, @path)";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@car_id", img.CarId);
                cmd.Parameters.AddWithValue("@path", img.ImagePath);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Delete Partial: Delete one image
        public bool DeleteOneImageById(int id)
        {
            string q = "DELETE FROM car_images WHERE id = @id";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Delete Total: Delete all images of a car
        public bool DeleteAllImagesByCarId(int carId)
        {
            string q = "DELETE FROM car_images WHERE car_id = @id";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", carId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
