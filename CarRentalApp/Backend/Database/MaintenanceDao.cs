using backendclienttesting.Backend.Helper;
using backendclienttesting.Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Database
{
    public class MaintenanceDao
    {

        // Get All Active Maintenance
        public List<Maintenance> GetAllActive()
        {
            List<Maintenance> list = new List<Maintenance>();
            string q = "SELECT * FROM maintenances WHERE status = 'ACTIVE' ORDER BY start_date DESC";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                var r = cmd.ExecuteReader();

                while (r.Read())
                    list.Add(HelperMaintenance.MapMaintenance(r));
            }
            return list;
        }

        // Get Finished Maintenance
        public List<Maintenance> GetAllFinished()
        {
            List<Maintenance> list = new List<Maintenance>();
            string q = "SELECT * FROM maintenances WHERE status = 'FINISHED' ORDER BY end_date DESC";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                var r = cmd.ExecuteReader();

                while (r.Read())
                    list.Add(HelperMaintenance.MapMaintenance(r));
            }

            return list;
        }

        // Show: Get Maintenance By ID
        public Maintenance GetById(int id)
        {
            string q = "SELECT * FROM maintenances WHERE id = @id LIMIT 1";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@id", id);

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return HelperMaintenance.MapMaintenance(r);
                }
            }
            return null;
        }

        // Add: Add new maintenance
        public bool StartMaintenance(Maintenance maintenanceA)
        {
            string q = @"
                INSERT INTO maintenances(car_id, start_date, end_date, description)
                VALUES(
                    @car_id,
                    COALESCE(@start_date, CURRENT_DATE()),
                    COALESCE(@end_date, DATE_ADD(COALESCE(@start_date, CURRENT_DATE()), INTERVAL 10 DAY)),
                    COALESCE(@desc, 'No Description')
            )";
            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@car_id", maintenanceA.CarId);
                // Start date: if null → DBNull.Value → SQL COALESCE uses CURRENT_DATE()
                cmd.Parameters.AddWithValue("@start_date", maintenanceA.StartDate.HasValue
                    ? (object)maintenanceA.StartDate.Value
                    : DBNull.Value);

                // End date: if null → DBNull.Value → SQL COALESCE uses start_date + 10 days
                cmd.Parameters.AddWithValue("@end_date", maintenanceA.EndDate.HasValue
                    ? (object)maintenanceA.EndDate.Value
                    : DBNull.Value);

                // Description: if null or empty → DBNull.Value → SQL COALESCE uses 'No Description'
                cmd.Parameters.AddWithValue("@desc", string.IsNullOrWhiteSpace(maintenanceA.Description)
                    ? (object)DBNull.Value
                    : maintenanceA.Description);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update Status: Finish maintenance
        public bool FinishMaintenance(int maintenanceId)
        {
            string q = @"UPDATE maintenances 
                     SET end_date = CURRENT_DATE(), status='FINISHED' 
                     WHERE id=@id";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@id", maintenanceId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update: Update Maintenance
        public bool UpdateMaintenance(Maintenance maint)
        {
            string q = @"
        UPDATE maintenances
        SET 
            end_date = COALESCE(@end_date, DATE_ADD(start_date, INTERVAL 10 DAY)),
            description = COALESCE(@desc, 'No Description')
        WHERE id = @id";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                using (var cmd = new MySqlCommand(q, c))
                {
                    cmd.Parameters.AddWithValue("@id", maint.Id);

                    // EndDate: if null → DBNull.Value → SQL uses start_date + 10
                    cmd.Parameters.AddWithValue("@end_date",
                        maint.EndDate.HasValue ? (object)maint.EndDate.Value : DBNull.Value);

                    // Description: if null/empty → DBNull.Value → SQL uses "No Description"
                    cmd.Parameters.AddWithValue("@desc",
                        string.IsNullOrWhiteSpace(maint.Description) ? (object)DBNull.Value : maint.Description);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Show Maintenance Car: List of maintenance of a car
        public List<Maintenance> GetMaintByCar(int carId)
        {
            List<Maintenance> result = new List<Maintenance>();
            string q = "SELECT * FROM maintenances WHERE car_id=@id ORDER BY start_date DESC";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@id", carId);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    result.Add(HelperMaintenance.MapMaintenance(reader));
            }
            return result;
        }

        // Show Active Maintenance Car: Get active maintenance for car
        public Maintenance GetActive(int carId)
        {
            string q = "SELECT * FROM maintenances WHERE car_id=@id AND status='ACTIVE' LIMIT 1";

            using (var c = DbConnection.GetConnection())
            {
                c.Open();
                var cmd = new MySqlCommand(q, c);
                cmd.Parameters.AddWithValue("@id", carId);
                var r = cmd.ExecuteReader();

                if (r.Read())
                    return HelperMaintenance.MapMaintenance(r);
            }
            return null;
        }


        // Create Transaction to end maintenance automaticly after enddate
        public void AutoFinishExpiredMaintenances()
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    // 1. Finish all maintenances whose end_date has passed
                    var finishQuery = @"
                        UPDATE maintenances
                        SET status = 'FINISHED'
                        WHERE status = 'ACTIVE'
                        AND end_date <= NOW();
                    ";

                    using (var cmd = new MySqlCommand(finishQuery, conn, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Set all related cars to AVAILABLE
                    var carQuery = @"
                        UPDATE cars
                        SET status = 'AVAILABLE'
                        WHERE id IN (
                            SELECT car_id FROM maintenances
                            WHERE status = 'FINISHED'
                            AND end_date <= NOW()
                        );
                    ";

                    using (var cmd = new MySqlCommand(carQuery, conn, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }


    }
}
