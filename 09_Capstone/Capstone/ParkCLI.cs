using Capstone.DAL;
using Capstone.Models;
using System;
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
                                Console.WriteLine("The command provided was not a valid command, please try again.");
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
            List<string> Wrap(string text, int margin)
            {
                int start = 0, end;
                var lines = new List<string>();
                text = Regex.Replace(text, @"\s", " ").Trim();

                while ((end = start + margin) < text.Length)
                {
                    while (text[end] != ' ' && end > start)
                        end -= 1;

                    if (end == start)
                        end = start + margin;

                    lines.Add(text.Substring(start, end - start));
                    start = end + 1;
                }

                if (start < text.Length)
                    lines.Add(text.Substring(start));

                return lines;
            }

            IList<Park> parks = parksDAO.GetAllParks();
            // TODO Here is where to edit the PARK MENU
            {
                Console.WriteLine("|Park ID|           |Park Name|                 |Location|           |Established|        |Size|           |Annual Visitors|                                         ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (Park park in parks)
                {
                    Console.WriteLine($" {park.ParkId.ToString().PadRight(20),5}{park.Name.ToString().PadRight(20),-30}{park.Location.ToString().PadRight(20)}{park.EstablishDate,-20:d}{park.Area.ToString().PadRight(20)}{park.VisitorCount.ToString().PadRight(20)}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    List<string> decript = Wrap(park.Description, 155);
                    Console.WriteLine();
                    foreach (string dp in decript)
                    {
                        Console.WriteLine($"         {dp}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        /// <summary>
        /// Displays user choices for Park Menu
        /// </summary>
        private void PrintParkChoices()
        {
            Console.WriteLine(" Enter Park ID to Select Park");
            Console.WriteLine(" M - Main Menu");
            Console.Write("Enter Selection: ");
        }



    }
}
