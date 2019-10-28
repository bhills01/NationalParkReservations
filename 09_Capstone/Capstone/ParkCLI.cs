using Capstone.DAL;
using Capstone.Models;
using System;
using System.Drawing;
using Console = Colorful.Console;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Colorful;

namespace Capstone
{
    public class ParkCLI : MainCLI
    {
        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        public ParkCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
            :base(parksDAO, campgroundDAO, siteDAO, reservationDAO)
        {
            this.parksDAO = parksDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        /// <summary>
        /// Runs menu to give user a choice of parks.
        /// </summary>
        public void RunParkCLI()
        {
            Console.Clear();
            PrintHeader();
            GetAllParks();
            PrintParkChoices();

            while (true)
            {
                string userChoice = Console.ReadLine();
                IList<Park> parks = parksDAO.GetAllParks();
                foreach (Park park in parks)
                {
                    string parkIdString = park.ParkId.ToString();
                    if (userChoice == parkIdString)
                    {
                        CampGroundCLI campGroundCLI = new CampGroundCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                        campGroundCLI.RunCampGroundCLI(park.Name, park.ParkId);

                    }
                    else
                    {
                        switch (userChoice.ToLower())
                        {

                            case "p":
                                Console.Clear();
                                RunMainMenuCLI();
                                break;

                            default:
                                Console.WriteLine("The command provided was not a valid command, please try again.",Color.OrangeRed);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all parks from the db and displays them on the screen.
        /// </summary>
        private void GetAllParks()
        {
            IList<Park> parks = parksDAO.GetAllParks();

            {
                string menu = "                    [ {0} ]           [ {1} ]                [ {2} ]          [ {3} ]     [ {4} ]         [ {5} ]                                         ";

                
                Formatter[] fruits = new Formatter[]
                {
    new Formatter("Park ID", Color.WhiteSmoke),
    new Formatter("Park Name", Color.WhiteSmoke),
    new Formatter("Location",Color.WhiteSmoke),
    new Formatter("Established",Color.WhiteSmoke),
    new Formatter("Size", Color.WhiteSmoke),
    new Formatter("Annual Visitors", Color.WhiteSmoke)

                };
                Console.WriteFormatted(menu, Color.Yellow, fruits);
                Console.WriteLine();
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                foreach (Park park in parks)
                {
                    Console.WriteLine($"                         {park.ParkId.ToString().PadRight(20),5}{park.Name.ToString().PadRight(20),-30}{park.Location.ToString().PadRight(20)}{park.EstablishDate,-20:d}{park.Area.ToString().PadRight(20)}{park.VisitorCount.ToString().PadRight(20)}",Color.Yellow);
                }
            }
        }

        /// <summary>
        /// Displays user choices for Park Menu.
        /// </summary>
        private void PrintParkChoices()
        {
            // TODO Will, Can we add Color to the "p" and "Park ID" here so they stand out?
            string menu = "    Press {0} - {4} Menu                       Enter {2} To {5} Park                             Enter Selection: ";
            Formatter[] fruits = new Formatter[]
            {
    new Formatter("P", Color.OrangeRed),
    new Formatter("M", Color.Pink),
    new Formatter("Park ID",Color.Green),
    new Formatter("Selection",Color.Green),
    new Formatter("Previous", Color.OrangeRed),
    new Formatter("Select", Color.Green)

            };


            
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine();
            Console.WriteFormatted(menu, Color.WhiteSmoke, fruits);
        }
    }
}
