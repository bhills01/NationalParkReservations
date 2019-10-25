using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class ParkCLI
    {

        private IParksDAO parksDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;

        public ParkCLI(IParksDAO parksDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parksDAO = parksDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

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
                        RunParkMenuCLI();
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

        public void RunParkMenuCLI()
        {
            Console.Clear();
            PrintHeader();
            GetAllParks();

            while (true)
            {
                SelectCampground();
                break;
            }
            PrintCampgroundMenu();
            RunSiteMunuCLI();
        }

        public void RunSiteMunuCLI()
        {
            Console.Clear();
            PrintHeader();
            PrintCampsiteSelection(parkIntId);

            while (true)
            {
                //SelectSite();
                break;
            }
            PrintSiteMenu();
            RunMainMenuCLI();
        }



        private void GetAllParks()
        {
            IList<Park> parks = parksDAO.GetAllParks();

            if (parks.Count > 0)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine("|Park ID|           |Park Name|                 |Location|           |Established|        |Size|           |Annual Visitors|                                         ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (Park park in parks)
                {
                    Console.WriteLine($" {park.ParkId.ToString().PadRight(20),5}{park.Name.ToString().PadRight(20),-30}{park.Location.ToString().PadRight(20)}{park.EstablishDate,-20:d}{park.Area.ToString().PadRight(20)}{park.VisitorCount.ToString().PadRight(20)}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine($"{park.Description,-20}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                 
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        public int parkIntId = 0;
        private void SelectCampground()
        {


            string parkId = CLIHelper.GetString("Enter park selection to view campground: ");
            parkIntId = int.Parse(parkId);

            IList<Campground> campgrounds = campgroundDAO.Search(parkIntId);

            

            if (campgrounds.Count > 0)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine();
                Console.WriteLine(@"Campground ID                           Campground Name                       OpenMonth            Closed Month              
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (Campground campground in campgrounds)
                {
                    Console.WriteLine($"{campground.CampgroundId.ToString().PadRight(20)}{(campground.Name).PadRight(30)} {campground.OpenMonth}{campground.ClosedMonth}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }


        // BOOK MARK THIS

        //private void SelectReservation()
        //{


        //    string siteId = CLIHelper.GetString("Enter site selection to check avialability: ");
        //    int siteIntId = int.Parse(siteId);

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

        private void PrintCampsiteSelection(int parkID)
        {
            IList<Campground> campgrounds = campgroundDAO.Search(parkIntId);

                foreach (Campground campground in campgrounds)
                {
                    string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenMonth);
                    string closedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.ClosedMonth);
                    Console.WriteLine($"Campground ID: {campground.CampgroundId.ToString().PadRight(5)}, Camp Ground Name: {(campground.Name).PadRight(30)} Open From:{openMonth}, Until: {closedMonth}");
                }
        }


        private void PrintHeader()
        {
            Console.WriteLine(@"                                                                                                                                                                                                                                                                                                                       
       %#           @@@& @@@@    @@@@   @@@@@@@. @@@* @@@@@@@@@  @@@# @@@@    @@@@    @@@@        @@@@@@@@@   @@@@&    @@@@@@@@@ .@@@  &@@@         %%         
      @@( .         @@@@ @@@@   /@@@@# (#%@@@%## @@@* @@@&.#@@@  @@@@ @@@@   *@@@@%   @@@@        @@@/./@@@   @@@@@    @@@*.@@@@ .@@@  @@@%        &@( /       
      @@@@/         @@@@ @@@@   @@@@@@   .@@@.   @@@* @@@# *@@@  @@@@@@@@@   @@@@@@   @@@@        @@@, .@@@  ,@@@@@.   @@@. %@@@ .@@@ %@@@         &@@@%       
      (#*@          @@@@@@@@@   @@@@@@,  .@@@.   @@@* @@@# *@@@  @@@@@@@@@  .@@@@@@   @@@@        @@@, .@@@  @@@&@@@   @@@. %@@@ .@@@ @@@*       (&.%@@(@,     
     *%@@@@/        @@@@@@@@@  #@@@#@@@  .@@@.   @@@* @@@# *@@@  @@@@@@@@@  @@@&@@@&  @@@@        @@@, .@@@  @@@ @@@   @@@(/@@@@ .@@@@@@@,       /#@@@@# ,/    
     @@@@@@&, .     @@@%@@@@@  @@@/ @@@  .@@@.   @@@* @@@# *@@@  @@@@@@@@@  @@@,,@@@  @@@@        @@@@@@@@@ *@@@ @@@%  @@@@@@@@@ .@@@ @@@@        @@@@@@@, ,   
  #%@%%@@@&%%*      @@@ @@@@@  @@@@@@@@* .@@@.   @@@* @@@# *@@@  @@@ @@@@@ ,@@@@@@@@. @@@@        @@@/,,,   @@@@@@@@@  @@@.@@@@  .@@@ *@@@,    #(@&%@@@@&%,    
   (@@@@@@@@#       @@@ &@@@@ %@@@@@@@@@ .@@@.   @@@* @@@@@@@@@  @@@ #@@@@ @@@@@@@@@@ @@@@@@@,    @@@,     .@@@@@@@@@  @@@. @@@* .@@@  @@@@     ,&@@@@@@@%     
       #@           @@@  @@@@ @@@#  /@@@ .@@@.   @@@* (@@@@@@@%  @@@  @@@@ @@@%  &@@@ @@@@@@@     @@@,     &@@@   @@@& @@@. @@@@ .@@@  @@@@         ,@         
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ");
           
            
        }

        private void PrintMainMenu()
        {
            //Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine(" 1 - Show all available Parks");

            Console.WriteLine(" Q - Quit");
            Console.Write("Enter Selection: ");

        }

        private void PrintCampgroundMenu()
        {
            Console.Write("Select Campground to view available Sites: ");

        }

        private void PrintSiteMenu()
        {
            Console.WriteLine("Select Site ID to check for availability: ");
        }

    }
}
