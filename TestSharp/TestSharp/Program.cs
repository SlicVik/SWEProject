using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSharp
{
     internal class Program
     {

           public static string[] loginUN = new string[50]; // username array
           public static string[] loginPWD = new string[50]; // password array

          static void startCustomer()
          {
               Console.WriteLine("Project Air");
               Console.WriteLine("1) Book a Flight");
               Console.WriteLine("2) View Account History");
               Console.WriteLine("3) Print Boarding Pass");
               Console.WriteLine("4) Cancel Flight");
               Console.WriteLine("5) Change Account Details");
               Console.WriteLine("6) Sign Out");
               Console.WriteLine("Enter a number to select an option:");
               int inputNum = Convert.ToInt32(Console.ReadLine());
               Console.Clear();

               // if 1) Book a Flight
               if (inputNum == 1)
               {
                    // code for booking a flight
                    Console.WriteLine("Project Air");
                    Console.WriteLine("Book a Flight");
                    Console.WriteLine("1) Book a Round-Trip Flight");
                    Console.WriteLine("2) Book a one-way flight");
                    Console.WriteLine("Enter a number to select an option");
                    int input = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    // if 1) Book a Round-Trip Flight
                    if (input == 1)
                    {
                         Console.WriteLine("Project Air");
                         Console.WriteLine("Select a source airport");
                         Console.WriteLine("1) All source airport options will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int srcAirInput = Convert.ToInt32(Console.ReadLine());

                         Console.WriteLine("Select a destination airport");
                         Console.WriteLine("1) All destination airport options will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int destAirInput = Convert.ToInt32(Console.ReadLine());
                         Console.Clear();

                         // figure out how to read dates, reading the as string for now, will change later
                         Console.WriteLine("Enter date of departure in the format MM/DD. Flights may be booked up to 6 months in advance"); ;
                         string depDate = Console.ReadLine();
                         Console.Clear();
                         Console.WriteLine("Enter date of arrival in the format MM/DD.");
                         string arrDate = Console.ReadLine();
                         Console.Clear();

                         Console.WriteLine("Select an outbound flight");
                         Console.WriteLine("1) All outbound flights will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int outInput = Convert.ToInt32(Console.ReadLine());
                         Console.Clear();

                         Console.WriteLine("Select an inbound flight");
                         Console.WriteLine("1) All inbound flights will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int inbInput = Convert.ToInt32(Console.ReadLine());
                         Console.Clear();

                         Console.WriteLine("***We should insert flight summary and price here***");
                         Console.WriteLine("Select an payment option");
                         Console.WriteLine("1) Pay with dollars");
                         Console.WriteLine("2) Pay with points");
                         Console.WriteLine("Enter a number to select an option");
                         int payInput = Convert.ToInt32(Console.ReadLine());

                         if (payInput == 1)
                         {
                              Console.WriteLine("Enter credit card number (consider removing this)");
                              long ccNumber = Convert.ToInt64(Console.ReadLine());
                              Console.Clear();

                              Console.WriteLine("Display all flight and payment information one more time");
                              Console.WriteLine("Review booking summary. Enter 'Y' to confirm or 'N' to cancel");
                              Console.WriteLine("('N' will eventually take us back to the previous page)");
                              string conf = Console.ReadLine();
                              Console.Clear();

                              // HANDLE Y/N CHECKS HERE
                         }
                         else if (payInput == 2)
                         {
                              Console.WriteLine("Here we check if they can pay with points");
                              Console.Clear();
                              Console.WriteLine("Display all flight and payment information one more time");
                              Console.WriteLine("Review booking summary. Enter 'Y' to confirm or 'N' to cancel");
                              Console.WriteLine("('N' will eventually take us back to the previous page)");
                              string conf = Console.ReadLine();
                              Console.Clear();

                              // HANDLE Y/N CHECKS HERE
                         }
                         else
                         {
                              Console.WriteLine("Enter a valid command (then handle exceptions here)");
                         }
                    }
                    // if book a one-way flight
                    else if (input == 2)
                    {
                         Console.WriteLine("Project Air");
                         Console.WriteLine("Select a source airport");
                         Console.WriteLine("1) All source airport options will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int srcAirInput = Convert.ToInt32(Console.ReadLine());

                         Console.WriteLine("Select a destination airport");
                         Console.WriteLine("1) All destination airport options will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int destAirInput = Convert.ToInt32(Console.ReadLine());
                         Console.Clear();

                         // figure out how to read dates, reading the as string for now, will change later
                         Console.WriteLine("Enter date of departure in the format MM/DD. Flights may be booked up to 6 months in advance"); ;
                         string depDate = Console.ReadLine();
                         Console.Clear();

                         Console.WriteLine("Select an outbound flight");
                         Console.WriteLine("1) All outbound flights will be displayed here");
                         Console.WriteLine("2) ...");
                         Console.WriteLine("3) ...");
                         Console.WriteLine("Enter a number to select an option");
                         int outInput = Convert.ToInt32(Console.ReadLine());
                         Console.Clear();

                         Console.WriteLine("***We should insert flight summary and price here***");
                         Console.WriteLine("Select an payment option");
                         Console.WriteLine("1) Pay with dollars");
                         Console.WriteLine("2) Pay with points");
                         Console.WriteLine("Enter a number to select an option");
                         int payInput = Convert.ToInt32(Console.ReadLine());

                         if (payInput == 1)
                         {
                              Console.WriteLine("Enter credit card number (consider removing this)");
                              long ccNumber = Convert.ToInt64(Console.ReadLine());
                              Console.Clear();

                              Console.WriteLine("Display all flight and payment information one more time");
                              Console.WriteLine("Review booking summary. Enter 'Y' to confirm or 'N' to cancel");
                              Console.WriteLine("('N' will eventually take us back to the previous page)");
                              string conf = Console.ReadLine();
                              Console.Clear();

                              /* HANDLE Y/N CHECKS HERE
                              *  if Y, display confirmation message
                              *  if N, go back to previous screen
                              */
                         }
                         else if (payInput == 2)
                         {
                              Console.WriteLine("Here we check if they can pay with points");
                              Console.Clear();
                              Console.WriteLine("Display all flight and payment information one more time");
                              Console.WriteLine("Review booking summary. Enter 'Y' to confirm or 'N' to cancel");
                              Console.WriteLine("('N' will eventually take us back to the previous page)");
                              string conf = Console.ReadLine();
                              Console.Clear();

                              /*  HANDLE Y/N CHECKS HERE
                               *  if Y, display confirmation message
                               *  if N, go back to previous screen
                               */
                         }
                         else
                         {
                              Console.WriteLine("Enter a valid command (then handle exceptions here)");
                         }
                    }
                    else
                    {
                         Console.WriteLine("Enter a valid command (then handle exceptions here)");
                    }
               }
               // if 2) View Account History
               else if (inputNum == 2)
               {
                    Console.WriteLine("View Account History (we will probably read if from a csv file)");
                    Console.WriteLine("Flights booked: ");
                    Console.WriteLine("Flights taken: ");
                    Console.WriteLine("Flights Cancelled: ");
                    Console.WriteLine("Points available");
                    Console.WriteLine("Points used: ");
                    // handle case to cancel AKA go back to home screen
               }
               // if 3) Print Boarding Pass
               else if (inputNum == 3)
               {
                    // if there are booked flights departing in 24 hours or less
                    Console.WriteLine("Print Boarding Pass");
                    Console.WriteLine("Select a flight to print a boarding pass for");
                    Console.WriteLine("1) All flight options displayed here");
                    Console.WriteLine("2) ...");
                    Console.WriteLine("3) ...");
                    Console.WriteLine("Enter a number to select an option");
                    int brdPassInput = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    Console.WriteLine("***Boarding pass to be displayed here***");
                    // handle case to go back after viewing

                    // else if there are no booked flights departing in 24 hours or less
                    // Console.WriteLine("No boarding passes available to print");
                    // then go back to previous screen
               }
               // if 4) Cancel a Flight
               else if (inputNum == 4)
               {
                    // code for cancel a flight               
               }
               // if 5) Change Account Details
               // GET RID OF THIS ENTIRE CASE
               else if (inputNum == 5)
               {
                    Console.WriteLine("Change Account Details");
                    Console.WriteLine("1) Address");
                    Console.WriteLine("2) Phone number");
                    Console.WriteLine("3) Credit card number");
                    Console.WriteLine("Enter a number to select a field to edit:");
                    int accInput = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    if (accInput == 1)
                    {
                         Console.WriteLine("Print existing address here");
                         Console.WriteLine("Enter new address:");
                         string newAddress = Console.ReadLine();
                         // then later we put newName into the correct spot of csv file
                    }


               }
               // if 6) Sign out
               else if (inputNum == 6)
               {
                    Console.WriteLine("Signing out");
                    // add a wait statament
                    // then we must return to the screen that vikram coded
               }
          }
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

                if ((usrID == loginUN[0]) && (password == loginPWD[0]))
                {
                    startLoadEngineer();
                }
                else if((usrID == loginUN[1]) && (password == loginPWD[1]))
                { 
                    startMarkMNG();
                }
                else if((usrID == loginUN[2]) && (password == loginPWD[2]))
                { 
                    startAccoMNG();
                }
                else if((usrID == loginUN[3]) && (password == loginPWD[3]))
                { 
                    startFligMNG();
                }
                 else if((usrID == loginUN[4]) && (password == loginPWD[4]))
                { 
                    startCustomer();
                }
            }
          
        }

        static void startMarkMNG()
        {
            Console.WriteLine("Marketing Manager");
            Console.WriteLine("1) Assign a plane to a flight.");
            Console.WriteLine("2) Sign out.");
            int selection = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            if (selection == 1)
            {
                //Do stuff for assigning a plane to a flight (TBD)
            }
            if (selection == 2)
            {
                //Do something to go back to Vikram's account sign in screen. This is a part I want to question a bit before I add much more.
            }
            return;
        }

        static void startAccoMNG()
        {
            Console.WriteLine("Accountant Manager");
            Console.WriteLine("1) Generate Flight Summary Report.");
            Console.WriteLine("2) Sign out.");
            int selection = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            if (selection == 1)
            {
                //Do stuff for printing out a flight summary report (TBD)
            }
            if (selection == 2)
            {
                //Do something to go back to Vikram's account sign in screen. This is a part I want to question a bit before I add much more.
            }
            return;
        }

        static void startFligMNG()
        {
            Console.WriteLine("Flight Manager");
            Console.WriteLine("1) Generate Flight Manifest.");
            Console.WriteLine("2) Sign out.");
            int selection = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            if (selection == 1)
            {
                //Do stuff for printing out a flight manifest (TBD)
            }
            if (selection == 2)
            {
                //Do something to go back to Vikram's account sign in screen. This is a part I want to question a bit before I add much more.
            }
            return;
        }


        static void Main(string[] args)
          {
            
            loginUN[0] =   "loadENG";//Load engineer login
            loginPWD[0] =  "1234"; 

            loginUN[1] =   "markMNG";//Marketing manager login
            loginPWD[1] =  "4567"; 

            loginUN[2] =   "accoMNG";//accountant Managerlogin
            loginPWD[2] =  "7078"; 

            loginUN[3] =   "fligMNG";//Flight Manager login
            loginPWD[3] =  "2343"; 

            loginUN[4] =   "testCust";//TEST CUSTOMER TO REMOVE LATER
            loginPWD[4] =  "123";

            //UN COMMENT THIS LATER
            //startUserLogin();
            
          }
     }
}
