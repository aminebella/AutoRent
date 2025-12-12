using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRentalApp.Backend.Database;
using CarRentalApp.Backend.Models;

namespace CarRentalApp.Backend.Services
{
    public class CarService
    {
        private readonly CarDao carDao;

        public CarService()
        {
            carDao = new CarDao();
        }

        // Admin
        // Index: Get All Cars
        public List<Car> GetAllCars() => carDao.GetAllCars();
        // Show: Get Car By Id
        public Car GetCarById(int id) => carDao.GetCarById(id);
        // Add: Add New Car
        public bool AddCar(Car car) => carDao.AddCar(car);
        // Update: Update car
        public bool UpdateCar(Car newCar)
        {
            // Get the existing car from DB
            Car oldCar = carDao.GetCarById(newCar.Id);
            if (oldCar == null) return false;
            // Only replace fields that are explicitly entered
            newCar.Brand = string.IsNullOrWhiteSpace(newCar.Brand) ? oldCar.Brand : newCar.Brand;
            newCar.Model = string.IsNullOrWhiteSpace(newCar.Model) ? oldCar.Model : newCar.Model;
            newCar.Year = newCar.Year == 0 ? oldCar.Year : newCar.Year;
            newCar.Color = string.IsNullOrWhiteSpace(newCar.Color) ? oldCar.Color : newCar.Color;
            newCar.PricePerDay = newCar.PricePerDay == 0 ? oldCar.PricePerDay : newCar.PricePerDay;
            newCar.CategoryName = string.IsNullOrWhiteSpace(newCar.CategoryName) ? oldCar.CategoryName : newCar.CategoryName;
            newCar.LastMaintenanceDate = newCar.LastMaintenanceDate == default ? oldCar.LastMaintenanceDate : newCar.LastMaintenanceDate;
            newCar.MaintenanceIntervalDays = newCar.MaintenanceIntervalDays == 0 ? oldCar.MaintenanceIntervalDays : newCar.MaintenanceIntervalDays;
            return carDao.UpdateCar(newCar);
        }

        // Delete: Delete car
        //public bool DeleteCar(int id) => carDao.DeleteCar(id);

        // Filters
        public List<Car> SearchCarsByModel(string model) => carDao.GetCarsByModel(model);
        public List<Car> SearchCarsByBrand(string brand) => carDao.GetCarsByBrand(brand);
        public List<Car> SearchCarsByStatus(string status) => carDao.GetCarsByStatus(status);
        public List<Car> SearchCarsByCategory(string category) => carDao.GetCarsByCategory(category);
        public List<Car> SearchCarsByColor(string color) => carDao.GetCarsByColor(color);

        // search by year
        public List<Car> SearchCarsByYear(int year) => carDao.SearchByYear(year);

        // MarkAvailable
        public bool MarkAvailable(int id)
        {
            if (carDao.IfUnAvailable(id)) return carDao.UpdateCarStatus(id, "AVAILABLE");
            return false;
        }

        // MarkUnavailable
        public bool MarkUnavailable(int id)
        {
            if (carDao.IfAvailable(id)) return carDao.UpdateCarStatus(id, "UNAVAILABLE");
            return false;
        }


    }
}
