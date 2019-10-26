using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> Search(int campgroundID, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Apoen the connection to SQL Server
                connection.Open();

                // Create Command object to execute queary to get all cities
                SqlCommand cmd = new SqlCommand($"SELECT * FROM site WHERE campground_id = '{campgroundID}'", connection);

                // Execute the command to get a result set. read by the SQLReader
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through the rows, and print data to the screen
                List<Site> sites = new List<Site>();
                while (reader.Read())
                {
                    Site site = new Site();
                    site.SiteId = Convert.ToInt32(reader["site_id"]);
                    site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                    site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                    site.MaxOccupants = Convert.ToInt32(reader["max_occupancy"]);
                    site.Accesible = Convert.ToBoolean(reader["accessible"]);
                    site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                    site.Utilities = Convert.ToBoolean(reader["utilities"]);
                    sites.Add(site);
                }
                return sites;
            }

        }
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

    }
}
