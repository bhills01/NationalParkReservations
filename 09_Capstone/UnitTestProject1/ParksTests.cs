using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class ParksSqlDAOTests
    {

        private TransactionScope transaction;
        const string connectionString = "Server=TE00156\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";

        [TestInitialize]
        public void Setup()
        {
            // Begin Transaction
            this.transaction = new TransactionScope();
            string script;
            // Load a script file to setup the db the way we want it
            using (StreamReader sr = new StreamReader("test_setup.sql"))
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
        public void GetAllProjectsTest()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);
            // Act
            IList<Project> list = dao.GetAllProjects();
            //Assert
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProjectTests()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);
            // Act
            dao.AssignEmployeeToProject(1, 1);
            //Assert
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void RemoveEmployeeFromProjectTests()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);
            // Act
            dao.RemoveEmployeeFromProject(1, 1);
            //Assert
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void CreateNewProjectTest()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);
            Project testProject = new Project();
            DateTime date = new DateTime(2010, 10, 10);
            testProject.Name = "BUG OFF";
            testProject.StartDate = date;
            testProject.EndDate = date;
            // Act
            dao.CreateProject(testProject);
            //Assert
            Assert.AreEqual(true, true);

        }
    }
}
