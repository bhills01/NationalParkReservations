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

        public IList<Site> Search(int campgroundID)
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
    }
}
