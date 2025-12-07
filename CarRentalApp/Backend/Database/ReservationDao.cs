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
        // GET ALL Reservation : GetAllReservations() (admin)
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

        // ADD Reservation : AddReservation(Reservation r)
        public bool AddReservation(Reservation reservation)
        {
            string query = @"
                INSERT INTO reservations 
                (car_id, user_id, start_date, end_date, total_price, status, payment_status)
                VALUES (@car_id, @user_id, @start_date, @end_date, @total_price, @status, @payment_status)";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@car_id", reservation.CarId);
                cmd.Parameters.AddWithValue("@user_id", reservation.UserId);
                cmd.Parameters.AddWithValue("@start_date", reservation.StartDate);
                cmd.Parameters.AddWithValue("@end_date", reservation.EndDate);
                cmd.Parameters.AddWithValue("@total_price", reservation.TotalPrice);
                cmd.Parameters.AddWithValue("@status", reservation.Status);
                cmd.Parameters.AddWithValue("@payment_status", reservation.PaymentStatus);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // GET Reservation BY Status(Generic for status filters): GetReservationsByStatus(string status)
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
        // GET Reservation BY ID : GetReservationById(int id)
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
        // Check if car is booked during this period before reservation (used in request) : IsCarBookedForPeriod(carId, start, end)
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

        // calculate total price of the reservation : CalculatePrice
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

        // UPDATE Reservation Status : UpdateReservationStatus(id, newStatus)
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

        // Client
        // GET Reservation BY User ID : GetReservationsByUser(int userId)
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

        // GET Reservation BY User ID And Status:
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

        // UPDATE Reservation PayementStatus : UpdatePaymentStatus(id, newStatus)
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

    }
}
