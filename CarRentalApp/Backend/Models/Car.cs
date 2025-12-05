using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public decimal PricePerDay { get; set; }
        public string Status { get; set; }   // enum("AVAILABLE", "RESERVED", "MAINTENANCE", "UNAVAILABLE")
        //public int CategoryId { get; set; }  // FK → categories table
        public string CategoryName { get; set; } // enum('ECONOMY', 'SPORT', 'MIDSIZE', 'SUV')
    }
}