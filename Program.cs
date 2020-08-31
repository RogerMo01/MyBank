using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to My Bank");

            Home();
        }

        public static void Home()
        {
            Console.WriteLine("You always have to write the number of the option you want to select");
            Console.WriteLine("[1] LogIn");
            Console.WriteLine("[2] SignUp");

            switch (Console.ReadLine())
            {                
                case "1":
                    Bank.LogIn();
                    break;
                case "2":
                    Bank.SignIn();
                    Console.WriteLine();
                    Home();
                    break;
                default:
                    ForSwitchDefaultCase();
                    Home();
                    break;
            }
        }

        public static void TryAgainGoBackMenu()
        {
            Console.WriteLine("[1] Try it again");
            Console.WriteLine("[0] Go back to Main Menu");
            switch (Console.ReadLine())
            {
                case "1":
                    Bank.LogIn();
                    break;
                case "0":
                    Console.Clear();
                    Home();
                    break;
                default:
                    ForSwitchDefaultCase();
                    TryAgainGoBackMenu();
                    break;
            }
        }

        public static void ForSwitchDefaultCase()
        {
            Console.WriteLine("You have to choose one of the given options");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
