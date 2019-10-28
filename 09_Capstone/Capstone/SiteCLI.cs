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


        // List of variables used in this CLI for displaying on menus.
        private string userParkName;
        private int userParkId;
        private int userCampgroundID;
        private string userCampgroundName;
        private decimal userDailyFee;
        private DateTime userFromDate;
        private DateTime userToDate;
        private string reservationName;
        private string siteIdString;
        private decimal totalCost;

        /// <summary>
        /// Runs menu to give user a choice of sites from the choosen campground.
        /// </summary>
        /// <param name="parkName"></param>
        /// <param name="parkId"></param>
        /// <param name="campgroundId"></param>
        /// <param name="campgroundName"></param>
        /// <param name="dailyFee"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
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

        /// <summary>
        /// Retrieves all sites from the db and displays Top 5 on the screen.
        /// </summary>
        private void GetSiteList()
        {
            int totalDays = HowManyDays(userFromDate, userToDate);
            totalCost = totalDays * userDailyFee;
            IList<Site> sites = siteDAO.Search(userCampgroundID, userFromDate, userToDate);
            {
                
                Console.WriteLine();
                Console.WriteLine($"                                             {userCampgroundName}                     |Total Price for {totalDays} Days| {totalCost:C}                       ",Color.Yellow);
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);
                string menu = "  [ {0} ]           [ {1} ]         [ {2} ]     [ {3} ]          [ {4} ]   [ {5} ]                                                       ";
                Formatter[] fruits = new Formatter[]
                   {
    new Formatter("Site ID", Color.WhiteSmoke),
    new Formatter("Maximum Occupancy", Color.WhiteSmoke),
    new Formatter("OHandicap Accessible",Color.WhiteSmoke),
    new Formatter("Maximum RV Length",Color.WhiteSmoke),
    new Formatter("Utilities Available", Color.WhiteSmoke),
    new Formatter("Campsite Number", Color.WhiteSmoke),

                   };
                Console.WriteFormatted(menu, Color.Yellow, fruits);
                Console.WriteLine();
                Console.WriteLine("___________________________________________________________________________________________________________________________________________________________________________",Color.DimGray);

                if (sites.Count == 0)
                {
                    Console.WriteLine("---------------------------------------------------        Sorry, no sites are available for the provided dates        ---------------------------------------------------", Color.OrangeRed);
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
                                Console.WriteLine("The command provided was not a valid command, please try again.",Color.OrangeRed);
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
                        Console.WriteLine($"     {site.SiteId.ToString().PadRight(25)} {site.MaxOccupants.ToString().PadRight(30)}   {isAccessible.PadRight(15)}            {site.MaxRvLength + "ft.".PadRight(30)}    {hasUtilities.PadRight(20)}   {site.SiteNumber}              ",Color.Yellow);
                    }
                    Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
                    PrintSiteChoices();
                }
            }
        }

        /// <summary>
        /// Provides choices for the Site menu.
        /// </summary>
        private void PrintSiteChoices()
        {
            string menu = "    Press {0} - {4}                               Enter {2} To Prompt Reservation Comfirmation                             Enter {3}: ";
            Formatter[] fruits = new Formatter[]
            {
    new Formatter("P", Color.OrangeRed),
    new Formatter("M", Color.Pink),
    new Formatter("Site ID",Color.Yellow),
    new Formatter("Selection",Color.Yellow),
    new Formatter("Previous Menu", Color.LightSeaGreen),
    new Formatter("Main Menu", Color.Pink)

            };


            Console.WriteLine();
            Console.WriteFormatted(menu, Color.WhiteSmoke, fruits);
            
            //Console.Write(@"    Press P - Previous Menu                                             Enter Site ID To Prompt Reservation Comfirmation                             Enter Selection: ",Color.WhiteSmoke);
        }

        /// <summary>
        /// Gives choices if no sites are available.
        /// </summary>
        private void PrintNoSitesAvailableChoices()
        {
            
            string menu = "    Press {0} - {2}                                            Press {1} - {3}                                    Enter Selection: ";
            Formatter[] fruits = new Formatter[]
            {
    new Formatter("P", Color.LightSeaGreen),
    new Formatter("M", Color.Pink),
    new Formatter("Previous Menu", Color.LightSeaGreen),
    new Formatter("Main Menu", Color.Pink)

            };

            
            Console.WriteLine();
            Console.WriteFormatted(menu, Color.WhiteSmoke, fruits);
            //Console.Write(@"    Press P - Previous Menu                                             Press M - Main menu                                    Enter Selection: ", Color.WhiteSmoke);
        }

        /// <summary>
        /// Given two dates, calculates the difference in days.
        /// </summary>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        /// <returns>an int with the difference between two dates</returns>
        public int HowManyDays(DateTime fromDate, DateTime toDate)
        {
            TimeSpan value = toDate.Subtract(fromDate);

            return value.Days;
        }

        /// <summary>
        /// Prints a summary of the reservation details.
        /// </summary>
        private void PrintConfirmationPage()
        {
            int userResId = reservationDAO.GetReservationId();
            string fromMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userFromDate.Month);
            string toMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userToDate.Month);
            Console.Clear();
            PrintHeader();
            Console.WriteLine();
            Console.WriteLine($"********************************************************************     RESERVATION SUCCESSFUL     *********************************************************************",Color.Gold);
            Console.WriteLine();
            Console.WriteLine("  [Confirmation #]           [ Park Name ]           [ Campground ]         [ Site ID ]     [ Check-In Date ]      [ Check Out Date ]      [ Reserved For ]         [Total Due]                      ", Color.GreenYellow);
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine($"           {userResId}                   {userParkName}                 {userCampgroundName}               {siteIdString}          {fromMonth} {userFromDate.Day}, {userFromDate.Year}         {fromMonth} {userToDate.Day}, {userFromDate.Year}         {reservationName}            {totalCost:C}                    ", Color.Gold);
            Console.WriteLine();
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine();
            // TODO Will, Can we add Color to the "M" and "Q" here so they stand out?
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
                        Console.WriteLine("The command provided was not a valid command, please try again.",Color.OrangeRed);
                        break;
                }
            }
        }
    }
}
