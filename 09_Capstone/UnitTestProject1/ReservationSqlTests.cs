using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class ReservationSqlDAOTests
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
        public void SearchReservationTest()
        {
            // Arrange
            ReservationSqlDAO reserve = new ReservationSqlDAO(connectionString);

            // Act
            IList<Reservation> reserveList = reserve.Search(1);
            //Assert
            Assert.AreEqual(1, reserveList.Count);
        }


        [TestMethod]
        public void IsAvailableTest()
        {
            // Arrange
            ReservationSqlDAO reserve = new ReservationSqlDAO(connectionString);
            var date1 = new DateTime(2008, 10,10);
            // Act
            reserve.IsAvailable(date1, date1,1);
            //Assert
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void IsAvailableTest2()
        {
            // Arrange
            ReservationSqlDAO reserve = new ReservationSqlDAO(connectionString);
            var date1 = new DateTime(2019, 10, 10);
            // Act
            reserve.IsAvailable(date1, date1, 1);
            //Assert
            Assert.AreEqual(false, false);
        }

        [TestMethod]
        public void MakeReservationTest()
        {
            // Arrange
            ReservationSqlDAO reserve = new ReservationSqlDAO(connectionString);
            var date1 = new DateTime(2008, 10, 10);
            // Act
            reserve.MakeReservation(date1,date1,"bob",1);
            //Assert
            Assert.AreEqual(false, false);
        }











    }
}

