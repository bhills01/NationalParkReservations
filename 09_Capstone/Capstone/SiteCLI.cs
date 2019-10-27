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
    class SiteCLI : MainCLI
    {
        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        public SiteCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
            : base(parksDAO, campgroundDAO, siteDAO, reservationDAO)
        {
            this.parksDAO = parksDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        int parkID;
        int userCampgroundID;
        string userCampgroundName;
        decimal userDailyFee;
        DateTime userFromDate;
        DateTime userToDate;
        public void RunSiteCLI(int parkId, int campgroundId, string campgroundName, decimal dailyFee, DateTime fromDate, DateTime toDate)
        {
            userCampgroundID = campgroundId;
            userCampgroundName = campgroundName;
            userDailyFee = dailyFee;
            userFromDate = fromDate;
            userToDate = toDate;
            
            Console.Clear();
            PrintHeader();
            GetSiteList();
            PrintSiteChoices();

            while (true)
            {
                string userChoice = Console.ReadLine();
                IList<Site> sites = siteDAO.Search(userCampgroundID, userFromDate, userToDate);
                foreach(Site site in sites)
                {
                    string siteIdString = site.SiteId.ToString();
                    if (userChoice == siteIdString)
                    {
                        int totalDays = HowManyDays(userFromDate, userToDate);
                        decimal totalCost = totalDays * userDailyFee;

                        string apptConfirm = CLIHelper.GetString($"Your total is {totalCost:C}, would you like to confirm your reservation. (Y)es or (N): ");
                        bool confirmAppointment;
                        if (apptConfirm == "y")
                        {
                            confirmAppointment = true;
                        }
                        else
                        {
                            confirmAppointment = false;
                        }
                        //Console.Write($"You your total is {totalCost}, would you like to confirm your reservation. (Y)es or (N)");
                        if (confirmAppointment == true)
                        {
                            string reservationName = CLIHelper.GetString("What name would you like to make your reservation under?: ");
                            reservationDAO.MakeReservation(userFromDate, userToDate, reservationName, site.SiteId);
                            // TODO Run Confirmation Menu

                            // Temporary for tonight
                            Console.WriteLine("****************SUCCESS, YOUR RESERVATION HAS BEEN MADE****************");
                            Console.ReadLine();
                        }
                        else
                        {
                            // Temporary for tonight
                            Console.WriteLine("Fine Then!");
                            Console.ReadLine();
                        }
                    }
                }
                switch (userChoice.ToLower())
                {
                    case "p":
                        Console.Clear();
                        CampGroundCLI campgroundCLI = new CampGroundCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                        campgroundCLI.RunCampGroundCLI(parkId);
                        break;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
            }
        }
        
        private void GetSiteList()
        {
            int totalDays = HowManyDays(userFromDate, userToDate);
            decimal totalCost = totalDays * userDailyFee;
            IList<Site> sites = siteDAO.Search(userCampgroundID, userFromDate, userToDate);
            {
                Console.WriteLine();
                Console.WriteLine($"                                             {userCampgroundName}                     |Total Price for {totalDays} Days| {totalCost:C}                       ",Color.Yellow);
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);
                Console.WriteLine("  [ Site ID ]           [ Maximum Occupancy ]         [ Handicap Accessible ]     [ Maximum RV Length ]          [ Utilities Available ]   [ Campsite Number ]                                                       ", Color.GreenYellow);
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);

                foreach (Site site in sites)
                {
                    // TODO Need to add display if no sites are available
                            string isAccessible = (site.Accesible == true) ? isAccessible = "Yes" : isAccessible = "No";
                            string hasUtilities = (site.Utilities == true) ? isAccessible = "Yes" : isAccessible = "No";
                            Console.WriteLine($"     {site.SiteId.ToString().PadRight(25)} {site.MaxOccupants.ToString().PadRight(30)}   {isAccessible.PadRight(15)}            {site.MaxRvLength + "ft.".PadRight(30)}    {hasUtilities.PadRight(20)}   {site.SiteNumber}              ", Color.GreenYellow);
                }
                Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);
            }
        }

        private void PrintSiteChoices()
        {
            // TODO Will, Can we add Color to the "P" and "Site ID" here so they stand out?
            Console.WriteLine();
            Console.Write(@"    Press P - Previous Menu                                             Enter Site ID To Propt Reservation Comfirmation                             Enter Selection: ",Color.WhiteSmoke);
        }

        /// <summary>
        /// Given two dates, calculates the difference in days
        /// </summary>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        /// <returns>an int with the difference between two dates</returns>
        public int HowManyDays(DateTime fromDate, DateTime toDate)
        {
            TimeSpan value = toDate.Subtract(fromDate);

            return value.Days;
        }
    }
}
