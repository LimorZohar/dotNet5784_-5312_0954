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

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        enum MainMenu { EXIT, MILESTONE, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        public static void MilestoneMenu ()
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
                    case 1:
                        Console.WriteLine()
                }
        }
            
            
        static void Main()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
        }


    }
}
