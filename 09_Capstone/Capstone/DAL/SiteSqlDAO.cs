using System;
using System.Collections.Generic;
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

        public IList<Site> Search(int siteID)
        {
            throw new NotImplementedException();
        }
    }
}
