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

        // Admin:
        // Admin auth see Auth service
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

            user.Role = "CLIENT"; // Ensure role always client
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

        // Search By first_name
        public List<User> SearchClientByFirstName(string value) => userDao.SearchClientByAtt("first_name", value);

        // Search By last_name
        public List<User> SearchClientByLastName(string value) => userDao.SearchClientByAtt("last_name", value);

        // Search By email
        public List<User> SearchClientByEmail(string value) => userDao.SearchClientByAtt("email", value);

        // See list of request of specific client : SEE REQUEST SERVICE

        // See list of reservation of specific client : SEE RESERVATION SERVICE



        // Client Auth : See Auth service
    }
}
