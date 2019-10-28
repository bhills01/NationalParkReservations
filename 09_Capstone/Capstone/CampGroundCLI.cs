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
    class CampGroundCLI : MainCLI
    {
        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        public CampGroundCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
            : base(parksDAO, campgroundDAO, siteDAO, reservationDAO)
        {
            this.parksDAO = parksDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        int userParkId;

        /// <summary>
        /// Runs menu to give user a choice of campgrounds from the choosen park.
        /// </summary>
        /// <param name="parkName"></param>
        /// <param name="parkId"></param>
        public void RunCampGroundCLI(string parkName, int parkId)
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
            foreach (Park park in parks)
            {

                List<string> decript = Wrap(park.Description, 155);
                foreach (string dp in decript)
                {
                    Console.WriteLine($"         {dp}");
                }
                Console.WriteLine();

                userParkId = parkId;
                IList<Park> pa = parksDAO.GetAllParks();
                parkName = "";
                string parkDecript = "";
                foreach (Park name in pa)
                {
                    if (name.ParkId == userParkId)
                    {
                        parkName = name.Name;
                        parkDecript = name.Description;
                    }
                }
                Console.Clear();
                PrintHeader();
                Console.WriteLine();
                Console.WriteLine($"                                                                         {parkName}", Color.Yellow);
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                Console.WriteLine();
                foreach (string dp in decript)
                {
                    Console.WriteLine($"         {dp}", Color.DarkKhaki);
                }
                Console.WriteLine();
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);

                GetCampGroundList();
                PrintCampGroundChoices();

                while (true)
                {
                    string userChoice = Console.ReadLine();
                    IList<Campground> campgrounds = campgroundDAO.Search(userParkId);
                    foreach (Campground campground in campgrounds)
                    {
                        string cgIdString = campground.CampgroundId.ToString();
                        if (userChoice == cgIdString)
                        {
                            Console.WriteLine();
                            DateTime fromDate = CLIHelper.GetDateTime("Please Enter Arrival Date(YYYY-MM-DD): ");
                            DateTime toDate = CLIHelper.GetDateTime("Please Enter Departure Date (YYYY-MM-DD): ");
                            SiteCLI siteCLI = new SiteCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                            siteCLI.RunSiteCLI(parkName, parkId, campground.CampgroundId, campground.Name, campground.DailyFee, fromDate, toDate);
                        }
                        else
                        {
                            switch (userChoice.ToLower())
                            {

                                case "p":
                                    Console.Clear();
                                    ParkCLI parkCLI = new ParkCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                                    parkCLI.RunParkCLI();
                                    break;

                                default:
                                    Console.WriteLine("The command provided was not a valid command, please try again.", Color.Red);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all parks from the db and displays them on the screen.
        /// </summary>
        void GetCampGroundList()
        {

            IList<Campground> campgrounds = campgroundDAO.Search(userParkId);
            {
                String menu1 = "            [ {0} ]           [ {1} ]                      [ {2} ]          [ {3} ]       [ {4} ]                                                       ";

                Formatter[] fruits = new Formatter[]
                   {
    new Formatter("Campground ID", Color.WhiteSmoke),
    new Formatter("Campground Name", Color.WhiteSmoke),
    new Formatter("Open Month",Color.WhiteSmoke),
    new Formatter("Closing Month",Color.WhiteSmoke),
    new Formatter("Daily Fee", Color.WhiteSmoke),
    

                   };
                Console.WriteFormatted(menu1, Color.Yellow, fruits);
                Console.WriteLine();
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                foreach (Campground campground in campgrounds)
                {
                    string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenMonth);
                    string closedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.ClosedMonth);
                    Console.WriteLine($"                  {campground.CampgroundId.ToString().PadRight(18)}     {(campground.Name).PadRight(25)}               {openMonth.PadRight(20)}       {closedMonth.PadRight(10)}             {campground.DailyFee:C}                 ", Color.Yellow);

                }
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            }
        }
        /// <summary>
        /// Displays user choices for Campground Menu.
        /// </summary>
        void PrintCampGroundChoices()
        {
            // TODO Will, Can we add Color to the "P" and "Campground ID" here so they stand out?
            string menu = "    Press {0} - {4} Menu                       Enter {2} To {5}                             Enter Selection: ";
            Formatter[] fruits = new Formatter[]
            {
    new Formatter("P", Color.OrangeRed),
    new Formatter("M", Color.Pink),
    new Formatter("CampGround ID",Color.Green),
    new Formatter("Selection",Color.Green),
    new Formatter("Previous", Color.OrangeRed),
    new Formatter("View Sites", Color.Green)

            };


            Console.WriteLine();
            Console.WriteFormatted(menu, Color.WhiteSmoke, fruits);
        }
    }
}

