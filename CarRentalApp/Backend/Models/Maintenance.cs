using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backendclienttesting.Backend.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }   // ACTIVE or FINISHED
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
