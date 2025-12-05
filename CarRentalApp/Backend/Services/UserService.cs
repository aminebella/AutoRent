using CarRentalApp.Backend.Database;
using CarRentalApp.Backend.Helper;
using CarRentalApp.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Services
{
    public class UserService
    {
        private readonly UserDao userDao;

        public UserService()
        {
            userDao = new UserDao();
        }

        // 🔹 Get all clients
        public List<User> GetAllClients()
        {
            return userDao.GetAllClients();
        }

        // 🔹 Get client by Id
        public User GetClientById(int id)
        {
            return userDao.GetByIdClient(id);
        }

        // 🔹 Add new client
        public bool AddClient(User user)
        {
            // Optional validation before inserting
            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password))
                return false;

            user.Role = "client"; // Ensure role always client
            return userDao.AddClient(user);
        }

        // 🔹 Update client
        public bool UpdateClient(User user)
        {
            if (user.Id <= 0)
                return false;

            user.Role = "CLIENT";

            return userDao.UpdateClient(user);
        }

        // 🔹 Delete client
        public bool DeleteClient(int id)
        {
            return userDao.DeleteClient(id);
        }

        // Search By last_name
        public List<User> SearchClientByLastName(string LastName)
        {
            List<User> clients = new List<User>();
            string query = "SELECT * FROM users where last_name like @format";
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@format", '%'+LastName+'%');
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    clients.Add(HelperClient.MapUser(reader));
                }

            }
            return clients;
        }
    }
}
