using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Dependencies
using MySql.Data.MySqlClient;

// Imports
using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Database;

namespace CarRentalApp.Backend.Services
{
    public class RequestService
    {
        private readonly RequestDao requestDao;
        private readonly ReservationDao reservationDao;
        private readonly CarDao carDao;

        public RequestService()
        {
            requestDao = new RequestDao();
            reservationDao = new ReservationDao();
            carDao = new CarDao();
        }

        
        // 🔹 Get Requests by Id
        public Request GetRequestById(int id) => requestDao.GetRequestById(id);
        
        
        
        // Filters
        // 🔹 Admin/Client Side :Get Requests by Client Id
        public List<Request> GetRequestsByClient(int clientId) => requestDao.GetRequestsByClient(clientId);
        
        


        // -------------------------
        // Client Side: Send , delete, update
        // -------------------------
        // 🔹 Client send Requests = Add new Requests
        // 🔹 Add new Requests
        public bool AddRequest(Request r) => requestDao.AddRequest(r);

        // 🔹 Client delete Requests =  Delete Requests
        // 🔹 Delete Requests
        public bool DeleteRequest(int id) => requestDao.DeleteRequest(id);

        // 🔹 Client Update Requests =  Update Requests
        // 🔹 Update Requests
        public bool UpdateRequest(Request r) => requestDao.UpdateRequest(r);


        // -------------------------
        // Admin Side: Accept request (or refuse) — orchestration
        // -------------------------

        // 🔹 Get all Requests
        public List<Request> GetAllRequests() => requestDao.GetAllRequests();

        

        // 🔹 Admin Side :Get Requests by Car Id
        public List<Request> GetRequestsByCar(int carId) => requestDao.GetRequestsByCar(carId);

        // 🔹 Get Requests by status pending
        public List<Request> GetPendingRequests() => requestDao.GetPendingRequests();


        public bool AcceptRequest(Request request)
        {
            // This is high-level pseudocode of the flow:
            // 1. Validate request still exists and is Pending
            // 2. Check car availability for dates (using _reservationDao)
            // 3. If available -> create reservation row via _reservationDao
            // 4. Update request.status = 'Accepted'
            // 5. Update car.status = 'reserved' or 'rented' depending on business rules
            // 6. Return true on success

            Request existing = requestDao.GetRequestById(request.Id);
            if (existing == null || existing.Status != "Pending") return false;

            // check availability (method on ReservationDao)
            bool isAvailable = !reservationDao.IsCarBookedForPeriod(request.CarId, request.StartDate, request.EndDate);
            if (!isAvailable) return false;

            // create reservation (total price calculation should be done here or earlier)
            var reservation = new Reservation
            {
                CarId = request.CarId,
                UserId = request.ClientId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalPrice = reservationDao.CalculatePrice(request.CarId, request.StartDate, request.EndDate),
                Status = "ACTIVE",
                PaymentStatus = "UNPAID"
            };

            bool created = reservationDao.AddReservation(reservation);
            if (!created) return false;

            // update request status
            existing.Status = "Accepted";
            requestDao.UpdateRequest(existing);

            // update car status
            Car car = carDao.GetCarById(request.CarId);
            if (car != null)
            {
                car.Status = "RESERVED"; // enum in db cars
                carDao.UpdateCar(car);
            }

            // TODO: trigger email + generate PDF
            //?
            return true;
        }

        public bool RefuseRequest(int requestId, string reason = null)
        {
            Request existing = requestDao.GetRequestById(requestId);
            if (existing == null) return false;
            existing.Status = "Refused";
            existing.Message = reason ?? existing.Message;
            return requestDao.UpdateRequest(existing);
        }
    }
}
