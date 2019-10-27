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

namespace Capstone
{
    public class ParkCLI : MainCLI
    {
 
        // Instanciating elements for the constructor
        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        public ParkCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
            :base(parksDAO, campgroundDAO, siteDAO, reservationDAO)
        {
            // Creating classes for use in this CLI
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
                        campGroundCLI.RunCampGroundCLI(park.ParkId);

                    }
                    else
                    {
                        switch (userChoice.ToLower())
                        {

                            case "m":
                                Console.Clear();
                                MainCLI mainCLI = new MainCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                                mainCLI.RunMainMenuCLI();
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
        /// Retrieves all parks from the db and displays them on the screen
        /// </summary>
        private void GetAllParks()
        {
            IList<Park> parks = parksDAO.GetAllParks();
            
            {
                Console.WriteLine("                    [ Park ID ]           [ Park Name ]                [ Location ]          [ Established ]     [ Size ]         [ Annual Visitors ]                                         ", Color.GreenYellow);
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                foreach (Park park in parks)
                {
                    Console.WriteLine($"                         {park.ParkId.ToString().PadRight(20),5}{park.Name.ToString().PadRight(20),-30}{park.Location.ToString().PadRight(20)}{park.EstablishDate,-20:d}{park.Area.ToString().PadRight(20)}{park.VisitorCount.ToString().PadRight(20)}",Color.GreenYellow);
                }
            }
        }

        /// <summary>
        /// Displays user choices for Park Menu
        /// </summary>
        private void PrintParkChoices()
        {
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine();
            Console.Write(@"    Press M - Main Menu                                             Enter Park ID To Select Park: ", Color.WhiteSmoke);
           
        }
    }
}
