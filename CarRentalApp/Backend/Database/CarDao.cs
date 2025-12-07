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

        // -------------------- GET ALL --------------------
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

        // -------------------- GET BY ID --------------------
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

        // -------------------- ADD CAR --------------------
        public bool AddCar(Car car)
        {
            string query = @"
                INSERT INTO cars (brand, model, year, color, price_per_day, status, category_name)
                VALUES (@brand, @model, @year, @color, @price, @status, @category)";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@brand", car.Brand);
                cmd.Parameters.AddWithValue("@model", car.Model);
                cmd.Parameters.AddWithValue("@year", car.Year);
                cmd.Parameters.AddWithValue("@color", car.Color);
                cmd.Parameters.AddWithValue("@price", car.PricePerDay);
                cmd.Parameters.AddWithValue("@status", car.Status);
                cmd.Parameters.AddWithValue("@category", car.CategoryName);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // -------------------- UPDATE CAR --------------------
        public bool UpdateCar(Car car)
        {
            string query = @"
                UPDATE cars SET
                    brand = @brand,
                    model = @model,
                    year = @year,
                    color = @color,
                    price_per_day = @price,
                    status = @status,
                    category_name = @category
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
                cmd.Parameters.AddWithValue("@status", car.Status);
                cmd.Parameters.AddWithValue("@category", car.CategoryName);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // -------------------- DELETE CAR --------------------
        public bool DeleteCar(int id)
        {
            string query = "DELETE FROM cars WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // -------------------- UPDATE CAR STATUS --------------------
        public bool UpdateCarStatus(int carId, string status)
        {
            string query = @"
                UPDATE cars SET
                    status = @status,
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


        // ============================================================
        //                      FILTER METHODS
        // ============================================================

        public List<Car> GetCarsByModel(string model)
        {
            return Search("model", model);
        }

        public List<Car> GetCarsByBrand(string brand)
        {
            return Search("brand", brand);
        }

        public List<Car> GetCarsByStatus(string status)
        {
            return Search("status", status);
        }

        public List<Car> GetCarsByCategory(string category)
        {
            return Search("category_name", category);
        }

        public List<Car> GetCarsByColor(string color)
        {
            return Search("color", color);
        }

        // Generic reusable search
        private List<Car> Search(string column, string value)
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
    }

}
