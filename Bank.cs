using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank
{
    static class Bank
    {
        static Dictionary<long, User> ListOfUsers = new Dictionary<long, User>();

        
        public static void GiveCredit(int quantity, long id, string from)
        {            
            if (!ListOfUsers.ContainsKey(id))
            {
                WrongKey();
                return;
            }
            else
            {
                User.ForReceiveOrRemoveCredit(quantity, ListOfUsers[id], from);
                Console.WriteLine("Process Sucessful");
            }
        }
        public static void RemoveCredit(int quantity, long id, string from)
        {
            if (!ListOfUsers.ContainsKey(id))
            {
                WrongKey();
                return;
            }
            else
            {
                quantity = quantity - (quantity * 2);
                User.ForReceiveOrRemoveCredit(quantity, ListOfUsers[id], from);
                Console.WriteLine("Process Sucessful");
            }
        }

        public static void Transfers(long idFrom, long idTo, int quantity, string nameFrom)
        {
            if (ListOfUsers.ContainsKey(idTo))
                RemoveCredit(quantity, idFrom, "Transfer");
            else
            {
                Console.WriteLine("Failed Process");
                return;
            }
            GiveCredit(quantity, idTo, nameFrom);

        }

        private static void WrongKey()
        {
            Console.WriteLine("The id entered does not correspond too any user");
            Console.WriteLine("Failed process");
            Console.ReadKey();
            return;
        }
        


        public static void SignIn()
        {
            Console.Clear();
            Console.WriteLine("Sign In");
            long id = GenerateID();

            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();

            string password = SetPassword();

            User newUser = new User(id, password, name);

            ListOfUsers.Add(id, newUser);

            Console.Clear();
            Console.WriteLine("Signed In...");
            Console.WriteLine("Hello {0}. Your ID number is {1}, you need it along whit your password whenever you want to access to our sistem", name, id);

        }
        private static string SetPassword()
        {
            Console.WriteLine("Enter a password");
            string password = Console.ReadLine();

            CheckPassword(password);

            return password;
        }
        private static void CheckPassword(string password)
        {
            Console.WriteLine("Enter your password to confirm");
            string toCheck = Console.ReadLine();

            if (!password.Equals(toCheck))
            {
                Console.WriteLine("Your password does not match, do it again");
                SetPassword();
            }            
        }
        private static long GenerateID()
        {
            DateTime forID = new DateTime();
            forID = DateTime.Now;
            string strID = forID.Day.ToString() + forID.Month + forID.Year + forID.Hour + forID.Minute + forID.Second;
            long finalId = long.Parse(strID);

            return finalId;
        }


        public static void LogIn()
        {
            Console.WriteLine("{0, 20}", "Logging In...");
            Console.WriteLine("Enter your Id");
            string readLine = Console.ReadLine();
            
            if (string.Equals(readLine, "admin"))
            {
                Console.WriteLine("Password:");
                if (Console.ReadLine().Equals("administrator"))
                    AdminMainMenu();
                else Program.Home();
            }

            long id = 0;
            if (CheckId(readLine))
                id = long.Parse(readLine);
            else LogIn();


            if (ListOfUsers.ContainsKey(id))
            {
                Console.Clear();
                User.CheckUserPassword(ListOfUsers[id]);
            }
            else
            {
                Console.WriteLine("The Id entered does't correspond to any user");
                Program.TryAgainGoBackMenu();
            }
        }


        public static void AdminMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("[1] Add User");
            Console.WriteLine("[2] Give Credit to User");
            Console.WriteLine("[3] Remove Credit from User");
            Console.WriteLine("[4] Show List of Users");
            Console.WriteLine("[5] Show Last Operations of an User");                   //Continuar con otras acciones
            Console.WriteLine("[6] Delete an User Account");

            Console.WriteLine("[0] Log Out");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter the new Id Number");
                    long id = long.Parse(Console.ReadLine());                    
                    Console.WriteLine("Enter the Name");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter the Password");
                    string password = Console.ReadLine();

                    User newUser = new User(id, password, name);
                    ListOfUsers.Add(id, newUser);
                    Console.WriteLine("Process Sucessful");
                    break;

                case "2":
                    ForUsersCreditCases(true);
                    break;

                case "3":
                    ForUsersCreditCases(false);
                    break;

                case "4":
                    ShowListOUsers();
                    break;

                case "5":
                    Console.WriteLine("Enter the id of the User");
                    long key = long.Parse(Console.ReadLine());

                    if (!ListOfUsers.ContainsKey(key))
                        WrongKey();
                    else User.ShowLastOperations(ListOfUsers[key]);
                    break;

                case "6":
                    Console.WriteLine("Enter the id of the User to Delete");
                    long idToDelete = long.Parse(Console.ReadLine());

                    if (!ListOfUsers.ContainsKey(idToDelete))
                        WrongKey();
                    else
                    {
                        ListOfUsers.Remove(idToDelete);
                        Console.WriteLine("Process Sucessful");
                    }
                    break;

                case "0":
                    Console.Clear();
                    Program.Home();
                    break;

                default:
                    Program.ForSwitchDefaultCase();
                    break;
            }
            AdminMainMenu();
        }


        private static void ForUsersCreditCases(bool giveOrRemove)
        {            
            Console.WriteLine("Enter the User id");
            long id = long.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Quantity");
            int quantity = int.Parse(Console.ReadLine());

            if (giveOrRemove)
                GiveCredit(quantity, id, "Bank");
            else RemoveCredit(quantity, id, "Bank");
        }
        private static void ShowListOUsers()
        {
            Dictionary<long, User>.KeyCollection keyCollection = ListOfUsers.Keys;
            Dictionary<long, User>.ValueCollection valueCollection = ListOfUsers.Values;
            long[] keys = keyCollection.ToArray();
            User[] values = valueCollection.ToArray();

            if (ListOfUsers.Count == 0)
            {
                Console.WriteLine("No user yet to display");
                return;
            }

            Console.WriteLine("   {0,15} {1,30} {2,35}", "Id Number", "Name", "ActualBalance");
            for (int i = 0; i < keys.Length; i++)
            {
                Console.WriteLine("   {0,20} {1,30} {2,30}", keys[i], values[i].Name, values[i].ActualBalance);
            }
        }


        public static bool CheckId(string readLine)
        {            
            try
            {
                long id = long.Parse(readLine);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("The entered Id has not the correct format");
                return false;
            }

            
        }

        public static void SetBorrow(User user)
        {            

            Console.Clear();
            Console.WriteLine("Terms and Conditions:");
            Console.WriteLine("You will have to return the amount of credit requested");
            Console.WriteLine("plus a percentage of interest in the chosen term");
            Console.WriteLine("paying an equal part each month until the term is covered");
            Console.WriteLine();

            Console.WriteLine("Terms");
            Console.WriteLine("[1] 6 months for return and a 5% of interest");
            Console.WriteLine("[2] 12 months for return and a 7% of interest");
            Console.WriteLine("[0] Cancel");
            Console.WriteLine("Enter the number of the term you want to select:");

            int percent = 0;
            int months = 0;
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Six months Term:");
                    DateTime finalDate6 = DateTime.Now.AddMonths(6);
                    Console.WriteLine("Date of last payment {0}", finalDate6.Date.ToShortDateString());
                    percent = 5;
                    months = 6;
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Twelve months Term:");
                    DateTime finalDate12 = DateTime.Now.AddYears(1);
                    Console.WriteLine("Date of last payment {0}", finalDate12.Date.ToShortDateString());
                    percent = 7;
                    months = 12;
                    break;
                case "0":
                    Console.Clear();
                    User.UserMainMenu(user);
                    break;
                default:
                    Program.ForSwitchDefaultCase();
                    SetBorrow(user);
                    break;
            }

            Console.WriteLine("How many credit you want to Borrow?");
            int quantityToBorrow = int.Parse(Console.ReadLine());

            int percentOfTotal = (quantityToBorrow * percent) / 100;

            Console.WriteLine("The amount to be returned would be: {0}", quantityToBorrow + percentOfTotal);
            Console.WriteLine("paying monthly {0}", (quantityToBorrow + percentOfTotal)/months);

            FinishBorrow(user, quantityToBorrow);
        }
        private static void FinishBorrow(User user, int quantity)
        {
            Console.WriteLine();
            Console.WriteLine("[1] Agree");
            Console.WriteLine("[2] Not Agree");
            switch (Console.ReadLine())
            {
                case "1":
                    GiveCredit(quantity, user.ID, "Borrow");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    User.UserMainMenu(user);
                    break;
                case "2":
                    Console.WriteLine("Operation Canceled");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    User.UserMainMenu(user);
                    break;
                default:
                    Program.ForSwitchDefaultCase();
                    FinishBorrow(user, quantity);
                    break;
            }
        }

    }
}
