using CarRentalApp.Backend.Helper;
using CarRentalApp.Backend.Models;
//using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using CarRentalApp.Backend.Models;
// Bblio
using BCrypt.Net;


namespace CarRentalApp.Backend.Database
{
    public class UserDao
    {
        public UserDao() { }

        // Index : // GetAllClients Active Ones(Not Deleted)
        public List<User> GetAllClients()//active ones
        {
            List<User> clients = new List<User>();
            string query = "SELECT * FROM users WHERE role = @role AND is_active = true";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@role", "CLIENT");
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(HelperClient.MapUser(reader));
                }
            }
            return clients;
        }

        // Filter : // GetAllInactiveClients (Deleted)
        public List<User> GetInactiveClients()
        {
            List<User> clients = new List<User>();
            string query = "SELECT * FROM users WHERE role = 'CLIENT' AND is_active = false";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    clients.Add(HelperClient.MapUser(reader));
            }
            return clients;
        }


        // Show: // GetByIdClient
        public User GetByIdClient(int id) {
            User user = null;
            string query = "SELECT * FROM Users WHERE Id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id",id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = HelperClient.MapUser(reader);
                }
            }
            return user;
        }

        // Add: // AddClient
        public bool AddClient(User user)
        {
            string query = @"INSERT INTO users (first_name, last_name, phone, role, email, password, is_active)
                                 VALUES (@first_name, @last_name, @phone, @role, @email, @password, @is_active)";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@first_name", user.FirstName);
                cmd.Parameters.AddWithValue("@last_name", user.LastName);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@role", "CLIENT");
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@is_active", true);
                return cmd.ExecuteNonQuery()>0;
            }
        }

        // Update: //UpdateClient
        public bool UpdateClient(User user)
        {
            string query = @"UPDATE users SET 
                                 first_name=@first_name, last_name=@last_name, phone=@phone, 
                                 email=@email, password=@password
                           WHERE id=@id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", user.Id);
                cmd.Parameters.AddWithValue("@first_name", user.FirstName);
                cmd.Parameters.AddWithValue("@last_name", user.LastName);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", HashPassword(user.Password));// Hash password

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        // Delete: DeleteClient
        public bool DeleteClient(int id)
        {
            //string query = "DELETE FROM users WHERE id=@id";
            string query = "UPDATE users SET is_active = false WHERE id=@id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                
                return cmd.ExecuteNonQuery() > 0;
            }

        }

        // Activate Client: //Reactivate client (Opposite of delete)
        public bool ActivateClient(int id)
        {
            string query = "UPDATE users SET is_active = 1 WHERE id=@id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Filter
        // Search By Att
        public List<User> SearchClientByAtt(string columnName, string value)
        {
            List<User> clients = new List<User>();
            // Allowed columns
            List<string> allowedColumns = new List<string> { "first_name", "last_name", "email" };
            if (!allowedColumns.Contains(columnName.ToLower()))
                throw new ArgumentException("Invalid column name");

            string query = $"SELECT * FROM users where {columnName} like @format";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@format", '%' + value + '%');
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    clients.Add(HelperClient.MapUser(reader));
                }

            }
            return clients;
        }

        // Hash Password
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

    }
}
