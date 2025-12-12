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

        // Requests
            // Admin Side: Managing + Orchestration
        // Get all Requests
        public List<Request> GetAllRequests() => requestDao.GetAllRequests();
        // Get Requests by Id
        public Request GetRequestById(int id) => requestDao.GetRequestById(id);
        // Filters
        // Get Requests by status pending
        public List<Request> GetPendingRequests() => requestDao.GetRequestsByStatus("PENDING");
        // Get Requests by status ACCEPTED
        public List<Request> GetACCEPTEDRequests() => requestDao.GetRequestsByStatus("ACCEPTED");
        // Get Requests by status DENIED
        public List<Request> GetDENIEDRequests() => requestDao.GetRequestsByStatus("DENIED");
        // Accepte Request
        public bool AcceptRequest(int requestId)
        {
            // 1.Get request from RequestDao
            // 2.Check request still exists and is Pending
            // 3.Check availability/Check if car is available -> Check if car is booked during this period before reservation (using reservationDao)
            // 4.Calculate total price -> calculate total price of the reservation :
            // 5.Create Reservation -> ADD Reservation (If car available during this time)
            // 6.Update request status to ACCEPTED
            // 7.Update car status to RESERVED
            // 8.Return true on success

            Request existing = requestDao.GetRequestById(requestId);
            if (existing == null || existing.Status != "PENDING") return false;

            // check availability (method on ReservationDao)
            bool isAvailable = !reservationDao.IsCarBookedForPeriod(existing.CarId, existing.StartDate, existing.EndDate);
            if (!isAvailable) return false;

            // create reservation (total price calculation should be done here or earlier)
            var reservation = new Reservation
            {
                CarId = existing.CarId,
                UserId = existing.ClientId,
                StartDate = existing.StartDate,
                EndDate = existing.EndDate,
                TotalPrice = reservationDao.CalculatePrice(existing.CarId, existing.StartDate, existing.EndDate),
                Status = "ACTIVE",
                PaymentStatus = "UNPAID"
            };

            bool created = reservationDao.AddReservation(reservation);
            if (!created) return false;

            // update request status
            //existing.Status = "Accepted";
            //requestDao.UpdateRequest(existing);
            requestDao.UpdateRequestStatus(existing.Id, "ACCEPTED");

            // update car status
            carDao.MarkRESERVED(existing.CarId);

            // TODO: trigger email + generate PDF
            //?
            return true;
        }

        // Refuse Request
        public bool RefuseRequest(int requestId)
        {
            return requestDao.UpdateRequestStatus(requestId, "DENIED");
        }

        // -------------------------// -------------------------// -------------------------// -------------------------
        // Client Side : Send , delete, update
        // Client send Requests = Add new Requests
        public bool AddRequest(Request r) => requestDao.AddRequest(r);
        // Client Update Requests =  Update Requests
        public bool UpdateRequest(Request r) => requestDao.UpdateRequest(r);
        // Client delete Requests =  Delete Requests
        public bool DeleteRequest(int id) => requestDao.DeleteRequest(id);
        // -------------------------// -------------------------// -------------------------
        // -------------------------
        // Admin :Client
        // Get Requests by Client Id
        public List<Request> GetRequestsByClient(int clientId) => requestDao.GetRequestsByClient(clientId);
        // -------------------------
        // Admin :Cars
        // Get Requests by Car Id
        public List<Request> GetRequestsByCar(int carId) => requestDao.GetRequestsByCar(carId);

        
    }
}
