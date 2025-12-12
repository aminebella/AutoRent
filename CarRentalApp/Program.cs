using backendclienttesting.Backend.Models;
using CarRentalApp.Backend.Database;
using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using Mysqlx;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRentalApp.Backend
{
    public class Program
    {
        // Users
        //static void Main()
        //{
        //    UserService userService = new UserService();
        //    int choice;

        //    do
        //    {
        //        Console.WriteLine("\n===== MENU =====");
        //        Console.WriteLine("1. Admin: Afficher tous les clients");
        //        Console.WriteLine("2. Admin: Afficher client par id");
        //        Console.WriteLine("3. Admin: Ajouter un client");
        //        Console.WriteLine("4. Admin: Modifier un client");
        //        Console.WriteLine("5. Admin: Supprimer un client");
        //        Console.WriteLine("6. Admin: Rechercher un client par first_name");
        //        Console.WriteLine("7. Admin: Rechercher un client par last_name");
        //        Console.WriteLine("8. Admin: Rechercher un client par email");
        //        Console.WriteLine("9. Admin: afficher les demandes(requests) d'un client");
        //        Console.WriteLine("10. Admin: afficher les reservations d'un client");
        //        Console.WriteLine("11. Admin: Désactiver un client");
        //        Console.WriteLine("12. Admin: Réactiver un client");
        //        Console.WriteLine("13. Admin: Afficher clients désactivés");

        //        Console.WriteLine("0. Quitter");
        //        Console.Write("Choix : ");
        //        choice = int.Parse(Console.ReadLine());

        //        switch (choice)
        //        {
        //            case 1:
        //                foreach (User c in userService.GetAllClients())
        //                    Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - Phone: {c.Phone ?? "Doesn't have one"} - Email: {c.Email} - Active: {c.IsActive}");
        //                break;

        //            case 2:
        //                Console.WriteLine("Enter User you wanna search:");
        //                int Ids = int.Parse(Console.ReadLine());
        //                User client = userService.GetClientById(Ids);
        //                Console.WriteLine($"{client.Id} - {client.FirstName} {client.LastName} - Phone: {client.Phone ?? "Doesn't have one"} - Email: {client.Email}");
        //                break;

        //            case 3:
        //                Console.WriteLine("Enter FirstName:");
        //                string FirstName = Console.ReadLine();

        //                Console.WriteLine("Enter LastName:");
        //                string LastName = Console.ReadLine();

        //                Console.WriteLine("Enter Phone:");
        //                string Phone = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(Phone))
        //                    Phone = null;

        //                Console.WriteLine("Enter Email:");
        //                string Email = Console.ReadLine();

        //                Console.WriteLine("Enter Password:");
        //                string Password = Console.ReadLine();

        //                User newClient = new User
        //                {
        //                    FirstName = FirstName,
        //                    LastName = LastName,
        //                    Phone = Phone,
        //                    Email = Email,
        //                    Password = Password
        //                };
        //                bool added = userService.AddClient(newClient);
        //                Console.WriteLine("Added: " + added);
        //                break;

        //            case 4:
        //                Console.WriteLine("Enter User Id you wanna update:");
        //                int IdU = int.Parse(Console.ReadLine());
        //                User clientU = userService.GetClientById(IdU);
        //                if (clientU == null)
        //                {
        //                    Console.WriteLine("Client not found.");
        //                    break;
        //                }

        //                Console.WriteLine($"FirstName ({clientU.FirstName}): ");
        //                string FirstNameU = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(FirstNameU))
        //                    FirstNameU = clientU.FirstName;

        //                Console.WriteLine($"LastName ({clientU.LastName}): ");
        //                string LastNameU = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(LastNameU))
        //                    LastNameU = clientU.LastName;

        //                Console.WriteLine($"Phone ({clientU.Phone ?? "None"}): ");
        //                string PhoneU = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(PhoneU))
        //                    PhoneU = clientU.Phone;     // keep old phone

        //                Console.WriteLine($"Email ({clientU.Email}): ");
        //                string EmailU = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(EmailU))
        //                    EmailU = clientU.Email;

        //                Console.WriteLine($"Password (hidden): ");
        //                string PasswordU = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(PasswordU))
        //                    PasswordU = clientU.Password;

        //                User updatedClient = new User
        //                {
        //                    Id = IdU,
        //                    FirstName = FirstNameU,
        //                    LastName = LastNameU,
        //                    Phone = PhoneU,
        //                    Email = EmailU,
        //                    Password = PasswordU
        //                };
        //                bool updated = userService.UpdateClient(updatedClient);
        //                Console.WriteLine("Updated: " + updated);
        //                break;

        //            case 5:
        //                Console.WriteLine("Enter User Id you wanna Delete:");
        //                int IdD = int.Parse(Console.ReadLine());
        //                bool deleted = userService.DeactivateClient(IdD);
        //                Console.WriteLine("Deleted: " + deleted);
        //                break;

        //            case 6:
        //                Console.WriteLine("Enter the FirstName of the user you are looking for:");
        //                string FirstNameS = Console.ReadLine();
        //                foreach (User c in userService.SearchClientByFirstName(FirstNameS))
        //                    Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - {c.Email}");
        //                break;
        //            case 7:
        //                Console.WriteLine("Enter the LastName of the user you are looking for:");
        //                string LastNameS = Console.ReadLine();
        //                foreach (User c in userService.SearchClientByLastName(LastNameS))
        //                    Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - {c.Email}");
        //                break;
        //            case 8:
        //                Console.WriteLine("Enter the Email of the user you are looking for:");
        //                string EmailS = Console.ReadLine();
        //                foreach (User c in userService.SearchClientByEmail(EmailS))
        //                    Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - {c.Email}");
        //                break;

        //            case 9:
        //                Console.WriteLine("Enter the Id of the client you wanna see its requests:");
        //                int idCReq = int.Parse(Console.ReadLine());
        //                RequestService requestService = new RequestService();
        //                List<Request> requestsClient = requestService.GetRequestsByClient(idCReq);
        //                foreach (Request req in requestsClient)
        //                    Console.WriteLine($"{req.Id} - Client:{req.ClientId} Car:{req.CarId}| Message: {req.Message ?? "No Message"} | {req.StartDate:yyyy-MM-dd} to {req.EndDate:yyyy-MM-dd} | {req.Status}");
        //                break;

        //            case 10:
        //                Console.WriteLine("Enter the Id of the client you wanna see its reservation:");
        //                int idCRes = int.Parse(Console.ReadLine());
        //                ReservationService reservationService = new ReservationService();
        //                List<Reservation> reservationsClient = reservationService.GetReservationsByUser(idCRes);
        //                foreach (Reservation res in reservationsClient)
        //                    Console.WriteLine($"Id: {res.Id} - ClientId: {res.UserId} - CarId: {res.CarId} | {res.StartDate:yyyy-MM-dd} to {res.EndDate:yyyy-MM-dd} | TotalPrice: {res.TotalPrice} | Status: {res.Status}| PaymentStatus: {res.PaymentStatus}| CreatedAt: {res.CreatedAt} | UpdatedAt: {res.UpdatedAt}.");
        //                break;

        //            case 11:
        //                Console.WriteLine("Enter User Id to deactivate:");
        //                int idDeact = int.Parse(Console.ReadLine());
        //                Console.WriteLine("Deactivated: " + userService.DeactivateClient(idDeact));
        //                break;

        //            case 12:
        //                Console.WriteLine("Enter User Id to activate:");
        //                int idAct = int.Parse(Console.ReadLine());
        //                Console.WriteLine("Activated: " + userService.ActivateClient(idAct));
        //                break;

        //            case 13:
        //                foreach (User c in userService.GetInactiveClients())
        //                    Console.WriteLine($"{c.Id} - {c.FirstName} {c.LastName} - INACTIVE");
        //                break;

        //        }

        //    } while (choice != 0);
        //}

        //Cars // Maint
        static void Main()
        {
            CarService carService = new CarService();
            MaintenanceService maintSrv = new MaintenanceService();
            int choice;

            do
            {
                Console.WriteLine("\n===== CAR MENU =====");
                Console.WriteLine("1. Admin/Client: Show all cars");
                Console.WriteLine("2. Admin/Client: Show car by ID");
                Console.WriteLine("3. Admin: Add new car");
                Console.WriteLine("4. Admin: Update car");
                //Console.WriteLine("5. Admin: Delete car");
                Console.WriteLine("6. Admin/Client: Search car by model");
                Console.WriteLine("7. Admin/Client: Filter cars by brand");
                Console.WriteLine("8. Admin/Client: Filter cars by status");
                Console.WriteLine("9. Admin/Client: Filter cars by category");
                Console.WriteLine("10. Admin/Client: Filter cars by color");
                Console.WriteLine("11. Admin/Client: Filter cars by year");
                Console.WriteLine("12. Admin: show request with status pending of this car");
                Console.WriteLine("13. Admin: show reservation(all status) of this car");
                Console.WriteLine("14. Admin: Make Car UNAVAILABLE");
                Console.WriteLine("15. Admin: Make Car AVAILABLE");
                Console.WriteLine("16. Admin: Send car to MAINTENANCE");
                Console.WriteLine("17. Admin: End Maintenance");
                Console.WriteLine("18. Admin: List of Maintenance of a car");
                Console.WriteLine("---");
                Console.WriteLine("19. Admin: Show ALL maintenance");
                Console.WriteLine("20. Admin: Show FINISHED maintenance");
                Console.WriteLine("21. Admin: Show ACTIVE maintenance");
                Console.WriteLine("22. Admin: Get maintenance by ID");
                Console.WriteLine("23. Admin: Update maintenance");
                Console.WriteLine("24. Admin: AutoEnd maintenance");
                Console.WriteLine("25: Admin: AUTO CHECK MAINTENANCE FOR ALL CARS");

                Console.WriteLine("0. Quit");
                Console.Write("Choice: ");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        foreach (Car car in carService.GetAllCars())
                            Console.WriteLine($"{car.Id} - {car.Brand} {car.Model} - {car.Color} - {car.PricePerDay}$/day - Category: {car.CategoryName}");
                        break;

                    case 2:
                        Console.Write("Enter ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Car c = carService.GetCarById(id);

                        if (c != null)
                            Console.WriteLine($"{c.Id} - {c.Brand} {c.Model} - {c.Color} - {c.PricePerDay}$/day - Category: {c.CategoryName}");
                        else
                            Console.WriteLine("Car not found.");
                        break;

                    case 3:
                        Console.WriteLine("Enter Brand:");
                        string brand = Console.ReadLine();

                        Console.WriteLine("Enter Model:");
                        string model = Console.ReadLine();

                        Console.WriteLine("Enter Year:");
                        int year = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Color (or leave empty for default 'No Color'):");
                        string color = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(color))
                            color = "No Color"; // default value in DB

                        Console.WriteLine("Enter Price Per Day:");
                        decimal pricePerDay = decimal.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Category Name (ECONOMY / SPORT / MIDSIZE / SUV) or leave empty for default 'ECONOMY':");
                        string categoryName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(categoryName))
                            categoryName = "ECONOMY"; // default value in DB

                        // --- NEW: Last Maintenance Date ---
                        Console.WriteLine("Enter Last Maintenance Date (yyyy-mm-dd) or leave empty for today:");
                        string lastMaintInput = Console.ReadLine();
                        DateTime LastMaintenanceDate = string.IsNullOrWhiteSpace(lastMaintInput)
                                                       ? default    // DateTime.Now   default = today
                                                       : DateTime.Parse(lastMaintInput);

                        // --- NEW: Maintenance Interval Days ---
                        Console.WriteLine("Enter Maintenance Interval Days or leave empty for 180:");
                        string intervalInput = Console.ReadLine();
                        int MaintenanceIntervalDays = string.IsNullOrWhiteSpace(intervalInput)
                                                     ? 0  // default = 180
                                                     : int.Parse(intervalInput);

                        // LastMaintenanceDate and MaintenanceIntervalDays will use DB defaults if not provided
                        Car newCar = new Car
                        {
                            Brand = brand,
                            Model = model,
                            Year = year,
                            Color = color,
                            PricePerDay = pricePerDay,
                            CategoryName = categoryName,
                            LastMaintenanceDate = LastMaintenanceDate,
                            MaintenanceIntervalDays = MaintenanceIntervalDays
                        };

                        Console.WriteLine("Added: " + carService.AddCar(newCar));
                        break;


                    case 4:
                        Console.WriteLine("Enter ID of car to update:");
                        int updateId = int.Parse(Console.ReadLine());
                        Car existingCar = carService.GetCarById(updateId);
                        if (existingCar == null)
                        {
                            Console.WriteLine("Car not found.");
                            break;
                        }

                        Console.WriteLine("New Brand (leave empty to keep current):");
                        string uBrand = Console.ReadLine();

                        Console.WriteLine("New Model (leave empty to keep current):");
                        string uModel = Console.ReadLine();

                        Console.WriteLine("New Year (leave empty to keep current):");
                        string uYearInput = Console.ReadLine();

                        Console.WriteLine("New Color (leave empty to keep current):");
                        string uColor = Console.ReadLine();

                        Console.WriteLine("New Price Per Day (leave empty to keep current):");
                        string uPriceInput = Console.ReadLine();

                        Console.WriteLine("New Category Name (leave empty to keep current):");
                        string uCategory = Console.ReadLine();

                        Console.WriteLine("New Last Maintenance Date (yyyy-mm-dd) or leave empty to keep current:");
                        string uLastMaintInput = Console.ReadLine();
                        DateTime uLastMaintenanceDate = string.IsNullOrWhiteSpace(uLastMaintInput)
                                                         ? default   // marker for service to keep old value
                                                         : DateTime.Parse(uLastMaintInput);

                        Console.WriteLine("New Maintenance Interval Days (leave empty to keep current):");
                        string uIntervalInput = Console.ReadLine();
                        int uMaintenanceIntervalDays = string.IsNullOrWhiteSpace(uIntervalInput)
                                                       ? 0   // marker for service to keep old value
                                                       : int.Parse(uIntervalInput);

                        // Build update object, let service handle old values
                        Car updateCar = new Car
                        {
                            Id = updateId,
                            Brand = uBrand,
                            Model = uModel,
                            Year = string.IsNullOrWhiteSpace(uYearInput) ? 0 : int.Parse(uYearInput),
                            Color = uColor,
                            PricePerDay = string.IsNullOrWhiteSpace(uPriceInput) ? 0 : decimal.Parse(uPriceInput),
                            CategoryName = uCategory,
                            LastMaintenanceDate = uLastMaintenanceDate,
                            MaintenanceIntervalDays = uMaintenanceIntervalDays
                            // Status & Maintenance fields not changed here
                        };

                        Console.WriteLine("Updated: " + carService.UpdateCar(updateCar));
                        break;


                    //case 5:
                    //    Console.Write("Enter ID to delete: ");
                    //    int delId = int.Parse(Console.ReadLine());
                    //    Console.WriteLine("Deleted: " + carService.DeleteCar(delId));
                    //    break;

                    case 6:
                        Console.Write("Model search: ");
                        string Smodel = Console.ReadLine();

                        foreach (Car car in carService.SearchCarsByModel(Smodel))
                            Console.WriteLine($"{car.Brand} {car.Model} - {car.Color}");
                        break;

                    case 7:
                        Console.Write("Brand: ");
                        string Sbrand = Console.ReadLine();

                        foreach (Car car in carService.SearchCarsByBrand(Sbrand))
                            Console.WriteLine($"{car.Brand} {car.Model} - {car.Color}");
                        break;

                    case 8:
                        Console.Write("Status: ");
                        string status = Console.ReadLine();

                        foreach (Car car in carService.SearchCarsByStatus(status))
                            Console.WriteLine($"{car.Brand} {car.Model} - {car.Status}");
                        break;

                    case 9:
                        Console.Write("Category Name (NOT ID): ");
                        string category = Console.ReadLine();

                        foreach (Car car in carService.SearchCarsByCategory(category))
                            Console.WriteLine($"{car.Brand} {car.Model} - Category: {car.CategoryName}");
                        break;

                    case 10:
                        Console.Write("Color: ");
                        string Scolor = Console.ReadLine();

                        foreach (Car car in carService.SearchCarsByColor(Scolor))
                            Console.WriteLine($"{car.Brand} {car.Model} - {car.Color}");
                        break;

                    case 11:
                        Console.Write("Year: ");
                        int Syear = int.Parse(Console.ReadLine());

                        foreach (Car car in carService.SearchCarsByYear(Syear))
                            Console.WriteLine($"{car.Brand} {car.Model} - {car.Color} - year: {car.Year}");
                        break;

                    case 12:
                        Console.WriteLine("Enter the Id of the car you wanna see its requests:");
                        int idCarReq = int.Parse(Console.ReadLine());
                        RequestService requestService = new RequestService();
                        List<Request> requestsCar = requestService.GetRequestsByCar(idCarReq);
                        foreach (Request req in requestsCar)
                            Console.WriteLine($"{req.Id} - Client:{req.ClientId} Car:{req.CarId}| Message: {req.Message ?? "No Message"} | {req.StartDate:yyyy-MM-dd} to {req.EndDate:yyyy-MM-dd} | {req.Status}");
                        break;

                    case 13:
                        Console.WriteLine("Enter the Id of the car you wanna see its reservation:");
                        int idCarRes = int.Parse(Console.ReadLine());
                        ReservationService reservationService = new ReservationService();
                        List<Reservation> reservationsCar = reservationService.GetReservationsByCar(idCarRes);
                        foreach (Reservation res in reservationsCar)
                            Console.WriteLine($"{res.Id} - Client:{res.UserId} Car:{res.CarId}| start: {res.StartDate:yyyy-MM-dd} to {res.EndDate:yyyy-MM-dd} | TotalPrice: {res.TotalPrice} | status: {res.Status} | PaymentStatus: {res.PaymentStatus}");
                        break;

                    case 14:
                        Console.WriteLine("Enter the Id of the car you wanna Make UNAVAILABLE:");
                        int idCarUnAV = int.Parse(Console.ReadLine());
                        if (carService.MarkUnavailable(idCarUnAV))
                        {
                            Console.WriteLine("the car know is Unavailable");
                        }
                        else
                        {
                            Console.WriteLine("!! there is error we can't make car unavailable plz check if car is available first if not we can't do so");
                        }
                        break;

                    case 15:
                        Console.WriteLine("Enter the Id of the car you wanna Make AVAILABLE:");
                        int idCarAV = int.Parse(Console.ReadLine());
                        if (carService.MarkAvailable(idCarAV))
                        {
                            Console.WriteLine("the car know is available");
                        }
                        else
                        {
                            Console.WriteLine("!! there is error we can't make car available plz check if car is Unavailable first if not that we can't do so(if maint or reserved)");
                        }
                        break;

                    case 16:
                        Console.WriteLine("Enter the ID of the car you want to send to maintenance:");
                        int carId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter End Date (yyyy-MM-dd) or press ENTER to use default (10 days):");
                        string endInput = Console.ReadLine();
                        DateTime? endDate = null;
                        if (!string.IsNullOrWhiteSpace(endInput))
                        {
                            if (DateTime.TryParse(endInput, out DateTime parsedEnd))
                                endDate = parsedEnd;
                            else
                            {
                                Console.WriteLine("Invalid date format. Operation cancelled.");
                                break;
                            }
                        }

                        Console.WriteLine("Enter description (optional):");
                        string desc = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(desc))
                            desc = null;

                        Maintenance maint = new Maintenance
                        {
                            CarId = carId,
                            StartDate = null,     // NULL → MySQL uses CURRENT_DATE
                            EndDate = endDate,    // NULL → MySQL adds +10 days
                            Description = desc
                        };

                        MaintenanceService ms = new MaintenanceService();
                        bool sent = ms.SendToMaintenance(maint);

                        Console.WriteLine(sent ? "Car sent to maintenance!" : "Failed to send car to maintenance.");
                        break;

                    case 17:
                        Console.WriteLine("Enter the ID of the car to finish maintenance:");
                        int carIdFinish = int.Parse(Console.ReadLine());

                        MaintenanceService maintenanceService2 = new MaintenanceService();
                        bool done = maintenanceService2.FinishMaintenance(carIdFinish);

                        if (done)
                            Console.WriteLine("Maintenance finished successfully! Car is now AVAILABLE.");
                        else
                            Console.WriteLine("Failed to finish maintenance. Car may not be under active maintenance.");

                        break;

                    case 18:
                        Console.WriteLine("Enter the ID of the car you wanna see its list of maintenance:");
                        int carIdMaint = int.Parse(Console.ReadLine());

                        MaintenanceDao maintenanceDAO = new MaintenanceDao();
                        List<Maintenance> listMaintCar = maintenanceDAO.GetMaintByCar(carIdMaint);

                        foreach (Maintenance m in listMaintCar)
                        {
                            Console.WriteLine($"Car ID: {m.CarId} - Start Date: {m.StartDate:yyyy-MM-dd} - End Date: {(m.EndDate.HasValue ? m.EndDate.Value.ToString("yyyy-MM-dd") : "NULL")} - Description  : {m.Description} - Status       : {m.Status}");
                        }

                        break;

                    case 19:
                        {
                            MaintenanceService msA = new MaintenanceService();
                            var list = msA.GetAllMaintenance();

                            Console.WriteLine("\n--- All Maintenance Records ---");

                            foreach (var m in list)
                            {
                                Console.WriteLine($"Car: {m.CarId} | Start: {m.StartDate:yyyy-MM-dd} | End: {(m.EndDate?.ToString("yyyy-MM-dd") ?? "NULL")} | Status: {m.Status} | Description: {m.Description}");
                            }
                        }
                        break;

                    case 20:
                        {
                            MaintenanceService msS = new MaintenanceService();
                            var list = msS.GetFinishedAll();

                            Console.WriteLine("\n--- Finished Maintenance ---");

                            foreach (var m in list)
                            {
                                Console.WriteLine($"Car: {m.CarId} | Start: {m.StartDate:yyyy-MM-dd} | End: {m.EndDate:yyyy-MM-dd} | Description: {m.Description}");
                            }
                        }
                        break;

                    case 21:
                        {
                            MaintenanceService msAct = new MaintenanceService();
                            var list = msAct.GetActiveAll();

                            Console.WriteLine("\n--- Active Maintenance ---");

                            foreach (var m in list)
                            {
                                Console.WriteLine($"Car: {m.CarId} | Start: {m.StartDate:yyyy-MM-dd} | Description: {m.Description}");
                            }
                        }
                        break;

                    case 22:
                        {
                            Console.Write("Enter maintenance ID: ");
                            int mid = int.Parse(Console.ReadLine());

                            MaintenanceService msByID = new MaintenanceService();
                            var m = msByID.GetById(mid);

                            if (m == null)
                            {
                                Console.WriteLine("Maintenance not found.");
                                break;
                            }

                            Console.WriteLine($"Car: {m.CarId}\nStart: {m.StartDate}\nEnd: {m.EndDate}\nStatus: {m.Status}\nDescription: {m.Description}");
                        }
                        break;

                    case 23:
                        {
                            Console.Write("Enter maintenance ID: ");
                            int mid = int.Parse(Console.ReadLine());

                            MaintenanceService msUpd = new MaintenanceService();
                            var existing = msUpd.GetById(mid);

                            if (existing == null)
                            {
                                Console.WriteLine("Maintenance ID not found");
                                break;
                            }

                            Console.WriteLine("Enter new END date (yyyy-MM-dd) or leave empty:");
                            string eIn = Console.ReadLine();
                            DateTime? newEnd = null;
                            if (!string.IsNullOrWhiteSpace(eIn))
                                newEnd = DateTime.Parse(eIn);

                            Console.WriteLine("Enter new DESCRIPTION or leave empty:");
                            string newDesc = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newDesc))
                                newDesc = null;

                            Maintenance upd = new Maintenance
                            {
                                Id = mid,
                                EndDate = newEnd,
                                Description = newDesc
                            };

                            bool ok = msUpd.UpdateMaintenance(upd);

                            Console.WriteLine(ok ? "Updated successfully!" : "Update failed.");
                        }
                        break;

                    case 24:
                        MaintenanceService maintenanceTestEndAuto = new MaintenanceService();
                        maintenanceTestEndAuto.RunMaintenanceAutoFinish();
                        Console.WriteLine("AutoFinished");
                        break;

                    case 25:
                        {
                            Console.WriteLine("\n--- AUTO CHECK MAINTENANCE FOR ALL CARS ---");

                            List<Car> cars = carService.GetAllCars();

                            foreach (Car car in cars)
                            {
                                var result = maintSrv.AutoCheckAndSend(car.Id);

                                Console.WriteLine(
                                  "Message: " + result.Message + "/n"+
                                  "AutoSent: " + result.AutoSent + "/n"+
                                  "Warning: " + result.Warning + "/n/n"

                                );
                                //Console.WriteLine(
                                //    $"Car #{car.Id} | {car.Brand} {car.Model}\n" +
                                //    $" → AutoSent: {result.AutoSent}\n" +
                                //    $" → Warning : {result.Warning}\n" +
                                //    $" → Message : {result.Message}\n"
                                //);
                            }

                            Console.WriteLine("------ END AUTO CHECK ------");
                        }
                        break;


                }

            } while (choice != 0);

        }


        // Request:
        //static void Main()
        //{
        //    RequestService requestService = new RequestService();
        //    int choice;

        //    do
        //    {
        //        Console.WriteLine("\n===== REQUEST MENU =====");
        //        Console.WriteLine("1. Admin: Show all requests");
        //        Console.WriteLine("2. Admin: Show request by ID");
        //        Console.WriteLine("3. Admin: Filter request by status PENDING");
        //        Console.WriteLine("33. Admin: Filter request by status ACCEPTED");
        //        Console.WriteLine("333. Admin: Filter request by status DENIED");
        //        Console.WriteLine("4. Admin: Accepte request");
        //        Console.WriteLine("5. Admin: Denied/Refuse request");

        //        Console.WriteLine("6. Client: Send new request");
        //        Console.WriteLine("7. Client: show client request");
        //        Console.WriteLine("8. Client: Delete request");
        //        Console.WriteLine("9. Client: Edit request");

        //        Console.WriteLine("10. Car: show car request");
        //        Console.WriteLine("0. Quit");
        //        Console.Write("Choice: ");

        //        choice = int.Parse(Console.ReadLine());

        //        switch (choice)
        //        {
        //            case 1:
        //                foreach (Request r in requestService.GetAllRequests())
        //                    Console.WriteLine($"{r.Id} - Client:{r.ClientId} Car:{r.CarId} | {r.StartDate:yyyy-MM-dd} to {r.EndDate:yyyy-MM-dd} | {r.Status}");
        //                break;

        //            case 2:
        //                Console.Write("Enter Request ID: ");
        //                int id = int.Parse(Console.ReadLine());

        //                Request req = requestService.GetRequestById(id);
        //                if (req != null)
        //                    Console.WriteLine($"{req.Id} - Client:{req.ClientId} Car:{req.CarId}| Message:{req.Message ?? "No Message"} | {req.StartDate:yyyy-MM-dd} to {req.EndDate:yyyy-MM-dd} | {req.Status}");
        //                else
        //                    Console.WriteLine("Request not found.");
        //                break;

        //            case 3:
        //                foreach (Request r in requestService.GetPendingRequests())
        //                    Console.WriteLine($"{r.Id} - Client:{r.ClientId} Car:{r.CarId} | PENDING");
        //                break;

        //            case 33:
        //                foreach (Request r in requestService.GetACCEPTEDRequests())
        //                    Console.WriteLine($"{r.Id} - Client:{r.ClientId} Car:{r.CarId} | {r.Status}");
        //                break;

        //            case 333:
        //                foreach (Request r in requestService.GetDENIEDRequests())
        //                    Console.WriteLine($"{r.Id} - Client:{r.ClientId} Car:{r.CarId} | {r.Status}");
        //                break;


        //            case 4:
        //                Console.Write("Enter Request ID: ");
        //                int idA = int.Parse(Console.ReadLine());
        //                bool acceptedOk = requestService.AcceptRequest(idA);
        //                Console.WriteLine($"Accepted Request {acceptedOk}");
        //                break;

        //            case 5:
        //                Console.Write("Enter Request ID: ");
        //                int idD = int.Parse(Console.ReadLine());
        //                bool deniedOk = requestService.RefuseRequest(idD);
        //                Console.WriteLine($"Denied Request {deniedOk}");
        //                break;

        //            case 6:
        //                Console.Write("Client ID: ");
        //                int client = int.Parse(Console.ReadLine());

        //                Console.Write("Car ID: ");
        //                int car = int.Parse(Console.ReadLine());

        //                Console.Write("Start Date (yyyy-mm-dd): ");
        //                DateTime start = DateTime.Parse(Console.ReadLine());

        //                Console.Write("End Date (yyyy-mm-dd): ");
        //                DateTime end = DateTime.Parse(Console.ReadLine());

        //                Console.Write("Message (optional): ");
        //                string message = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(message)) message = null;

        //                Request newReq = new Request
        //                {
        //                    ClientId = client,
        //                    CarId = car,
        //                    StartDate = start,
        //                    EndDate = end,
        //                    Status = "Pending",
        //                    Message = message,
        //                    CreatedAt = DateTime.Now
        //                };

        //                Console.WriteLine("Added: " + requestService.AddRequest(newReq));
        //                break;

        //            case 7:
        //                Console.Write("Client ID: ");
        //                int cId = int.Parse(Console.ReadLine());

        //                foreach (Request r in requestService.GetRequestsByClient(cId))
        //                    Console.WriteLine($"{r.Id} - Car:{r.CarId} | {r.Status}");
        //                break;



        //            case 8:
        //                Console.Write("Enter request ID to delete: ");
        //                int delId = int.Parse(Console.ReadLine());
        //                Console.WriteLine("Deleted: " + requestService.DeleteRequest(delId));
        //                break;

        //            case 9:
        //                Console.Write("Enter ID of request to update: ");
        //                int updateId = int.Parse(Console.ReadLine());
        //                Request reqU = requestService.GetRequestById(updateId);

        //                Console.Write("New Start Date: ");
        //                string uStartInput = Console.ReadLine();
        //                DateTime uStart = string.IsNullOrWhiteSpace(uStartInput) ? reqU.StartDate : DateTime.Parse(uStartInput);

        //                Console.Write("New End Date: ");
        //                string uEndInput = Console.ReadLine();
        //                DateTime uEnd = string.IsNullOrWhiteSpace(uEndInput) ? reqU.EndDate : DateTime.Parse(uEndInput);


        //                //Console.Write("New Status (Pending/Accepted/Denied): ");
        //                //string uStatus = Console.ReadLine();
        //                string uStatus = reqU.Status;

        //                Console.Write("New Message: ");
        //                string uMessageInput = Console.ReadLine();
        //                string uMessage = string.IsNullOrWhiteSpace(uMessageInput) ? reqU.Message : uMessageInput;

        //                Request upReq = new Request
        //                {
        //                    Id = updateId,
        //                    StartDate = uStart,
        //                    EndDate = uEnd,
        //                    Status = uStatus,
        //                    Message = uMessage
        //                };

        //                Console.WriteLine("Updated: " + requestService.UpdateRequest(upReq));
        //                break;

        //            case 10:
        //                Console.Write("Car ID: ");
        //                int caId = int.Parse(Console.ReadLine());

        //                foreach (Request r in requestService.GetRequestsByCar(caId))
        //                    Console.WriteLine($"{r.Id} - Client:{r.ClientId} | {r.Status}");
        //                break;
        //        }

        //    } while (choice != 0);
        //}

        // Reservation
        //static void Main()
        //{
        //    ReservationService reservationService = new ReservationService();
        //    RequestService requestService = new RequestService();
        //    int choice;

        //    do
        //    {
        //        Console.WriteLine("\n===== RESERVATION MENU =====");
        //        Console.WriteLine("1. Admin: Show all reservations");
        //        Console.WriteLine("2. Admin(Maybe Client): Show reservation by ID");
        //        Console.WriteLine("3. Show ACTIVE reservations");
        //        Console.WriteLine("4. Show FINISHED reservations");
        //        Console.WriteLine("5. Show CANCELLED reservations");
        //        //Console.WriteLine("6. Accept request (create reservation)");
        //        //Console.WriteLine("7. Refuse request");
        //        //Console.WriteLine("8. Pay reservation");
        //        Console.WriteLine("9. Finish reservation");
        //        Console.WriteLine("10. Cancel reservation");
        //        Console.WriteLine("11. Show reservations by User ID");
        //        Console.WriteLine("12. Show Active reservations by User ID");
        //        Console.WriteLine("13. Show Finished reservations by User ID");
        //        Console.WriteLine("14. Show Cancelled reservations by User ID");
        //        Console.WriteLine("15. Auto End Reservations if date end passed");

        //        Console.WriteLine("0. Quit");
        //        Console.Write("Choice: ");

        //        choice = int.Parse(Console.ReadLine());
        //        Console.WriteLine();

        //        switch (choice)
        //        {
        //            case 1:
        //                foreach (Reservation r in reservationService.GetAllReservations())
        //                    Console.WriteLine(
        //                        $"{r.Id} | Car:{r.CarId} User:{r.UserId} | " +
        //                        $"{r.StartDate:yyyy-MM-dd} → {r.EndDate:yyyy-MM-dd} | " +
        //                        $"${r.TotalPrice} | {r.Status} | Pay:{r.PaymentStatus}"
        //                    );
        //                break;

        //            case 2:
        //                Console.Write("Enter reservation ID: ");
        //                int id = int.Parse(Console.ReadLine());

        //                Reservation res = reservationService.GetReservationById(id);
        //                if (res == null)
        //                    Console.WriteLine("Reservation not found.");
        //                else
        //                    Console.WriteLine(
        //                        $"{res.Id} | Car:{res.CarId} User:{res.UserId} | " +
        //                        $"{res.StartDate:yyyy-MM-dd} → {res.EndDate:yyyy-MM-dd} | " +
        //                        $"${res.TotalPrice} | {res.Status} | Pay:{res.PaymentStatus}"
        //                    );
        //                break;

        //            case 3:
        //                foreach (Reservation r in reservationService.GetActiveReservations())
        //                    Console.WriteLine($"{r.Id} - {r.Status} - Car:{r.CarId}");
        //                break;

        //            case 4:
        //                foreach (Reservation r in reservationService.GetFinishedReservations())
        //                    Console.WriteLine($"{r.Id} - {r.Status} - Car:{r.CarId}");
        //                break;

        //            case 5:
        //                foreach (Reservation r in reservationService.GetCancelledReservations())
        //                    Console.WriteLine($"{r.Id} - {r.Status} - Car:{r.CarId}");
        //                break;

        //            //case 6:
        //            //    Console.Write("Enter Request ID to accept: ");
        //            //    int reqIdA = int.Parse(Console.ReadLine());

        //            //    bool accepted = reservationService.AcceptRequest(reqIdA);
        //            //    Console.WriteLine("Accepted: " + accepted);
        //            //    break;

        //            //case 7:
        //            //    Console.Write("Enter Request ID to refuse: ");
        //            //    int reqIdR = int.Parse(Console.ReadLine());

        //            //    bool refused = reservationService.RefuseRequest(reqIdR);
        //            //    Console.WriteLine("Refused: " + refused);
        //            //    break;

        //            //case 8:
        //            //    Console.Write("Enter Reservation ID to pay: ");
        //            //    int resPay = int.Parse(Console.ReadLine());

        //            //    bool paid = reservationService.PayReservation(resPay);
        //            //    Console.WriteLine("Paid: " + paid);
        //            //    break;

        //            case 9:
        //                Console.Write("Enter Reservation ID to finish: ");
        //                int resIdF = int.Parse(Console.ReadLine());

        //                bool finished = reservationService.FinishReservation(resIdF);
        //                Console.WriteLine("Finished: " + finished);
        //                break;

        //            case 10:
        //                Console.Write("Enter Reservation ID to cancel: ");
        //                int resIdC = int.Parse(Console.ReadLine());

        //                bool cancelled = reservationService.CancelReservation(resIdC);
        //                Console.WriteLine("Cancelled: " + cancelled);
        //                break;


        //            case 11:
        //                Console.Write("Enter User ID: ");
        //                int userId = int.Parse(Console.ReadLine());

        //                foreach (Reservation r in reservationService.GetReservationsByUser(userId))
        //                    Console.WriteLine($"{r.Id} - {r.Status} - {r.PaymentStatus} - {r.StartDate:yyyy-MM-dd}");
        //                break;

        //            case 12:
        //                Console.Write("Enter User ID, to show active Reservation of this client: ");
        //                int userIdA = int.Parse(Console.ReadLine());

        //                foreach (Reservation r in reservationService.GetActiveClientReservations(userIdA))
        //                    Console.WriteLine($"{r.Id} - {r.Status} - {r.PaymentStatus} - {r.StartDate:yyyy-MM-dd}");
        //                break;

        //            case 13:
        //                Console.Write("Enter User ID, to show finished Reservation of this client: ");
        //                int userIdF = int.Parse(Console.ReadLine());

        //                foreach (Reservation r in reservationService.GetFinishedClientReservations(userIdF))
        //                    Console.WriteLine($"{r.Id} - {r.Status} - {r.PaymentStatus} - {r.StartDate:yyyy-MM-dd}");
        //                break;

        //            case 14:
        //                Console.Write("Enter User ID, to show cacelled Reservation of this client: ");
        //                int userIdC = int.Parse(Console.ReadLine());

        //                foreach (Reservation r in reservationService.GetCancelledClientReservations(userIdC))
        //                    Console.WriteLine($"{r.Id} - {r.Status} - {r.PaymentStatus} - {r.StartDate:yyyy-MM-dd}");
        //                break;

        //            case 15:
        //                reservationService.RunReservationAutoFinish();
        //                break;

        //        }

        //    } while (choice != 0);
        //}

        // Image Cars
        //public static void Main(string[] args)
        //{
        //    CarImageService imageService = new CarImageService();

        //    while (true)
        //    {
        //        Console.WriteLine("\n=== CAR IMAGE MANAGER ===");
        //        Console.WriteLine("1 - Add Image to Car");
        //        Console.WriteLine("2 - List Car Images");
        //        Console.WriteLine("3 - Delete Image");
        //        Console.WriteLine("4 - Delete All Images of Car");
        //        Console.WriteLine("0 - Exit");

        //        Console.Write("Choice: ");
        //        string choice = Console.ReadLine();

        //        switch (choice)
        //        {
        //            case "1":
        //                Console.Write("Enter Car ID: ");
        //                int carId = int.Parse(Console.ReadLine());

        //                Console.Write("Enter image path: ");
        //                string path = Console.ReadLine();

        //                bool added = imageService.AddImage(carId, path);

        //                Console.WriteLine(added ? "Image added!" : "Error adding image.");
        //                break;

        //            case "2":
        //                Console.Write("Enter Car ID: ");
        //                carId = int.Parse(Console.ReadLine());

        //                var images = imageService.GetImagesByCarId(carId);

        //                foreach (var img in images)
        //                    Console.WriteLine($"[{img.Id}] {img.ImagePath}");

        //                break;

        //            case "3":
        //                Console.Write("Enter Image ID to delete: ");
        //                int imgId = int.Parse(Console.ReadLine());

        //                bool deleted = imageService.DeleteOneImageById(imgId);
        //                Console.WriteLine(deleted ? "Deleted!" : "Error deleting.");
        //                break;

        //            case "4":
        //                Console.Write("Enter Car ID: ");
        //                carId = int.Parse(Console.ReadLine());

        //                bool delAll = imageService.DeleteAllImagesOfCar(carId);
        //                Console.WriteLine(delAll ? "ALL images deleted!" : "Error deleting images.");
        //                break;

        //            case "0":
        //                return;
        //        }
        //    }
        //}
    }
}
