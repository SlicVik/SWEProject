using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace TestSharp
{
     class Customer // use this class as a way to more easily keep track of and read multiple customers
     {
          int ccnum; // creditcardnum
          int credits;
          int points;
          string name;
          string password;
          string bday;
          string usrID;
          string address;
          string phonenum;

     }
     internal class Program
     {

          public static string[] loginUN = new string[50]; // username array
          public static string[] loginPWD = new string[50]; // password array

        // CUSTOMER METHODS BEGIN HERE

            static void changePass()
            { 
        
            }

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
                    bookAFlight();
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
                    Console.WriteLine("Change Password");

                    changePass();

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
                    signOut();
               }
               else
               {
                    // handle error
               }
          }

          static void bookAFlight()
          {
               // code for booking a flight
               Console.WriteLine("Project Air");
               Console.WriteLine("Book a Flight");
               Console.WriteLine("1) Book a Round-Trip Flight");
               Console.WriteLine("2) Book a one-way flight");
               Console.WriteLine("3) Go back");
               Console.WriteLine("Enter a number to select an option");
               int input = Convert.ToInt32(Console.ReadLine());
               Console.Clear();

               // if 1) Book a Round-Trip Flight
               if (input == 1)
               {
                    bookRoundTrip();
               }
               // if book a one-way flight
               else if (input == 2)
               {
                    bookOneWay();
               }
               else if (input == 3)
               {
                    startCustomer();
               }
               else
               {
                    // MUST ADD
                    // print error message, then call bookAFlight()
                    // this isn't a perfect idea though, due to what prints out
                    Console.WriteLine("Enter a valid command (then handle exceptions here)");
               }
          }

          /* note: if I want to add "go back" functionality for any steps in bookRoundTrip(),
           * I will need to break bookRoundTrip into a few more methods...NOT DONE HERE
           */
          static void bookRoundTrip()
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
                    payWithDollarsRT();
               }
               else if (payInput == 2)
               {
                    payWithPointsRT();
               }
               else
               {
                    // see note on exceptions in bookAFlight
                    Console.WriteLine("Enter a valid command (then handle exceptions here)");
               }
          }

          static void bookOneWay()
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

          static void payWithDollarsRT()
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

          static void payWithPointsRT()
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

          // CUSTOMER METHODS END HERE

          static void startLoadEngineer()
          {
               Console.WriteLine("Load Engineer");
               Console.WriteLine("1) Add Flight Route");
               Console.WriteLine("2) Manage Flight Routes");
               Console.WriteLine("3) Sign out");
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
                    signOut();
               }
          }

          /* In this method, consider clearing console after user enters user ID and password,
           * save the entered user ID in a variable, then print "Welcome <user ID>" in each start... method1
           */
          static void startUserLogin()
          {
               //start of Intro Screen
               Console.WriteLine("Project Air ");
               Console.WriteLine("1) Login ");
               Console.WriteLine("2) Create Account");
               Console.WriteLine("3) Log Off");
               Console.WriteLine("Enter a number to select option: ");
               int selection = Convert.ToInt32(Console.ReadLine());
               Console.Clear();

               // End of Intro Screen

               // Account Creation Screen
               if (selection == 2)
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

                    if ((usrID != "") && (password != "") && (creditcardnum != ""))
                    {
                         Console.WriteLine("Account Made!");
                    }

               }
               else if (selection == 1) // user login screen
               {
                    string usrID, password;
                    Console.WriteLine("Enter the Login:");
                    Console.WriteLine("User ID: ");
                    usrID = Console.ReadLine();

                    Console.WriteLine("Password: ");
                    password = Console.ReadLine();
                    validateSignIn(usrID, password);        // call validate sign in here

                    /* Commented out Vikram's hard coded sign in
                    if ((usrID == loginUN[0]) && (password == loginPWD[0]))
                    {
                         startLoadEngineer();
                    }
                    else if ((usrID == loginUN[1]) && (password == loginPWD[1]))
                    {
                         startMarkMNG();
                    }
                    else if ((usrID == loginUN[2]) && (password == loginPWD[2]))
                    {
                         startAccoMNG();
                    }
                    else if ((usrID == loginUN[3]) && (password == loginPWD[3]))
                    {
                         startFligMNG();
                    }
                    else if ((usrID == loginUN[4]) && (password == loginPWD[4]))
                    {
                         startCustomer();
                    }
                    */
               }
               else if (selection == 3)
               {
                    Console.WriteLine("Logging off");
                    // end the program
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
                    signOut();
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
                    signOut();
               }
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
                    signOut();
               }
          }

          static void signOut()
          {
               Console.WriteLine("Signing out");
               Thread.Sleep(3000);
               Console.Clear();
               startUserLogin();
          }

          // new method to check the csv file with login data to sign the user in to the account
          static void validateSignIn(String entUserID, String entPass)
          {
               string csvUserID;
               string csvPassword;
               // change filepath to match where your Accounts.csv file resides
               String filePath = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Accounts.csv";

               using (StreamReader reader = new StreamReader(filePath))
               {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                         //Console.WriteLine(line);
                         string[] row = line.Split(',');
                         csvUserID = row[6];
                         csvPassword = row[7];
                         string csvAccType = row[8];

                         if (entUserID == csvUserID)
                         {
                              if (entPass == csvPassword)
                              {
                                   if (csvAccType == "LE")
                                   {
                                        startLoadEngineer();
                                        return;
                                   }
                                   else if (csvAccType == "MM")
                                   {
                                        startMarkMNG();
                                        return;
                                   }
                                   else if (csvAccType == "AM")
                                   {
                                        startAccoMNG();
                                        return;
                                   }
                                   else if (csvAccType == "FM")
                                   {
                                        startFligMNG();
                                        return;
                                   }
                                   else
                                   {
                                        startCustomer();
                                        return;
                                   }
                              }
                              else if (entPass != csvPassword)
                              {
                                   Console.WriteLine("Incorrect password. Try again or create an account.");
                                   startUserLogin();
                                   return;

                              }
                         }
                    }
                    Console.WriteLine("UserID does not exist. Create an account or try again.");
                    startUserLogin();
                    return;
               }

          }

          static void Main(string[] args)
          {

               //UN COMMENT THIS LATER
               startUserLogin();

          }
     }
}
