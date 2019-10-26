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
            userParkId = parkId;
            Console.Clear();
            PrintHeader();
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
                        // TODO need to put fix it if the user does not enter date correctly here. Can probably override CliHelper Get String method to fix
                        //string from = CLIHelper.GetString("Please enter arrival Date (YYYY-MM-DD): ");
                        //string to = CLIHelper.GetString("Please enter arrival Date (YYYY-MM-DD): ");
                        //DateTime fromDate = DateTime.Parse(from);
                        //DateTime toDate = DateTime.Parse(to);                        
                        DateTime fromDate = CLIHelper.GetDateTime("Please enter arrival Date(YYYY-MM-DD): ");
                        DateTime toDate = CLIHelper.GetDateTime("Please enter arrival Date (YYYY-MM-DD): ");
                        SiteCLI siteCLI = new SiteCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                        siteCLI.RunSiteCLI(campground.CampgroundId, campground.Name, campground.DailyFee, fromDate, toDate);


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
        /// Displays the camps in the choosen park ID.
        /// </summary>
        private void GetCampGroundList()
        {

            IList<Campground> campgrounds = campgroundDAO.Search(userParkId);
            // TODO Here is where to edit the CAMPGROUND MENU
            {
                Console.WriteLine("|Campground ID|           |Campground Name|                 |Open Month|           |Closing Month|        |Daily Fee|                                                       ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (Campground campground in campgrounds)
                {
                    string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenMonth);
                    string closedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.ClosedMonth);
                    Console.WriteLine($"      {campground.CampgroundId.ToString().PadRight(5)}     {(campground.Name).PadRight(30)}          {openMonth}               {closedMonth}                       {campground.DailyFee:C}                 ");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }
        /// <summary>
        /// Displays user choices for Campground Menu
        /// </summary>
        private void PrintCampGroundChoices()
        {
            Console.WriteLine(" Enter Campground ID to view Site availability");
            Console.WriteLine(" M - Main Menu");
            Console.Write("Enter Selection: ");
        }

    }
}
