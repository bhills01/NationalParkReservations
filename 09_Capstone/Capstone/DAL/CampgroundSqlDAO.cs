using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Generates a list of campgrounds in a given park.
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        public IList<Campground> Search(int parkId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Apoen the connection to SQL Server
                connection.Open();

                // Create Command object to execute queary to get all cities
                SqlCommand cmd = new SqlCommand($"SELECT * FROM campground WHERE park_id = '{parkId}'", connection);

                // Execute the command to get a result set. read by the SQLReader
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through the rows, and print data to the screen
                List<Campground> campgrounds = new List<Campground>();
                while (reader.Read())
                {
                    Campground campground = new Campground();
                    campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                    campground.ParkId = Convert.ToInt32(reader["park_id"]);
                    campground.Name = Convert.ToString(reader["name"]);
                    campground.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                    campground.ClosedMonth = Convert.ToInt32(reader["open_to_mm"]);
                    campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                    campgrounds.Add(campground);
                }
                return campgrounds;
            }
        }
    }
}
