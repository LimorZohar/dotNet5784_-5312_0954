using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalTest;
namespace BlTest
{
    internal class program
    {
        // Static field to access the BL layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        enum MainMenu { EXIT, MILESTONE, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }
        static void Main(string[] args)
        {
            Initialization.Do();

            Console.WriteLine("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

            if (ans.ToUpper() == "Y")
                DalTest.Initialization.Do();

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Manage Engineers");
                Console.WriteLine("2. Manage Tasks");
                Console.WriteLine("3. Manage Milestones");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageEngineers();
                        break;
                    case "2":
                        ManageTasks();
                        break;
                    case "3":
                        MilestoneMenu();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void ManageEngineers()
        {
            // Implement the logic for managing engineers using s_bl.Engineer methods
            // ...
        }

        public static void ManageTasks()
        {
            // Implement the logic for managing tasks using s_bl.Task methods
            // ...
        }

        public static void MilestoneMenu()
        {
            int chooseSubMenu;
            while (true)
            {
                Console.WriteLine("EXIT click 0\r\n" +
                    "CREATE click 1\r\n" +
                    "READ click 2\r\n" +
                    "READALL click 3\r\n" +
                    "UPDATE click 4\r\n" +
                    "DELETE click 5\r\n");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                }
            }
        }
    }
}



  



        
