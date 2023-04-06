using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSharp
{
     internal class Program
     {
        static void startLoadEngineer()
        {
            Console.WriteLine("Load Engineer");
            Console.WriteLine("1) Add Flight Route.");
            Console.WriteLine("2) Manage Flight Routes");
            Console.WriteLine("3) Sign out.");
            int selection = Convert.ToInt32(Console.ReadLine());
            Console.Clear();


            if (selection == 1)
            {
                //Do stuff for adding a flight route (TBD)
            }
            if (selection == 2)
            {
                Console.WriteLine("Select a Flight Route");
                //This can be changed later.
                Console.WriteLine("0) Return to Previous Screen");
                //Flight routes would be listed here
                int chosenFlightRoute = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                if (chosenFlightRoute != 0)
                {
                    Console.WriteLine("Manage Flight Route");
                    Console.WriteLine("1) Delete Route");
                    Console.WriteLine("2) Edit Departure Time of Route");
                    int editOptionChosen = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                }
            }
            if (selection == 3)
            {
                //Do something to go back to Vikram's account sign in screen. This is a part I want to question a bit before I add much more.
            }
        }

        static void startUserLogin()
        { 
            //start of Intro Screen
            Console.WriteLine("Project Air "); 
            Console.WriteLine("1) Login ");
            Console.WriteLine("2) Create Account");
            Console.WriteLine("Enter a number to select option: ");
            int input = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            // End of Intro Screen

            // Account Creation Screen
            if (input == 2) 
            {
                string usrID, password, creditcardnum, name, bday, phone, address;

                Console.WriteLine("Enter into the following fields to make your account (the asterix means that the field is required)");

                Console.WriteLine("User ID*: ");
                usrID = Console.ReadLine();

                Console.WriteLine("Password*: ");
                password = Console.ReadLine();

                Console.WriteLine("Credit Card Number*: ");
                creditcardnum = Console.ReadLine();

                Console.WriteLine("Name: ");
                name = Console.ReadLine();

                Console.WriteLine("Birthday: ");
                bday = Console.ReadLine();

                Console.WriteLine("Phone Number: ");
                phone = Console.ReadLine();

                Console.WriteLine("Address: ");
                address = Console.ReadLine();

                if ((usrID != "") && (password != "") &&(creditcardnum != ""))
                {
                    Console.WriteLine("Account Made!");
                }

            }
            else if(input == 1) // user login screen
            {       
                string usrID, password;
                Console.WriteLine("Enter the Login:");
                Console.WriteLine("User ID: ");
                usrID = Console.ReadLine();

                Console.WriteLine("Password: ");
                password = Console.ReadLine();

               // if ((usrID != AVALIDUSERNAME) && (password != AVALIDPASSWORD))
               // {
               //     Console.WriteLine("Account Made!");
               // }
            }

          
        }
        static void Main(string[] args)
          {
            //UN COMMENT THIS LATER
            //startUserLogin();
            

          }
     }
}
