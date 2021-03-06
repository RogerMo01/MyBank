﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank
{
    class User
    {
        public int ActualBalance = 0;
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public List<(DateTime, int, string)> LastOperations = new List<(DateTime, int, string)>();

        public User(long id, string password, string name)
        {
            Id = id;
            Password = password;
            Name = name;
        }
        
        public static void CheckUserPassword(User user)
        {
            int counter = 0;

            while (counter < 3)
            {
                Console.WriteLine("Hello {0}, Enter your password", user.Name);
                string password = Console.ReadLine();

                if (password != user.Password)
                {
                    Console.WriteLine("Your password does't match, do it again");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadKey();
                    Console.Clear();

                    counter++;
                    if (counter == 3)
                    {
                        counter = 0;
                        Console.WriteLine("You entered a wrong password three consecutive times");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.WriteLine("Would you try to Log In again, or go back to Main menu");
                        Program.TryAgainGoBackMenu();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("This is your user interface");
                    Console.WriteLine("You always have to write the number of the option you want to select");
                    UserMainMenu(user);
                }
            }
        }


        private enum Cases
        {
            logOut, checkBalance, checkLastOperations, transferCredit, askForBorrow
        }
        public static void UserMainMenu(User user)
        {
            Console.WriteLine();
            Console.WriteLine("[1] Check Current Balance");
            Console.WriteLine("[2] Check Last Operations");
            Console.WriteLine("[3] Transfer Credit to another User");
            Console.WriteLine("[4] Ask For a Borrow");
            Console.WriteLine("[0] Log Out");


            switch (int.Parse(Console.ReadLine()))
            {
                case (int)Cases.checkBalance:
                    Console.WriteLine("Balance: {0}", user.ActualBalance);
                    UserMainMenu(user);
                    break;

                case (int)Cases.checkLastOperations:
                    ShowLastOperations(user);
                    UserMainMenu(user);
                    break;

                case (int)Cases.transferCredit:
                    Console.WriteLine("Enter the recipient Id");
                    long id = long.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the quantity to transfer");
                    int quantity = int.Parse(Console.ReadLine());

                    if (quantity > user.ActualBalance)
                    {
                        Console.WriteLine("Your actual balance is minor than the quantity you want to transfer");
                        break;
                    }
                    Bank.Transfers(user.Id, id, quantity, user.Name);

                    break;
                case (int)Cases.askForBorrow:
                    Bank.SetBorrow(user);
                    break;

                case (int)Cases.logOut:
                    Console.Clear();
                    Program.Home();
                    break;

                default:
                    Program.ForSwitchDefaultCase();
                    UserMainMenu(user);
                    break;
            }
            UserMainMenu(user);
        }

        public static void ShowLastOperations(User user)
        {
            List<(DateTime, int, string)> LastOperations = user.LastOperations;

            if (LastOperations.Count == 0)
            {
                Console.WriteLine("No operations to display");
                return;
            }

            Console.WriteLine("{0, 30}", "Last 5 Operations");
            Console.WriteLine("{0,10} {2, 30} {1,37}", "Date", "Credit", "From");
            for (int i = LastOperations.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("   {0,5} {2, 20} {1,35}", LastOperations[i].Item1, LastOperations[i].Item2, LastOperations[i].Item3);
            }
        }

        public static void HandleCredit(int quantity, User user, string from)
        {
            int balance = user.ActualBalance;
            if (balance + quantity < 0)
            {
                Console.WriteLine("The actual balance of the user is {0}", balance);
                Console.WriteLine("Failed process");
                return;
            }            
            else user.ActualBalance += quantity;
            user.LastOperations.Add((DateTime.Now, quantity, from));
        }

    }
}
