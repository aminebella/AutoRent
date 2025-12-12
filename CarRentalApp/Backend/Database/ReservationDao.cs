using backendclienttesting.Backend.Helper;
using CarRentalApp.Backend.Helper;
// Imports
using CarRentalApp.Backend.Models;
// Biblio
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Database
{
    public class ReservationDao
    {
        // Admin
        // Index: Get All Reservation (Admin)
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> list = new List<Reservation>();
            string query = "SELECT * FROM reservations ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    list.Add(HelperReservation.MapReservation(reader));
            }
            return list;
        }
        // Show: GET Reservation BY ID (Admin/ maybe Client)
        public Reservation GetReservationById(int id)
        {
            string query = "SELECT * FROM reservations WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    return HelperReservation.MapReservation(reader);
            }
            return null;
        }

        // Filter : GET Reservation BY Status (Admin)
        public List<Reservation> GetReservationsByStatus(string status)
        {
            List<Reservation> list = new List<Reservation>();
            string query = "SELECT * FROM reservations WHERE status = @status ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", status);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperReservation.MapReservation(reader));
            }
            return list;
        }

        // Creating Reservation Processus
        // Add: Add Reservation (Admin)
        public bool AddReservation(Reservation reservation)
        {
            string query = @"
                INSERT INTO reservations 
                (car_id, user_id, start_date, end_date, total_price)
                VALUES (@car_id, @user_id, @start_date, @end_date, @total_price)";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@car_id", reservation.CarId);
                cmd.Parameters.AddWithValue("@user_id", reservation.UserId);
                cmd.Parameters.AddWithValue("@start_date", reservation.StartDate);
                cmd.Parameters.AddWithValue("@end_date", reservation.EndDate);
                cmd.Parameters.AddWithValue("@total_price", reservation.TotalPrice);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Check if car is booked during this period before reservation (used in Request Service)
        public bool IsCarBookedForPeriod(int carId, DateTime start, DateTime end)
        {
            string query = @"
                SELECT COUNT(*) FROM reservations
                WHERE car_id = @car_id
                AND status = 'ACTIVE'
                AND (
                    (start_date <= @end AND end_date >= @start)
                )
            ";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@car_id", carId);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // Calculate total price of the reservation (used in Request Service)
        public decimal CalculatePrice(int carId, DateTime start, DateTime end)
        {
            string query = "SELECT price_per_day FROM cars WHERE id = @car_id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@car_id", carId);

                decimal pricePerDay = Convert.ToDecimal(cmd.ExecuteScalar());

                int days = (end - start).Days;
                if (days <= 0) days = 1; // minimum 1 day

                return pricePerDay * days;
            }
        }

        // Update: Update Reservation Status
        public bool UpdateReservationStatus(int id, string newStatus)
        {
            string query = "UPDATE reservations SET status = @status WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", newStatus);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update: Update Reservation End Date With current Date
        public bool UpdateReservationEndDateWCurrentDate(int id)
        {
            string query = "UPDATE reservations SET end_date = CURRENT_DATE() WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // Client
        // Get Reservation BY User ID (Admin/Client)
        public List<Reservation> GetReservationsByUser(int userId)
        {
            List<Reservation> list = new List<Reservation>();
            string query = "SELECT * FROM reservations WHERE user_id = @user_id ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperReservation.MapReservation(reader));
            }
            return list;
        }

        // Get Reservation BY User ID And Status (Admin/Client)
        public List<Reservation> GetReservationsByUserAndStatus(int userId, string status)
        {
            List<Reservation> list = new List<Reservation>();
            string query = "SELECT * FROM reservations WHERE user_id = @user_id AND status = @status ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@status", status);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperReservation.MapReservation(reader));
            }
            return list;
        }

        // Get Reservations By Car ID
        public List<Reservation> GetReservationsByCar(int carId)
        {
            List<Reservation> list = new List<Reservation>();
            string query = "SELECT * FROM reservations WHERE car_id = @car_id ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@car_id", carId);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperReservation.MapReservation(reader));
            }
            return list;
        }

        // UPDATE Reservation PayementStatus (Used in payment Service)
        public bool UpdatePaymentStatus(int id, string newPayment)
        {
            string query = "UPDATE reservations SET payment_status = @payment WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@payment", newPayment);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        
        // Create Transaction to end reservation automaticly after enddate
        public void AutoFinishExpiredReservation()
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    // 1. Finish all maintenances whose end_date has passed
                    var finishQuery = @"
                        UPDATE reservations
                        SET status = 'FINISHED'
                        WHERE status = 'ACTIVE'
                        AND end_date <= NOW();
                    ";

                    using (var cmd = new MySqlCommand(finishQuery, conn, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Set all related cars to AVAILABLE
                    var carQuery = @"
                        UPDATE cars
                        SET status = 'AVAILABLE'
                        WHERE id IN (
                            SELECT car_id FROM reservations
                            WHERE status = 'FINISHED'
                            AND end_date <= NOW()
                        );
                    ";

                    using (var cmd = new MySqlCommand(carQuery, conn, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }


    }
}
