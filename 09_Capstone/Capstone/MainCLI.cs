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
            // TODO Will, Can we add Color to the "Q" and "Press 1" here so they stand out?
            Console.WriteLine();
            Console.Write(@" Press Q - Quit                       Press 1 - To Enter The National Park Reservation Helper                                 Enter Selection: ", Color.WhiteSmoke);
        }
    }
}

   

