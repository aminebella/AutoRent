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

        // 🔹 Get all cars
        public List<Car> GetAllCars() => carDao.GetAllCars();
        // 🔹 Get car by Id
        public Car GetCarById(int id) => carDao.GetCarById(id);
        // 🔹 Add new car
        public bool AddCar(Car car) => carDao.AddCar(car);
        // 🔹 Update car
        public bool UpdateCar(Car car) => carDao.UpdateCar(car);
        // 🔹 Delete car
        public bool DeleteCar(int id) => carDao.DeleteCar(id);

        // Filters
        public List<Car> SearchCarsByModel(string model) => carDao.GetCarsByModel(model);
        public List<Car> SearchCarsByBrand(string brand) => carDao.GetCarsByBrand(brand);
        public List<Car> SearchCarsByStatus(string status) => carDao.GetCarsByStatus(status);
        public List<Car> SearchCarsByCategory(string category) => carDao.GetCarsByCategory(category);
        public List<Car> SearchCarsByColor(string color) => carDao.GetCarsByColor(color);

    }
}
