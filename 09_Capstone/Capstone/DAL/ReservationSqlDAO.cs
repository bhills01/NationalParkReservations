using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        // DO WE NEED THIS?!?!

        //public IList<Reservation> Search(int siteID)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        // Apoen the connection to SQL Server
        //        connection.Open();

        //        // Create Command object to execute queary to get all cities
        //        SqlCommand cmd = new SqlCommand($"SELECT * FROM reservation WHERE site_id = '{siteID}'", connection);

        //        // Execute the command to get a result set. read by the SQLReader
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // Loop through the rows, and print data to the screen
        //        List<Reservation> reservations = new List<Reservation>();
        //        while (reader.Read())
        //        {
        //            Reservation reservation = new Reservation();
        //            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
        //            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
        //            reservation.Name = Convert.ToString(reader["name"]);
        //            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
        //            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
        //            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);
        //            reservations.Add(reservation);
        //        }
        //        return reservations;
        //    }
        //}

        public bool IsAvailable(DateTime fromDate, DateTime toDate, int siteID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Apoen the connection to SQL Server
                connection.Open();

                // Create Command object to execute queary to get all cities
                SqlCommand cmd = new SqlCommand($"SELECT * FROM reservation WHERE site_id = '{siteID}'", connection);

                // Execute the command to get a result set. read by the SQLReader
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through the rows, and print data to the screen
                List<Reservation> reservations = new List<Reservation>();
                while (reader.Read())
                {
                    Reservation reservation = new Reservation();
                    reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                    reservation.SiteId = Convert.ToInt32(reader["site_id"]);
                    reservation.Name = Convert.ToString(reader["name"]);
                    reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                    reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                    reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);
                    reservations.Add(reservation);
                }

                foreach (Reservation reservation in reservations)
                {
                    
                    if (fromDate >= reservation.FromDate && fromDate <= reservation.ToDate && toDate <= reservation.ToDate && toDate >= reservation.FromDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool MakeReservation(DateTime fromDate, DateTime toDate, string name, int siteID)
        {
            bool areDatesAvailable = IsAvailable(fromDate, toDate, siteID);
            
            if (areDatesAvailable)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    DateTime currentDate = DateTime.Now;

                    connection.Open();


                    SqlCommand cmd = new SqlCommand($"INSERT reservation (site_id, name, from_date, to_date, create_date) VALUES (@siteID, @name, @fromDate, @toDate, @currentDate)", connection);
                    cmd.Parameters.AddWithValue("@siteID", siteID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@currentDate", currentDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    return true;
                }
            }
            return false;
        }
    }
}
