using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

//Biblio

// Imports:
using CarRentalApp.Backend.Database;
using CarRentalApp.Backend.Models;

namespace CarRentalApp.Backend.Services

{
    internal class ReservationService
    {
        private readonly ReservationDao reservationDao;
        private readonly RequestDao requestDao;
        private readonly CarDao carDao;

        public ReservationService()
        {
            reservationDao = new ReservationDao();
            requestDao = new RequestDao();
            carDao = new CarDao();
        }

        


        // Admin
        // GET ALL Reservation :
        public List<Reservation> GetAllReservations() => reservationDao.GetAllReservations();


        // GET Reservation BY Status == ACTIVE:
        public List<Reservation> GetActiveReservations() => reservationDao.GetReservationsByStatus("ACTIVE");


        // GET Reservation BY Status == FINISHED:
        public List<Reservation> GetFinishedReservations()=> reservationDao.GetReservationsByStatus("FINISHED");


        // GET Reservation BY Status == CANCELLED:
        public List<Reservation> GetCancelledReservations()=> reservationDao.GetReservationsByStatus("CANCELLED");


        // GET Reservation BY ID :
        public Reservation GetReservationById(int id) => reservationDao.GetReservationById(id);


        // Accepte Reservation (ACTIVE)
        // 1.Get request from RequestDao
        // 2.Check request still pending
        // 3.Check availability/Check if car is available -> Check if car is booked during this period before reservation
        // 4.Calculate total price -> calculate total price of the reservation :
        // 5.Create Reservation -> ADD Reservation :
        // 6.Update request status to ACCEPTED
        // 7.Update car status to RESERVED
        // 8.Return true
        //public bool AccepteRequest(int requestId)//Request req
        //{
        //}


        // Refuse/Deny Reservation
        // 1.Get request
        // 2.Update status = REFUSED
        //public bool RefuseRequest(int requestId)
        //{

        //}


        // Finish Reservation (FINISHED)
        // UPDATE Reservation Status == FINISHED
        // 1.Get reservation
        // 2.Update reservation status -> FINISHED
        // 3.Update car status -> AVAILABLE
        public bool FinishReservation(int reservationId)
        {
            Reservation r = reservationDao.GetReservationById(reservationId);
            if (r == null || r.Status != "ACTIVE")
                return false;

            // 1. Update reservation status
            bool ok = reservationDao.UpdateReservationStatus(reservationId, "FINISHED");
            if (!ok) return false;

            // 2. Make the car available again
            carDao.UpdateCarStatus(r.CarId, "AVAILABLE");

            return true;
        }


        // Cancel Reservation (CANCELLED)
        // UPDATE Reservation Status == CANCELLED
        // 1.Get reservation
        // 2.Update status → CANCELLED
        // 3.Update car status → AVAILABLE
        public bool CancelReservation(int reservationId)
        {
            Reservation r = reservationDao.GetReservationById(reservationId);
            if (r == null || r.Status != "ACTIVE")
                return false;

            // 1. Update reservation status
            bool ok = reservationDao.UpdateReservationStatus(reservationId, "CANCELLED");
            if (!ok) return false;

            // 2. Make the car available
            carDao.UpdateCarStatus(r.CarId, "AVAILABLE");

            return true;
        }


        // AVAILABILITY  ????
        //public bool IsCarAvailable(int carId, DateTime start, DateTime end) {
        //    return !reservationDao.IsCarBookedForPeriod(carId, start, end);
        //}
        // PRICE  ???
        //public decimal CalculatePrice(int carId, DateTime start, DateTime end) {
        //    return reservationDao.CalculatePrice(carId, start, end);
        //}


        // Client
        // GET Reservation BY User ID :
        public List<Reservation> GetReservationsByUser(int userId) => reservationDao.GetReservationsByUser(userId);

        // GET Reservation BY User ID And Status == ACTIVE:
        public List<Reservation> GetActiveClientReservations(int userId)=> reservationDao.GetReservationsByUserAndStatus(userId,"ACTIVE");


        // GET Reservation BY User ID And Status == FINISHED:
        public List<Reservation> GetFinishedClientReservations(int userId)=> reservationDao.GetReservationsByUserAndStatus(userId,"FINISHED");


        // GET Reservation BY User ID And Status == CANCELLED:
        public List<Reservation> GetCancelledClientReservations(int userId)=> reservationDao.GetReservationsByUserAndStatus(userId,"CANCELLED");


        // Pay Reservation:
        // 1.Get reservation
        // 2.Update payment_status -> PAID : // UPDATE Reservation PayementStatus(Paid) :
        // 3.Generate receipt PDF
        //public bool PayReservation(int reservationId) {
        //    Reservation r = reservationDao.GetReservationById(reservationId);
        //    if (r == null || r.PaymentStatus == "PAID")
        //        return false;

        //    // Update payment
        //    bool ok = reservationDao.UpdatePaymentStatus(reservationId, "PAID");
        //    if (!ok) return false;

        //    // TODO: generate PDF receipt (optional)

        //    return true;
        //}




    }
}