using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class SitesSqlDAOTests
    {

        private TransactionScope transaction;
        const string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";

        [TestInitialize]
        public void Setup()
        {
            // Begin Transaction
            this.transaction = new TransactionScope();
            string script;
            // Load a script file to setup the db the way we want it
            using (StreamReader sr = new StreamReader(@"..\..\..\..\UnitTestProject1\TextFile1.SQL"))
            {
                script = sr.ReadToEnd();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(script, conn);

                SqlDataReader rdr = cmd.ExecuteReader();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            this.transaction.Dispose();
        }



        [TestMethod]
        public void GetAllCampgroundsTest()
        {
            // Arrange
            CampgroundSqlDAO caGr = new CampgroundSqlDAO(connectionString);

            // Act
            IList<Campground> campGrList = caGr.GetAllCampground();
            //Assert
            Assert.AreEqual(2, campGrList.Count);
        }

        [TestMethod]
        public void SearchCampgroundsTest()
        {
            // Arrange
            CampgroundSqlDAO caGr = new CampgroundSqlDAO(connectionString);

            // Act
            IList<Campground> campGrList = caGr.Search(1);
            //Assert
            Assert.AreEqual(1, campGrList.Count);
        }

















    }
}

