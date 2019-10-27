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

        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool IsAvailable(DateTime fromDate, DateTime toDate, int siteId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand($"SELECT * " +
                    $"FROM reservation r " +
                    $"JOIN site s ON s.site_id = r.site_id " +
                    $"JOIN campground cg ON cg.campground_id = s.campground_id " +
                    $"WHERE s.site_id = @siteID"
                    , connection);
                cmd.Parameters.AddWithValue("@siteID", siteId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Reservation> reservations = new List<Reservation>();
                while (reader.Read())
                {
                    Reservation reservation = new Reservation();
                    reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                    reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                    reservations.Add(reservation);

                    foreach (Reservation checkReservation in reservations)
                    {
                        if (fromDate >= checkReservation.FromDate && fromDate <= checkReservation.ToDate && toDate <= checkReservation.ToDate && toDate >= checkReservation.FromDate)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                return true;
            }
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
