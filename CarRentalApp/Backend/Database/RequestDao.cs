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

        // Index: GET ALL Requests
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

        // Show: Get Request By ID
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

        // ADD: Add Request
        public bool AddRequest(Request r)
        {
            string q = @"
                INSERT INTO requests(client_id, car_id, start_date, end_date, message, created_at)
                VALUES(
                    @client,
                    @car,
                    COALESCE(@start, CURRENT_TIMESTAMP()), 
                    COALESCE(@end, DATE_ADD(COALESCE(@start, CURRENT_TIMESTAMP()), INTERVAL 1 DAY)),
                    COALESCE(@msg, 'No message sent'),
                    CURRENT_TIMESTAMP()
                )";// COALESCE(@status, 'PENDING'),

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);

                cmd.Parameters.AddWithValue("@client", r.ClientId);
                cmd.Parameters.AddWithValue("@car", r.CarId);

                cmd.Parameters.AddWithValue("@start",
                    r.StartDate == default ? (object)DBNull.Value : r.StartDate);

                cmd.Parameters.AddWithValue("@end",
                    r.EndDate == default ? (object)DBNull.Value : r.EndDate);

                //cmd.Parameters.AddWithValue("@status",
                //    string.IsNullOrWhiteSpace(r.Status) ? (object)DBNull.Value : r.Status);

                cmd.Parameters.AddWithValue("@msg",
                    string.IsNullOrWhiteSpace(r.Message) ? (object)DBNull.Value : r.Message);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // Update: Update Request partial: start_date/end_date/message
        public bool UpdateRequest(Request r)
        {
            string q = @"
                UPDATE requests SET
                    start_date = COALESCE(@start, start_date),
                    end_date   = COALESCE(@end, DATE_ADD(COALESCE(@start, start_date), INTERVAL 1 DAY)),
                    message    = COALESCE(@msg, 'No message sent'),
                    updated_at = CURRENT_TIMESTAMP()
                WHERE id = @id";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);

                cmd.Parameters.AddWithValue("@id", r.Id);

                cmd.Parameters.AddWithValue("@start",
                    r.StartDate == default ? (object)DBNull.Value : r.StartDate);

                cmd.Parameters.AddWithValue("@end",
                    r.EndDate == default ? (object)DBNull.Value : r.EndDate);

                cmd.Parameters.AddWithValue("@msg",
                    string.IsNullOrWhiteSpace(r.Message) ? (object)DBNull.Value : r.Message);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update: Update Request status
        public bool UpdateRequestStatus(int requestId, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return false;

            // normalize status to the DB enum format
            status = status.Trim().ToUpper();

            // Optional: you can validate allowed values here or let service do it.
            var allowed = new HashSet<string> { "PENDING", "ACCEPTED", "DENIED" };
            if (!allowed.Contains(status))
                return false;

            string q = @"
                UPDATE requests
                SET status = @status,
                    updated_at = CURRENT_TIMESTAMP()
                WHERE id = @id
            ";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                using (var cmd = new MySqlCommand(q, c))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id", requestId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Delete: Delete Request
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

        ////Get Pending requests
        //public List<Request> GetPendingRequests()
        //{
        //    List<Request> list = new List<Request>();
        //    string query = "SELECT * FROM requests WHERE status = 'Pending' ORDER BY created_at ASC";
        //    using (MySqlConnection conn = DbConnection.GetConnection())
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read()) list.Add(HelperRequest.MapRequest(reader));
        //    }
        //    return list;
        //}

        // Filters
        //Get requests by status
        public List<Request> GetRequestsByStatus(string status)
        {
            // Normalize input
            status = status?.Trim().ToUpper();

            // Allowed enum values
            var allowed = new HashSet<string> { "PENDING", "ACCEPTED", "DENIED" };

            // Invalid -> return EMPTY list immediately
            if (!allowed.Contains(status))
                return new List<Request>();

            // Valid → run query
            List<Request> list = new List<Request>();
            string query = @"SELECT * FROM requests WHERE status = @status ORDER BY created_at DESC";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", status);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(HelperRequest.MapRequest(reader));
            }

            return list;
        }

        // Fk
        // Get Pending Request For a specific Client(Admin Side)
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

        // Get Pending Request For a specific Car(Admin Side)
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

    }
}
