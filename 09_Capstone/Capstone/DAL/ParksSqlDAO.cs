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

        public IList<Park> GetAllParks()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Apoen the connection to SQL Server
                connection.Open();

                // Create Command object to execute queary to get all cities
                SqlCommand cmd = new SqlCommand("SELECT * FROM park", connection);

                // Execute the command to get a result set. read by the SQLReader
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through the rows, and print data to the screen
                List<Park> parks = new List<Park>();
                while (reader.Read())
                {
                    Park park = new Park();

                    park.ParkId = Convert.ToInt32(reader["park_id"]);
                    park.Name = Convert.ToString(reader["name"]);
                    park.Location = Convert.ToString(reader["location"]);
                    park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                    park.Area = Convert.ToInt32(reader["area"]);
                    park.VisitorCount = Convert.ToInt32(reader["vistors"]);
                    park.Description = Convert.ToString(reader["description"]);

                    parks.Add(park);
                }
                return parks;
            }
        }
    }
}
