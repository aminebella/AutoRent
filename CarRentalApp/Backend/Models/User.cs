using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Models
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } // "ADMIN" or "CLIENT"
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
