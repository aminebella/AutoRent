using backendclienttesting.Backend.Helper;
// Imports
using CarRentalApp.Backend.Helper;
using CarRentalApp.Backend.Models;
// Dependencies
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Database
{
    public class RequestDao
    {
        public RequestDao() { }

        // GET ALL
        public List<Request> GetAllRequests()
        {
            List<Request> list = new List<Request>();
            string query = "SELECT * FROM requests ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperRequest.MapRequest(reader));
            }
            return list;
        }

        // GET BY ID
        public Request GetRequestById(int id)
        {
            Request req = null;
            string query = "SELECT * FROM requests WHERE id = @id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    req = HelperRequest.MapRequest(reader);
            }
            return req;
        }

        // ADD REQUEST
        public bool AddRequest(Request r)
        {
            string query = @"
                INSERT INTO requests (client_id, car_id, start_date, end_date, status, message, created_at)
                VALUES (@client, @car, @start, @end, @status, @message, @created)";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@client", r.ClientId);
                cmd.Parameters.AddWithValue("@car", r.CarId);
                cmd.Parameters.AddWithValue("@start", r.StartDate.Date);
                cmd.Parameters.AddWithValue("@end", r.EndDate.Date);
                cmd.Parameters.AddWithValue("@status", "Pending");
                cmd.Parameters.AddWithValue("@message", (object)r.Message ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@created", r.CreatedAt == default ? DateTime.Now : r.CreatedAt);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // UPDATE (partial: update status/message)
        public bool UpdateRequest(Request r)
        {
            string query = @"
                UPDATE requests SET
                    start_date = @start,
                    end_date = @end,
                    message = @message,
                    updated_at = @updated
                WHERE id = @id"; //status = @status,

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", r.Id);
                cmd.Parameters.AddWithValue("@start", r.StartDate.Date);
                cmd.Parameters.AddWithValue("@end", r.EndDate.Date);
                //cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.Parameters.AddWithValue("@message", (object)r.Message ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@updated", DateTime.Now);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // DELETE
        public bool DeleteRequest(int id)
        {
            string query = "DELETE FROM requests WHERE id = @id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Filters
        // Get Request Client and Admin Side for a specific client with status pending
        public List<Request> GetRequestsByClient(int clientId)
        {
            List<Request> list = new List<Request>();
            string query = "SELECT * FROM requests WHERE client_id = @client AND status = 'PENDING' ORDER BY created_at DESC";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@client", clientId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(HelperRequest.MapRequest(reader));
            }
            return list;
        }

        // Get Request Admin side for a specific car with status pending
        public List<Request> GetRequestsByCar(int carId)
        {
            List<Request> list = new List<Request>();
            string query = "SELECT * FROM requests WHERE car_id = @car AND status = 'PENDING' ORDER BY created_at DESC";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@car", carId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(HelperRequest.MapRequest(reader));
            }
            return list;
        }

        //Get Pending requests
        public List<Request> GetPendingRequests()
        {
            List<Request> list = new List<Request>();
            string query = "SELECT * FROM requests WHERE status = 'Pending' ORDER BY created_at ASC";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(HelperRequest.MapRequest(reader));
            }
            return list;
        }
    }
}
