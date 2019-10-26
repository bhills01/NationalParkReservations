using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("npcampground");

            IParksDAO parksDAO = new ParksSqlDAO(connectionString);
            ICampgroundDAO campgroundDAO = new CampgroundSqlDAO(connectionString);
            ISiteDAO siteDAO = new SiteSqlDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationSqlDAO(connectionString);

            MainCLI mainCLI = new MainCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
            ParkCLI parkCLI = new ParkCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
            CampGroundCLI campGroundCLI = new CampGroundCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
            SiteCLI siteCLI = new SiteCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
            mainCLI.RunMainMenuCLI();

        }
    }
}
