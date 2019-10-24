using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
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
            PrintParkMenu();
        }

        //private void RemoveEmployeeFromProject()
        //{
        //    int projectId = CLIHelper.GetInteger("Which project id is the employee removed from:");
        //    int employeeId = CLIHelper.GetInteger("Which employee is getting removed:");

        //    bool result = projectDAO.RemoveEmployeeFromProject(projectId, employeeId);

        //    if (result)
        //    {
        //        Console.WriteLine("*** SUCCESS ***");
        //    }
        //    else
        //    {
        //        Console.WriteLine("*** DID NOT CREATE ***");
        //    }

        //}

        //private void AssignEmployeeToProject()
        //{
        //    int projectId = CLIHelper.GetInteger("Which project id is the employee assigned to:");
        //    int employeeId = CLIHelper.GetInteger("Which employee is getting added:");

        //    bool result = projectDAO.AssignEmployeeToProject(projectId, employeeId);

        //    if (result)
        //    {
        //        Console.WriteLine("*** SUCCESS ***");
        //    }
        //    else
        //    {
        //        Console.WriteLine("*** DID NOT CREATE ***");
        //    }
        //}

        //private void CreateProject()
        //{
        //    string projectName = CLIHelper.GetString("Provide a name for the project:");
        //    DateTime startDate = CLIHelper.GetDateTime("Provide a start date for the project:");
        //    DateTime endDate = CLIHelper.GetDateTime("Provide an end date for the project");

        //    Project newProj = new Project()
        //    {
        //        Name = projectName,
        //        StartDate = startDate,
        //        EndDate = endDate
        //    };

        //    int id = projectDAO.CreateProject(newProj);

        //    if (id > 0)
        //    {
        //        Console.WriteLine("*** SUCCESS ***");
        //    }
        //    else
        //    {
        //        Console.WriteLine("*** DID NOT CREATE ***");
        //    }
        //}

        //private void UpdateDepartment()
        //{
        //    int departmentId = CLIHelper.GetInteger("Which department are you updating?");
        //    string updatedName = CLIHelper.GetString("Provide the new name:");
        //    Department updatedDepartment = new Department
        //    {
        //        Id = departmentId,
        //        Name = updatedName
        //    };

        //    bool result = departmentDAO.UpdateDepartment(updatedDepartment);

        //    if (result)
        //    {
        //        Console.WriteLine("*** SUCCESS ***");
        //    }
        //    else
        //    {
        //        Console.WriteLine("*** DID NOT UPDATE ***");
        //    }
        //}

        //private void CreateDepartment()
        //{
        //    string departmentName = CLIHelper.GetString("Provide a name for the new department:");
        //    Department newDept = new Department
        //    {
        //        Name = departmentName
        //    };

        //    int id = departmentDAO.CreateDepartment(newDept);

        //    if (id > 0)
        //    {
        //        Console.WriteLine("*** SUCCESS ***");
        //    }
        //    else
        //    {
        //        Console.WriteLine("*** DID NOT CREATE ***");
        //    }
        //}

        private void GetAllParks()
        {
            IList<Park> parks = parksDAO.GetAllParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine(park.ParkId.ToString().PadRight(10) + park.Name.PadRight(40));
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        //private void GetAllEmployees()
        //{
        //    IList<Employee> employees = employeeDAO.GetAllEmployees();

        //    if (employees.Count > 0)
        //    {
        //        foreach (Employee emp in employees)
        //        {
        //            Console.WriteLine(emp.EmployeeId.ToString().PadRight(5) + (emp.LastName + ", " + emp.FirstName).PadRight(30) + emp.JobTitle.PadRight(30) + emp.Gender.PadRight(3) + emp.BirthDate.ToShortDateString().PadRight(10));
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("**** NO RESULTS ****");
        //    }
        //}

        private void SelectCampground()
        {


            string parkId = CLIHelper.GetString("Enter park selection to view campground: ");
            int parkIntId = int.Parse(parkId);
            IList<Campground> campgrounds = campgroundDAO.Search(parkIntId);

            if (campgrounds.Count > 0)
            {
                Console.Clear();
                PrintHeader();
                foreach (Campground campground in campgrounds)
                {
                    Console.WriteLine($"Campground ID: {campground.CampgroundId.ToString().PadRight(5)}, Camp Ground Name: {(campground.Name).PadRight(30)} Open From:{campground.OpenMonth}, Until: {campground.ClosedMonth}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        //private void GetEmployeesWithoutProjects()
        //{
        //    IList<Employee> employees = employeeDAO.GetEmployeesWithoutProjects();

        //    if (employees.Count > 0)
        //    {
        //        foreach (Employee emp in employees)
        //        {
        //            Console.WriteLine(emp.EmployeeId.ToString().PadRight(5) + (emp.LastName + ", " + emp.FirstName).PadRight(30) + emp.JobTitle.PadRight(30) + emp.Gender.PadRight(3) + emp.BirthDate.ToShortDateString().PadRight(10));
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("**** NO RESULTS ****");
        //    }
        //}

        //private void GetAllProjects()
        //{
        //    IList<Project> projects = projectDAO.GetAllProjects();

        //    if (projects.Count > 0)
        //    {
        //        foreach (Project proj in projects)
        //        {
        //            Console.WriteLine(proj.ProjectId.ToString().PadRight(5) + proj.Name.PadRight(20) + proj.StartDate.ToShortDateString().PadRight(10) + proj.EndDate.ToShortDateString().PadRight(10));
        //        }

        //    }
        //    else
        //    {
        //        Console.WriteLine("**** NO RESULTS ****");
        //    }
        //}

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
                                                                                                                                                          
        ");
            Console.WriteLine();
            Console.WriteLine();
        }

        private void PrintMainMenu()
        {
            //Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine(" 1 - Show all available Parks");

            Console.WriteLine(" Q - Quit");
            Console.Write("Enter Selection: ");

        }

        private void PrintParkMenu()
        {
            Console.WriteLine("Select Campground to view available Sites");

            Console.Write("Enter Selection: ");
        }

    }
}
