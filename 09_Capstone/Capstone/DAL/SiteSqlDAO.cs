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

        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Given a Campground ID, displays the Top 5 available sites.
        /// </summary>
        /// <param name="campgroundID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
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
    }
}
