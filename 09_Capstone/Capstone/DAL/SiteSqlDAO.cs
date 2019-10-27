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
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
                                                SELECT TOP 5 *
                                                FROM site
                                                WHERE campground_id = @campgroundID AND site_id NOT IN (SELECT DISTINCT	site_id
                                                FROM reservation
                                                WHERE (@FROMDATE >= from_date AND @FROMDATE <= to_date) OR (@TODATE >= from_date AND @TODATE <= to_date) 
                                                OR (from_date >= @FROMDATE AND from_date <= @TODATE) OR (to_date >= @FROMDATE AND to_date <= @TODATE))"
                    , connection);
                cmd.Parameters.AddWithValue("@campgroundID", campgroundID);
                cmd.Parameters.AddWithValue("@FROMDATE", fromDate);
                cmd.Parameters.AddWithValue("@TODATE", toDate);
                SqlDataReader reader = cmd.ExecuteReader();

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



        public bool IsAvailable(DateTime userFromDate, DateTime userToDate, DateTime resFromDate, DateTime resToDate, int siteID)
        {
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
                //connection.Open();

                //SqlCommand cmd = new SqlCommand($"SELECT * " +
                //    $"FROM reservation r " +
                //    $"JOIN site s ON s.site_id = r.site_id " +
                //    $"JOIN campground cg ON cg.campground_id = s.campground_id " +
                //    $"WHERE s.site_id = @siteID"
                //    , connection);
                //cmd.Parameters.AddWithValue("@siteID", siteID);
                //SqlDataReader reader = cmd.ExecuteReader();

                //List<Reservation> reservations = new List<Reservation>();
                //while (reader.Read())
                //{
                //    Reservation reservation = new Reservation();
                //    reservation.SiteId = Convert.ToInt32(reader["site_id"]);
                //    reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                //    reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                //    foreach (Reservation reservation1 in reservation)
                //    {

            //    if (userFromDate >= resFromDate && userFromDate <= resToDate && userToDate <= resToDate && userToDate >= resFromDate)
            //    {
            //            return false;

            //    }
            
            //return true;


            // Is requested FROM date within an existing reservation
            if (userFromDate >= resFromDate && userFromDate <= resToDate)
            {
                return false;
            }
            // Is requested TO date within an existing reservation
            else if (userToDate >= resFromDate && userToDate <= resToDate)
            {
                return false;
            }
            // Is existing FROM date within requested dates
            else if (resFromDate >= userFromDate && resFromDate <= userToDate)
            {
                return false;
            }
            // Is existing TO date within requested dates
            else if (resToDate >= userFromDate && resToDate <= userToDate)
            {
                return false;
            }
            // Is there no reservation for the dates requested
            else if (resToDate == null || resFromDate == null)
            {
                return true;
            }

            return true;

        }

    }
}
