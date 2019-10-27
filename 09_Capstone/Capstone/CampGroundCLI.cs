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
        /// <param name="parkId"></param>
        public void RunCampGroundCLI(int parkId)
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
                string parkName = "";
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
                    Console.WriteLine($"         {dp}", Color.ForestGreen);
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
                            siteCLI.RunSiteCLI(parkId, campground.CampgroundId, campground.Name, campground.DailyFee, fromDate, toDate);
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
            /// <summary>
            /// Displays the camps in the choosen park ID.
            /// </summary>
            void GetCampGroundList()
            {

                IList<Campground> campgrounds = campgroundDAO.Search(userParkId);
                // TODO Here is where to edit the CAMPGROUND MENU
                {
                    Console.WriteLine("            [ Campground ID ]           [ Campground Name ]            [ Open Month ]           [ Closing Month ]        [ Daily Fee ]                                                       ", Color.YellowGreen);
                    Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                    foreach (Campground campground in campgrounds)
                    {
                        string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenMonth);
                        string closedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.ClosedMonth);
                        Console.WriteLine($"                  {campground.CampgroundId.ToString().PadRight(20)}     {(campground.Name).PadRight(15)}               {openMonth.PadRight(20)}       {closedMonth.PadRight(10)}             {campground.DailyFee:C}                 ", Color.YellowGreen);
                        
                    }
                    Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                }
            }
            /// <summary>
            /// Displays user choices for Campground Menu
            /// </summary>
            void PrintCampGroundChoices()
            {
                // TODO Will, Can we add Color to the "P" and "Campground ID" here so they stand out?
                Console.WriteLine();
                Console.Write(@"    Press P - Previous Menu                                             Enter Campground ID To View Sites                                      Enter Selection: ",Color.WhiteSmoke);
            }
        }
    }
}
