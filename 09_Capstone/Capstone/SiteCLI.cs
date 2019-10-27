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

        string userParkName;
        int userParkId;
        int userCampgroundID;
        string userCampgroundName;
        decimal userDailyFee;
        DateTime userFromDate;
        DateTime userToDate;
        string reservationName;
        string siteIdString;
        decimal totalCost;
        public void RunSiteCLI(string parkName, int parkId, int campgroundId, string campgroundName, decimal dailyFee, DateTime fromDate, DateTime toDate)
        {
            userParkName = parkName;
            userParkId = parkId;
            userCampgroundID = campgroundId;
            userCampgroundName = campgroundName;
            userDailyFee = dailyFee;
            userFromDate = fromDate;
            userToDate = toDate;
            
            Console.Clear();
            PrintHeader();
            GetSiteList();

            while (true)
            {
                string userChoice = Console.ReadLine();
                IList<Site> sites = siteDAO.Search(userCampgroundID, userFromDate, userToDate);
                foreach(Site site in sites)
                {
                    siteIdString = site.SiteId.ToString();
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
                        if (confirmAppointment == true)
                        {
                            reservationName = CLIHelper.GetString("What name would you like to make your reservation under?: ");
                            reservationDAO.MakeReservation(userFromDate, userToDate, reservationName, site.SiteId);
                            PrintConfirmationPage();
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
                        campgroundCLI.RunCampGroundCLI(parkName, parkId);
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
            totalCost = totalDays * userDailyFee;
            IList<Site> sites = siteDAO.Search(userCampgroundID, userFromDate, userToDate);
            {
                
                Console.WriteLine();
                Console.WriteLine($"                                             {userCampgroundName}                     |Total Price for {totalDays} Days| {totalCost:C}                       ",Color.Yellow);
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);
                Console.WriteLine("  [ Site ID ]           [ Maximum Occupancy ]         [ Handicap Accessible ]     [ Maximum RV Length ]          [ Utilities Available ]   [ Campsite Number ]                                                       ", Color.GreenYellow);
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);

                if (sites.Count == 0)
                {
                    Console.WriteLine("---------------------------------------------------        Sorry, no sites are available for the provided dates        ---------------------------------------------------");
                    PrintNoSitesAvailableChoices();
                    while (true)
                    {
                        string userChoice = Console.ReadLine();

                        switch (userChoice.ToLower())
                        {
                            case "p":

                                CampGroundCLI campGroundCLI = new CampGroundCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                                campGroundCLI.RunCampGroundCLI(userParkName, userParkId);
                                break;

                            case "m":
                                RunMainMenuCLI();
                                return;

                            default:
                                Console.WriteLine("The command provided was not a valid command, please try again.");
                                break;
                        }
                    }
                }
                else
                {
                    foreach (Site site in sites)
                    {
                        string isAccessible = (site.Accesible == true) ? isAccessible = "Yes" : isAccessible = "No";
                        string hasUtilities = (site.Utilities == true) ? isAccessible = "Yes" : isAccessible = "No";
                        Console.WriteLine($"     {site.SiteId.ToString().PadRight(25)} {site.MaxOccupants.ToString().PadRight(30)}   {isAccessible.PadRight(15)}            {site.MaxRvLength + "ft.".PadRight(30)}    {hasUtilities.PadRight(20)}   {site.SiteNumber}              ", Color.GreenYellow);
                    }
                    Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                    PrintSiteChoices();
                }
            }
        }

        private void PrintSiteChoices()
        {
            // TODO Will, Can we add Color to the "P" and "Site ID" here so they stand out?
            Console.WriteLine();
            Console.Write(@"    Press P - Previous Menu                                             Enter Site ID To Prompt Reservation Comfirmation                             Enter Selection: ",Color.WhiteSmoke);
        }

        private void PrintNoSitesAvailableChoices()
        {
            // TODO Will, Can we add Color to the "P" and "Site ID" here so they stand out?
            Console.WriteLine();
            Console.Write(@"    Press P - Previous Menu                                             Press M - Main menu                                    Enter Selection: ", Color.WhiteSmoke);
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

        private void PrintConfirmationPage()
        {
            string fromMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userFromDate.Month);
            string toMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userToDate.Month);
            Console.Clear();
            PrintHeader();
            Console.WriteLine();
            Console.WriteLine($"********************************************************************     RESERVATION SUCCESSFUL     *********************************************************************",Color.Gold);
            Console.WriteLine();
            Console.WriteLine("  [ Park Name ]           [ Campground ]         [ Site ID ]     [ Check-In Date ]      [ Check Out Date ]      [ Reserved For ]         [Total Due]                      ", Color.GreenYellow);
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine($"     {userParkName}                 {userCampgroundName}               {siteIdString}          {fromMonth} {userFromDate.Day}, {userFromDate.Year}         {fromMonth} {userToDate.Day}, {userFromDate.Year}         {reservationName}            {totalCost:C}                    ", Color.Gold);
            Console.WriteLine();
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine();
            Console.Write(@"    Press M - Main Menu                                             Press Q - Quit                             Enter Selection: ", Color.WhiteSmoke);

            while (true)
            {
                string userChoice = Console.ReadLine();

                Console.Clear();

                switch (userChoice.ToLower())
                {
                    case "m":

                        RunMainMenuCLI();
                        break;

                    case "q":
                        PrintHeader();
                        Console.WriteLine();
                        Console.WriteLine("---------------------------------------------------------THANK YOU FOR USING THE NATIONAL PARK RESERVATION SERVICE--------------------------------------------------------", Color.GreenYellow);
                        Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                        Console.WriteLine("**************************************************************      Press [ENTER] to exit the service      ***************************************************************", Color.LightSteelBlue);
                        Console.ReadLine();
                        Environment.Exit(0);
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
            }
        }
    }
}
