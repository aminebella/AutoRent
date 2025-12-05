using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int ClientId { get; set; }    // FK -> users.id
        public int CarId { get; set; }       // FK -> cars.id
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }   // enum('PENDING','ACCEPTED','DENIED')
        public string Message { get; set; }  // optional client message
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
