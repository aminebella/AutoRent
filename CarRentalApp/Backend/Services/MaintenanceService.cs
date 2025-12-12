using backendclienttesting.Backend.Models;
using CarRentalApp.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Services
{
    public class MaintenanceService
    {
        private readonly MaintenanceDao maintenanceDao;
        private readonly CarDao carDao;

        public MaintenanceService()
        {
            maintenanceDao = new MaintenanceDao();
            carDao = new CarDao();
        }

        // Get all maintenance
        public List<Maintenance> GetAllMaintenance()
        {
            // Merge active + finished
            List<Maintenance> list = new List<Maintenance>();
            list.AddRange(maintenanceDao.GetAllActive());
            list.AddRange(maintenanceDao.GetAllFinished());
            return list;
        }
        // Filter
        // Get all active Maintenance
        public List<Maintenance> GetActiveAll() => maintenanceDao.GetAllActive();

        // Get all finished
        public List<Maintenance> GetFinishedAll() => maintenanceDao.GetAllFinished();

        // Show: Get Maintenance By ID
        public Maintenance GetById(int id) => maintenanceDao.GetById(id);

        // Update: Update Maintenance
        public bool UpdateMaintenance(Maintenance m) => maintenanceDao.UpdateMaintenance(m);

        // Add: Send car to maintenance
        public bool SendToMaintenance(Maintenance maintenance)
        {
            var active = maintenanceDao.GetActive(maintenance.CarId);
            if (active != null)
                return false;  // Already under maintenance
            bool carAvailable = carDao.IfAvailable(maintenance.CarId);
            if (!carAvailable)
                return false; // Car is not available

            bool ok = maintenanceDao.StartMaintenance(maintenance);

            if (!ok) return false; // not added

            return carDao.MarkMaintenance(maintenance.CarId); // mark status of car as maintenane
        }

        // Update Status: Finish maintenance
        public bool FinishMaintenance(int carId)
        {
            var m = maintenanceDao.GetActive(carId);
            if (m == null) return false; // not in maintenance so can't finish it

            bool ok = maintenanceDao.FinishMaintenance(m.Id);
            if (!ok) return false; // error in finishing

            carDao.MarkAvailable(carId); // make car available again

            carDao.UpdateLastMaintenance(carId); // new method : change the last maintenance date

            return true;
        }

        // end maintenance automaticly after arrive of its enddate
        public void RunMaintenanceAutoFinish() => maintenanceDao.AutoFinishExpiredMaintenances();


        // Automatic check if car needs maintenance, or the date of start maintenace is close (give warning)
        //public bool AutoCheckAndSend(int carId)
        //{
        //    var car = carDao.GetCarById(carId);

        //    if (car.LastMaintenanceDate == null) return false;

        //    //int days = (DateTime.Now - car.LastMaintenanceDate.Value).Days;
        //    int days = (DateTime.Now - car.LastMaintenanceDate).Days;


        //    if (days >= car.MaintenanceIntervalDays)
        //    {
        //        Maintenance m = new Maintenance
        //        {
        //            CarId = carId,
        //            StartDate = null,   // Let MySQL choose CURRENT_DATE
        //            EndDate = null,     // Let MySQL choose CURRENT_DATE + 10 days
        //            Description = "Automatic scheduled maintenance",
        //            Status = null
        //        };
        //        return SendToMaintenance(m);
        //    }

        //    return false;
        //}
        //public MaintenanceCheckResult AutoCheckAndSend(int carId)
        //{
        //    var car = carDao.GetCarById(carId);

        //    if (car == null || car.LastMaintenanceDate == null)
        //        return new MaintenanceCheckResult { AutoSent = false, Warning = false, Message = "Car not found or missing last maintenance date." };

        //    DateTime last = car.LastMaintenanceDate.Value;
        //    int daysPassed = (DateTime.Now - last).Days;

        //    int interval = car.MaintenanceIntervalDays;     // Example: every 60 days
        //    int daysLeft = interval - daysPassed;

        //    // CASE A — Warning 10 days before maintenance due
        //    if (daysLeft > 0 && daysLeft <= 10)
        //    {
        //        return new MaintenanceCheckResult
        //        {
        //            AutoSent = false,
        //            Warning = true,
        //            Message = $"Warning: {daysLeft} days left before car maintenance is due."
        //        };
        //    }

        //    // CASE B — Maintenance date has arrived (daysLeft == 0)
        //    if (daysLeft == 0)
        //    {
        //        // Check if within 24h manual window
        //        if ((DateTime.Now - last.AddDays(interval)).TotalHours <= 24)
        //        {
        //            return new MaintenanceCheckResult
        //            {
        //                AutoSent = false,
        //                Warning = true,
        //                Message = "Maintenance date has arrived. Admin has 24 hours to send car manually."
        //            };
        //        }
        //    }

        //    // CASE C — More than 24 hours late → AUTO SEND
        //    if (daysLeft < 0 && Math.Abs(daysLeft) >= 1)
        //    {
        //        Maintenance m = new Maintenance
        //        {
        //            CarId = carId,
        //            StartDate = DateTime.Now,
        //            EndDate = DateTime.Now.AddDays(10),
        //            Description = "Automatic scheduled maintenance",
        //            Status = "AUTO"
        //        };

        //        bool success = SendToMaintenance(m);

        //        return new MaintenanceCheckResult
        //        {
        //            AutoSent = success,
        //            Warning = false,
        //            Message = $"Car automatically sent to maintenance. (Start: {DateTime.Now:yyyy-MM-dd}, End: {DateTime.Now.AddDays(10):yyyy-MM-dd})"
        //        };
        //    }

        //    return new MaintenanceCheckResult { AutoSent = false, Warning = false, Message = "No action needed." };
        //}

        // Show Maintenance Car
        public List<Maintenance> GetHistory(int carId) => maintenanceDao.GetMaintByCar(carId);


        



    }

}
