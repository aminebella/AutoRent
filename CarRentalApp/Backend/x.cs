//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CarRentalApp.Backend.Models;
//using CarRentalApp.Backend.Services;

//namespace CarRentalApp.Backend
//{
//    internal class x
//    {
//        static void Main()
//        {
//            UserService userService = new UserService();

//            // 🔹 Test GetAllClients
//            var clients = userService.GetAllClients();
//            foreach (var c in clients)
//            {
//                Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - {c.Email}");
//            }

//            // 🔹 Test AddClient
//            var newClient = new User
//            {
//                FirstName = "Test",
//                LastName = "Client",
//                Phone = "123456789",
//                Email = "testclient@example.com",
//                Password = "12345"
//            };
//            bool added = userService.AddClient(newClient);
//            Console.WriteLine("Added: " + added);

//            // 🔹 Test UpdateClient
//            if (clients.Count > 0)
//            {
//                var first = clients[0];
//                first.FirstName = "UpdatedName";
//                bool updated = userService.UpdateClient(first);
//                Console.WriteLine("Updated: " + updated);
//            }

//            // 🔹 Test DeleteClient
//            if (clients.Count > 0)
//            {
//                int idToDelete = clients[0].Id;
//                bool deleted = userService.DeleteClient(idToDelete);
//                Console.WriteLine("Deleted: " + deleted);
//            }
//        }
//    }
//}
