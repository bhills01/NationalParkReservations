using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParksSqlDAO : IParksDAO
    {
        private string connectionString;

        public ParksSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Displays all parks within the Forest Search system.
        /// </summary>
        /// <returns></returns>
        public IList<Park> GetAllParks()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM park", connection);

                SqlDataReader reader = cmd.ExecuteReader();

                List<Park> parks = new List<Park>();
                while (reader.Read())
                {
                    Park park = new Park();

                    park.ParkId = Convert.ToInt32(reader["park_id"]);
                    park.Name = Convert.ToString(reader["name"]);
                    park.Location = Convert.ToString(reader["location"]);
                    park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                    park.Area = Convert.ToInt32(reader["area"]);
                    park.VisitorCount = Convert.ToInt32(reader["visitors"]);
                    park.Description = Convert.ToString(reader["description"]);

                    parks.Add(park);
                }
                return parks;
            }
        }
    }
}
