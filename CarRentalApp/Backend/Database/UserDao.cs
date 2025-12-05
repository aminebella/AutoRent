using CarRentalApp.Backend.Helper;
using CarRentalApp.Backend.Models;
//using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using CarRentalApp.Backend.Models;

namespace CarRentalApp.Backend.Database
{
    public class UserDao
    {
        public UserDao() { }

        // GetAllClients
        public List<User> GetAllClients()
        {
            List<User> clients = new List<User>();
            string query = "SELECT * FROM users WHERE role = @role";

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


        // GetByIdClient
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

        // AddClient
        public bool AddClient(User user)
        {
            string query = @"INSERT INTO users (first_name, last_name, phone, role, email, password)
                                 VALUES (@first_name, @last_name, @phone, @role, @email, @password)";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@first_name", user.FirstName);
                cmd.Parameters.AddWithValue("@last_name", user.LastName);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                return cmd.ExecuteNonQuery()>0;
            }
        }

        // UpdateClient
        public bool UpdateClient(User user)
        {
            string query = @"UPDATE users SET 
                                 first_name=@first_name, last_name=@last_name, phone=@phone, 
                                 role=@role, email=@email, password=@password
                           WHERE id=@id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", user.Id);
                cmd.Parameters.AddWithValue("@first_name", user.FirstName);
                cmd.Parameters.AddWithValue("@last_name", user.LastName);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        // DeleteClient
        public bool DeleteClient(int id)
        {
            string query = "DELETE FROM users WHERE id=@id";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                
                return cmd.ExecuteNonQuery() > 0;
            }

        }

        //SearchClientByAtt
        //public List<User> SearchClientByAtt(string colonne, string format)
        //{
        //    List<User> clients = new List<User>();
        //    string query = "SELECT * FROM users where @colonne= @format";
        //    using (MySqlConnection conn = DbConnection.GetConnection())
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@colonne", colonne);
        //        cmd.Parameters.AddWithValue("@format", format);
        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            clients.Add(HelperClient.MapUser(reader));
        //        }

        //    }
        //    return clients;
        //}

    }
}
