using System;
using System.Collections.Generic;
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


    }
}
