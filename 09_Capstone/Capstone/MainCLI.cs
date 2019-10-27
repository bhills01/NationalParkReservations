using Capstone.DAL;
using System;
using System.Drawing;
using Console = Colorful.Console;
using Capstone.Models;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Capstone
{
    public class MainCLI
    {

        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;

        public MainCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parksDAO = parksDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }
        
        /// <summary>
        /// Main Menu that prints header and menu options. Pressing one will open the Park Reservation service and Q will quit application
        /// </summary>
        public void RunMainMenuCLI()
        {
            PrintHeader();
            PrintMainMenu();

            while (true)
            {
                string userChoice = Console.ReadLine();

                Console.Clear();

                switch (userChoice.ToLower())
                {
                    case "1":
                        
                        ParkCLI parkCLI = new ParkCLI(parksDAO, campgroundDAO, siteDAO, reservationDAO);
                        parkCLI.RunParkCLI();


                        break;

                    case "q":
                        Console.WriteLine("Thank you for using the National Forest Reservation Service!");
                        Console.WriteLine("Press [EXIT] to eixt the service.");
                        Console.ReadLine();
                        Environment.Exit(0);
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
            }
        }


        //public void RunParkMenuCLI()
        //{
        //    Console.Clear();
        //    PrintHeader();
        //    GetAllParks();
        //    GetCampground();

        //    PrintCampgroundMenu();
        //    RunSiteMunuCLI();
        //}

        //public void RunSiteMunuCLI()
        //{
        //    Console.Clear();
        //    PrintHeader();
        //    PrintCampsiteSelection(parkIntId);
        //    SelectSite();

        //    PrintSiteMenu();
        //    RunReservationMunuCLI();
        //}

        //public void RunReservationMunuCLI()
        //{
        //    Console.Clear();
        //    PrintHeader();
        //    PrintSiteSelection(siteIntId);

        //    while (true)
        //    {
        //        SelectReservation();
        //        break;
        //    }
        //    PrintSiteMenu();
        //    RunMainMenuCLI();
        //}


            //---------------------------------------------------------Contains code for Wraping text 
        //    private void GetAllParks()
        //    {
        //        List<string> Wrap(string text, int margin)
        //        {
        //            int start = 0, end;
        //            var lines = new List<string>();
        //            text = Regex.Replace(text, @"\s", " ").Trim();

        //            while ((end = start + margin) < text.Length)
        //            {
        //                while (text[end] != ' ' && end > start)
        //                    end -= 1;

        //                if (end == start)
        //                    end = start + margin;

        //                lines.Add(text.Substring(start, end - start));
        //                start = end + 1;
        //            }

        //            if (start < text.Length)
        //                lines.Add(text.Substring(start));


        //            return lines;
        //        }





        //        IList<Park> parks = parksDAO.GetAllParks();

        //        if (parks.Count > 0)
        //        {
        //            Console.Clear();
        //            PrintHeader();
        //    Console.WriteLine("|Park ID|           |Park Name|                 |Location|           |Established|        |Size|           |Annual Visitors|                                         ");
        //            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        //            foreach (Park park in parks)
        //            {
        //                Console.WriteLine($" {park.ParkId.ToString().PadRight(20),5}{park.Name.ToString().PadRight(20),-30}{park.Location.ToString().PadRight(20)}{park.EstablishDate,-20:d}{park.Area.ToString().PadRight(20)}{park.VisitorCount.ToString().PadRight(20)}");
        //                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        //                List<string> decript = Wrap(park.Description, 155);
        //    Console.WriteLine();
        //                foreach (string dp in decript)
        //                {
        //                    Console.WriteLine($"         {dp}");
        //                }
        //Console.WriteLine();
        //                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("**** NO RESULTS ****");
        //        }
        //    }

        //public int parkIntId = 0;
        //private void GetCampground()
        //{
        //    string parkId = CLIHelper.GetString("Enter park selection to view campground: ");
        //    parkIntId = int.Parse(parkId);
        //}

        //public int siteIntId = 0;
        //private void SelectSite()
        //{


        //    string siteId = CLIHelper.GetString("Enter campground selection to check site avialability: ");
        //    siteIntId = int.Parse(siteId);

        //    IList<Site> sites = siteDAO.Search(siteIntId);



        //    if (sites.Count > 0)
        //    {
        //        Console.Clear();
        //        PrintHeader();
        //        foreach (Site site in sites)
        //        {
        //            string utilitiesAvailable;
        //            if (site.Utilities == true)
        //            {
        //                utilitiesAvailable = "Yes";
        //            }
        //            else
        //            {
        //                utilitiesAvailable = "No";
        //            }
        //            Console.WriteLine($"Site ID: {site.SiteId.ToString().PadRight(5)}, Site Number: {site.SiteNumber.ToString().PadRight(30)} Max Occupants {site.MaxOccupants}, Accesible {site.Accesible}, Max RV Length: {site.MaxRvLength}, Utilities Available?: {utilitiesAvailable}");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("**** NO RESULTS ****");
        //    }
        //}


        // TODO complete Select Reservation
        //public string siteSelection;
        //private void SelectReservation()
        //{
        //    string siteIdString = CLIHelper.GetString("Please enter the site you would like to reserve: ");
        //    int siteId = int.Parse(siteIdString);

        //    string checkInDate = CLIHelper.GetString("Enter Check-In Date (YYYY-MM-DD): ");
        //    DateTime checkIn = DateTime.Parse(checkInDate);
        //    string checkInMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(checkIn.Month);

        //    string checkOutDate = CLIHelper.GetString("Enter Check-Out Date (YYYY-MM-DD): ");
        //    DateTime checkOut = DateTime.Parse(checkOutDate);
        //    string checkOutMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(checkOut.Month);

        //    bool siteAvailable = reservationDAO.IsAvailable(checkIn, checkOut, siteId);


        //    if (siteAvailable == true)
        //    {
        //        decimal totalPlaceHolder = 149.99M;
        //        string confirmReservation = CLIHelper.GetString($"Those dates are available. Would you like to confirm and reserve those dates for {totalPlaceHolder}. (Y)es or (N)o: ");

        //        if (confirmReservation == "y")
        //        {
        //            string rezName = CLIHelper.GetString("Please Enter the full name of the party that is Registering: ");
        //            reservationDAO.MakeReservation(checkIn, checkOut, rezName, siteId);
        //            Console.WriteLine("***RESERVATION CONFIRMED SUCCESSFULLY***");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine();
        //        Console.WriteLine(".....loser....");
        //    }

        //}

        //private void PrintCampsiteSelection(int parkID)
        //{
        //    IList<Campground> campgrounds = campgroundDAO.Search(parkIntId);

        //        foreach (Campground campground in campgrounds)
        //        {
        //string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenMonth);
        //string closedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.ClosedMonth);
        //Console.WriteLine($"Campground ID: {campground.CampgroundId.ToString().PadRight(5)}, Camp Ground Name: {(campground.Name).PadRight(30)} Open From:{openMonth}, Until: {closedMonth}");
        //        }
        //}

        //private void PrintSiteSelection(int siteId)
        //{
        //    IList<Site> sites = siteDAO.Search(siteIntId);

        //    Console.Clear();
        //    PrintHeader();
        //    foreach (Site site in sites)
        //    {
        //        string utilitiesAvailable;
        //        if (site.Utilities == true)
        //        {
        //            utilitiesAvailable = "Yes";
        //        }
        //        else
        //        {
        //            utilitiesAvailable = "No";
        //        }
        //        Console.WriteLine($"Site ID: {site.SiteId.ToString().PadRight(5)}, Site Number: {site.SiteNumber.ToString().PadRight(30)} Max Occupants {site.MaxOccupants}, Accesible {site.Accesible}, Max RV Length: {site.MaxRvLength}, Utilities Available?: {utilitiesAvailable}");
        //    }
        //}

            /// <summary>
            /// prints the Title Header
            /// </summary>
        protected void PrintHeader()
        {

            
            string s= @"                              
                                                                             MM                                
                                                                            MMMM                                
                                                                           MMMMMM                               
                                                                         MMMM  MMMM                              
                                                                        MMMM    MMMM                             
                                                                       MMMM      MMMM                           
                                                                      MMMM        MMMMM                         
                                                                    MMMMM          MMMMM                         
                                                        M         MMMMMM            MMMMMM         M                
                                                       MMM       MMMMMMM              MMMMMM      MMM               
                                                      MMMMM    MMMMMMM                MMMMMMM    NMMMM              
                                                     MMMMMMM. MMMMMMMMMM            +MMMMMMMMM  =MMMMMM             
                                                    MMMMMMMMOMMMMMMMMMM              MMMMMMMMMMMMMMMMMMM             
                                                   MMMMMMMMMMMMMMMMMMM                MMMMMMMMMMMMMMMMMMM.           
                                                  MMMMMMMMMMMMMMMMMMM=                 MMMMMMMMMMMMMMMMMMM           
                                                 MMMMMMMMMMMMMMMMMMMM.                 .MMMMMMMMMMMMMMMMMMM          
                                                MMMMMMMMMMMMMMMMMMMMMMMMMMMM    MMMMMMMMMMMMMMMMMMMMMMMMMMMM          
                                               MMMMMMMMMMMMMMMMMMMMMMMMMMMMM    MMMMMMMMMMMMMMMMMMMMMMMMMMMMM         
                   
";
            
            int r = 255;
            int g = 255;
            int b = 0;
            foreach (char ch in s)
            {
                
               Console.Write(ch, Color.FromArgb(r, g, b));

                if (r >= 36 && g >= 18)
                {
                    r -= 36;
                    g -= 18;
                }
                else
                {
                     r = 255;
                     g = 255;
                     b = 0;
                }
                

            }

 Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
            Console.WriteLine(@"                               ______  ____   _____   ______   _____  _______    _____  ______            _____    _____  _    _ 
                              |  ____|/ __ \ |  __ \ |  ____| / ____||__   __|  / ____||  ____|    /\    |  __ \  / ____|| |  | |
                              | |__  | |  | || |__) || |__   | (___     | |    | (___  | |__      /  \   | |__) || |     | |__| |
                              |  __| | |  | ||  _  / |  __|   \___ \    | |     \___ \ |  __|    / /\ \  |  _  / | |     |  __  |
                              | |    | |__| || | \ \ | |____  ____) |   | |     ____) || |____  / ____ \ | | \ \ | |____ | |  | |
                              |_|     \____/ |_|  \_\|______||_____/    |_|    |_____/ |______|/_/    \_\|_|  \_\ \_____||_|  |_|", Color.WhiteSmoke);
            Console.WriteLine("__________________________________________________________________________________________________________________________________________________________________________", Color.DimGray);
           
            
        }
        /// <summary>
        /// prints the two options available on the main menu
        /// </summary>
        private void PrintMainMenu()
        {
            Console.WriteLine();
            Console.Write(@" Press Q - Quit                       Press 1 - To Enter The National Park Reservation Helper: ");
            
        }

        //private void PrintCampgroundMenu()
        //{
        //    Console.Write("Select Campground to view available Sites: ");

        //}

        //private void PrintSiteMenu()
        //{

        //    Console.WriteLine("Select Site ID to check for availability: ");
        //}


        //public int HowManyDays(DateTime toDate, DateTime fromDate)
        //{
        //    TimeSpan value = toDate.Subtract(fromDate);

        //    return value.Days;
        //}

    }

}

   

