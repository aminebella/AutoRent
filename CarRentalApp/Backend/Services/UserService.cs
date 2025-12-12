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

        // Verify att if null or not
        private bool IsValid(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName)) return false;
            if (string.IsNullOrWhiteSpace(user.LastName)) return false;
            if (string.IsNullOrWhiteSpace(user.Email)) return false;
            if (string.IsNullOrWhiteSpace(user.Password)) return false;
            if (user.Password.Length < 5) return false;

            return true;
        }

        // Admin:
        // Index: Get all active clients
        public List<User> GetAllClients() => userDao.GetAllClients();
        

        // Filter: Get Inactive clients
        public List<User> GetInactiveClients() => userDao.GetInactiveClients();

        // show: Get client by Id
        public User GetClientById(int id) => userDao.GetByIdClient(id);

        // Add: Add new client
        public bool AddClient(User user)
        {
            // validation before inserting
            if (!IsValid(user)) return false;
            return userDao.AddClient(user);
        }

        // Update: Update client
        public bool UpdateClient(User user)
        {
            if (user.Id <= 0)
                return false;
            if (!IsValid(user)) return false;
            user.Password = userDao.HashPassword(user.Password); // Hash
            return userDao.UpdateClient(user);
        }


        // Inactive Client: Soft delete Client
        public bool DeactivateClient(int id) => userDao.DeleteClient(id);

        // Reactivate Client
        public bool ActivateClient(int id) => userDao.ActivateClient(id);

        // Filter
        // Search By first_name
        public List<User> SearchClientByFirstName(string value) => userDao.SearchClientByAtt("first_name", value);

        // Search By last_name
        public List<User> SearchClientByLastName(string value) => userDao.SearchClientByAtt("last_name", value);

        // Search By email
        public List<User> SearchClientByEmail(string value) => userDao.SearchClientByAtt("email", value);
    }
}
