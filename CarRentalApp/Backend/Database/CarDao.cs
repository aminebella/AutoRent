using backendclienttesting.Backend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using CarRentalApp.Backend.Helper;
using CarRentalApp.Backend.Models;


namespace CarRentalApp.Backend.Database
{
    public class CarDao
    {
        public CarDao() { }

        // Index: Get All Cars
        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            string query = "SELECT * FROM cars";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    cars.Add(HelperCar.MapCar(reader));
            }
            return cars;
        }

        // Show: Get Car By Id
        public Car GetCarById(int id)
        {
            Car car = null;
            string query = "SELECT * FROM cars WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    car = HelperCar.MapCar(reader);
            }
            return car;
        }

        // Add: Add Car
        public bool AddCar(Car car)
        {
            string query = @"
                INSERT INTO cars 
                    (brand, model, year, color, price_per_day, status, category_name, last_maintenance_date, maintenance_interval_days)
                VALUES 
                    (@brand, @model, @year, @color, @price, @status, @category, @last_maintenance_date, @maintenance_interval_days)";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                // Set defaults if the front-end didn't provide a value
                string defaultColor = "No Color given";
                string defaultCategory = "ECONOMY";
                cmd.Parameters.AddWithValue("@brand", car.Brand);
                cmd.Parameters.AddWithValue("@model", car.Model);
                cmd.Parameters.AddWithValue("@year", car.Year);
                cmd.Parameters.AddWithValue("@color", string.IsNullOrWhiteSpace(car.Color) ? defaultColor : car.Color);
                cmd.Parameters.AddWithValue("@price", car.PricePerDay);
                cmd.Parameters.AddWithValue("@status", "AVAILABLE");
                cmd.Parameters.AddWithValue("@category", string.IsNullOrWhiteSpace(car.CategoryName) ? defaultCategory : car.CategoryName);
                cmd.Parameters.AddWithValue("@last_maintenance_date", car.LastMaintenanceDate == default ? DateTime.Now : car.LastMaintenanceDate);
                cmd.Parameters.AddWithValue("@maintenance_interval_days", car.MaintenanceIntervalDays == 0 ? 180 : car.MaintenanceIntervalDays);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update: Update Car
        public bool UpdateCar(Car car)
        {
            string query = @"
                UPDATE cars SET
                    brand = @brand,
                    model = @model,
                    year = @year,
                    color = @color,
                    price_per_day = @price,
                    category_name = @category,
                    last_maintenance_date = @last_maintenance_date,
                    maintenance_interval_days = @maintenance_interval_days
                WHERE id = @id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", car.Id);
                cmd.Parameters.AddWithValue("@brand", car.Brand);
                cmd.Parameters.AddWithValue("@model", car.Model);
                cmd.Parameters.AddWithValue("@year", car.Year);
                cmd.Parameters.AddWithValue("@color", car.Color);
                cmd.Parameters.AddWithValue("@price", car.PricePerDay);
                cmd.Parameters.AddWithValue("@category", car.CategoryName);
                cmd.Parameters.AddWithValue("@last_maintenance_date", car.LastMaintenanceDate);
                cmd.Parameters.AddWithValue("@maintenance_interval_days", car.MaintenanceIntervalDays);
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // Delete: Delete Car
        //public bool DeleteCar(int id)
        //{
        //    string query = "DELETE FROM cars WHERE id = @id";

        //    using (MySqlConnection conn = DbConnection.GetConnection())
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@id", id);
        //        return cmd.ExecuteNonQuery() > 0;
        //    }
        //}


        // Update Status: UPDATE CAR STATUS 
        public bool UpdateCarStatus(int carId, string status)
        {
            string query = @"
                UPDATE cars SET
                    status = @status
                WHERE id = @id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", carId);
                cmd.Parameters.AddWithValue("@status", status);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update Status Available: Mark Car As Available
        public bool MarkAvailable(int id)
        {
            return UpdateCarStatus(id, "AVAILABLE");
        }

        // Update Status Unavailable: Mark Car As Unavailable
        public bool MarkUnavailable(int id)
        {
            return UpdateCarStatus(id, "UNAVAILABLE");
        }

        // Update Status Maintenance: Mark Car As Maintenance
        public bool MarkMaintenance(int id)
        {
            return UpdateCarStatus(id, "MAINTENANCE");
        }

        // Update Status Reserved: Mark Car As Reserved
        public bool MarkRESERVED(int id)
        {
            return UpdateCarStatus(id, "RESERVED");
        }

        // Update Last date of Maintenance: UPDATE Last Maintenance
        public bool UpdateLastMaintenance(int carId)
        {
            string q = @"UPDATE cars 
                 SET last_maintenance_date = CURRENT_DATE()
                 WHERE id=@id";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@id", carId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }



        // Filters
        // Search Car By ...
        private List<Car> SearchCarBy(string column, string value)
        {
            List<Car> cars = new List<Car>();
            string query = $"SELECT * FROM cars WHERE {column} LIKE @value";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@value", "%" + value + "%");

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cars.Add(HelperCar.MapCar(reader));
            }

            return cars;
        }
        // By Model
        public List<Car> GetCarsByModel(string model)
        {
            return SearchCarBy("model", model);
        }
        // By Brand
        public List<Car> GetCarsByBrand(string brand)
        {
            return SearchCarBy("brand", brand);
        }
        // By Status
        public List<Car> GetCarsByStatus(string status)
        {
            return SearchCarBy("status", status);
        }
        // By Category
        public List<Car> GetCarsByCategory(string category)
        {
            return SearchCarBy("category_name", category);
        }
        // By Color
        public List<Car> GetCarsByColor(string color)
        {
            return SearchCarBy("color", color);
        }
        // Search Car by Year
        public List<Car> SearchByYear(int value)
        {
            List<Car> cars = new List<Car>();
            string query = "SELECT * FROM cars WHERE year >= @value";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@value", value);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cars.Add(HelperCar.MapCar(reader));
            }

            return cars;
        }

        // See if Car Available Now
        public bool IfAvailable(int idC)
        {
            string query = "SELECT status FROM cars WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idC);

                var result = cmd.ExecuteScalar();

                if (result == null)
                    return false;  // car does not exist

                string status = result.ToString();

                return status.Equals("AVAILABLE", StringComparison.OrdinalIgnoreCase);
            }
        }

        // See if Car UnAvailable Now
        public bool IfUnAvailable(int idC)
        {
            string query = "SELECT status FROM cars WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idC);

                var result = cmd.ExecuteScalar();

                if (result == null)
                    return false;  // car does not exist

                string status = result.ToString();

                return status.Equals("UNAVAILABLE", StringComparison.OrdinalIgnoreCase);
            }
        }

        // See Car Status
        public string CarStatus(int idC)
        {
            string query = "SELECT status FROM cars WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idC);

                var result = cmd.ExecuteScalar();

                if (result == null)
                    return null;  // car does not exist

                string status = result.ToString();

                return status;
            }
        }


    }

}
