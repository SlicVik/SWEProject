using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Collections;
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
          //Vikram's filepaths
          /*public static string routesfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Routes - Sheet1.csv";
          public static string routesTZfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\RouteDistWithTZ - Sheet1.csv";
          public static string accfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Accounts - Accounts.csv";
          public static string transactionsfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Transactions.csv";
          public static string bookedFlightsfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\BookedFlightRecords.csv";*/

          //Garrett's filepaths
          /*public static string routesfp = @"C:\Users\knowl\Downloads\Routes - Sheet1.csv";
          public static string routesTZfp = @"C:\Users\knowl\Downloads\RoutesDistWithTZ - Sheet1.csv";
          public static string accfp = @"C:\Users\knowl\Downloads\Accounts - Accounts.csv";
          public static string transactionsfp = @"C:\Users\knowl\Downloads\Transactions.csv";
          public static string bookedFlightsfp = @"C:\Users\knowl\Downloads\BookedFlightRecords.csv";*/

          //Olivia's filepaths
          public static string accfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Accounts.csv";
          public static string transactionsfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Transactions.csv";
          public static string routesfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Routes.csv";
          public static string bookedFlightsfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\BookedFlightRecords.csv";
          public static string routesTZfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\RouteDistWithTZ.csv";


          // Olivia added these because they were needed across multiple methods where it didn't make sense to pass parameters
          // we can restructure this later
          public static string srcAirportCode;
          public static string dstAirportCode;
          public static string deptDate;
          public static string arrDate;
          public static string sysDate;
          public static string sysTime;

          public static string fName;
          public static string lName;
          public static string userID;
          public static string ccnum;

          static string StrToSHAD(string input)
          {

               var algorithm = HashAlgorithm.Create("sha512");
               var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
               string output = BitConverter.ToString(hash);

               return output;
          }

          /*        static string SHADToStr(string input)
               {

                    return input;
               }*/
          static void changePass()
          {
               Console.WriteLine("Enter the userID that you would like to change the password for:");
               string accInput = Console.ReadLine();
               Console.Clear();

               String filePath = accfp; // to change to something else later

               //to check if there is a username that exists that matches the accInput;
               string readUsrID = "", readUsrPass = "";
               bool exists = false;

               StreamReader reader = new StreamReader(filePath);

               using (reader)
               {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         readUsrID = row[6];
                         readUsrPass = row[7];

                         if (accInput == readUsrID) // if the user ID exists then exit the loop
                         {
                              exists = true;
                              break;
                         }
                    }
               }

               reader.Close();

               if (exists == false)
               {
                    Console.WriteLine("Cannot find a UserID to match the one entered.");
                    return;
               }
               else // we know there exists a UsrID and now we want to change it
               {
                    Console.WriteLine("Enter previous password associated with account:");
                    string passInput = Console.ReadLine();

                    if (StrToSHAD(passInput) == readUsrPass) // If the passwords and user IDs are lined up
                    {
                         // allow the user to change the password now
                         // need the code for being able to write to the file
                         Console.WriteLine("Enter the new password:");
                         string newpass = Console.ReadLine();
                         Console.Clear();

                         string path = accfp;

                         List<String> lines = new List<String>();
                         StreamReader reader1 = new StreamReader(path);

                         if (File.Exists(path))
                         {
                              using (reader1)
                              {
                                   String line;

                                   while ((line = reader1.ReadLine()) != null)
                                   {
                                        if (line.Contains(","))
                                        {
                                             String[] split = line.Split(',');

                                             if (split[7].Contains(StrToSHAD(passInput)))
                                             {
                                                  split[7] = StrToSHAD(newpass);
                                                  line = String.Join(",", split);
                                             }
                                        }

                                        lines.Add(line);
                                   }
                              }

                              reader1.Close();

                              StreamWriter writer = new StreamWriter(path, false);

                              using (writer)
                              {
                                   foreach (String line in lines)
                                        writer.WriteLine(line);
                              }
                              writer.Close();
                         }

                         Console.WriteLine("Password changed successfully!");
                         Thread.Sleep(3000);

                    }
                    else
                    {
                         Console.WriteLine("Passwords do not match what the actual password is for User ID");
                         return;
                    }


               }

          }

          static void printBoardingPass()
          {
               //userID is already set so we know what that is. 
               //First go through accounts and get fname, lname and store that.


               String filePath = accfp; // to change to something else later

               StreamReader reader = new StreamReader(filePath);
               string usrfname = "", usrlname = "";

               using (reader)
               {
                    string line;
                    line = reader.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader.ReadLine()) != null)
                    {
                    string[] row = line.Split(',');
                    string readuserID = row[6];

                    if (readuserID == userID) // if the user ID exists then exit the loop
                    {
                         usrfname = row[0];
                         usrlname = row[1];
                    }

                    }
               }
               reader.Close();

               //Second go through transactions and if the name matches for the line, print dates and routenums

               String filePath1 = transactionsfp; // to change to something else later

               StreamReader reader1 = new StreamReader(filePath1);
               Console.WriteLine("Print Boarding Pass ");
               bool first = true;

               using (reader1)
               {
                    string line;
                    line = reader1.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader1.ReadLine()) != null)
                    {
                    first = false;
                    string[] row = line.Split(',');
                    string readfname = row[1];
                    string readlname = row[2];

                    if (readfname == usrfname && readlname == usrlname) // if the user ID exists then exit the loop
                    {
                         Console.WriteLine(row[4] + " " + row[5]);
                    }

                    }
                    if (first)
                    {
                    Console.WriteLine("No flights have been booked for this user!");
                    }
               }
               reader1.Close();

               //ask the user what date and routenumber they want to see the boarding pass for

               Console.WriteLine("Choose a flight by typing in the date and route number that match");
               Console.WriteLine("Date:");
               string usrDate = Console.ReadLine();
               Console.WriteLine("Route Number:");
               string usrRN = Console.ReadLine();

               //Next go through booked and if the date and rn match, get the source, dest, arrival time, dest time, both tzs,

               String filePath2 = bookedFlightsfp; // to change to something else later
               StreamReader reader2 = new StreamReader(filePath2);
               string displayDest = "", displaySource = "", displayAT = "", displayDT = "", displayDTZ = "", displayATZ = "";

               using (reader2)
               {
                    string line;
                    line = reader2.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader2.ReadLine()) != null)
                    {
                    string[] row = line.Split(',');
                    string readDate = row[1];
                    string readRN = row[0];

                    if (readDate == usrDate && readRN == usrRN) // if the user ID exists then exit the loop
                    {
                         displaySource = row[2];
                         displayDest = row[3];
                         displayDT = row[5];
                         displayDTZ = row[6];
                         displayAT = row[7];
                         displayATZ = row[8];
                         break;
                    }

                    }
               }
               reader2.Close();
               //display the get^ and acc num, flight

               Console.Clear();
               Console.WriteLine("BOARDING PASS");
               Console.WriteLine(userID);
               Console.WriteLine(usrfname + " " + usrlname);
               Console.WriteLine("From: " + displaySource + " To: " + displayDest);
               Console.WriteLine("Departure Time: " + displayDT + " " + displayDTZ + " Arrival Time: " + displayAT + " " + displayATZ);
               Thread.Sleep(5000);
               startCustomer();
               return;
          }
          static void startCustomer()
          {
               Console.WriteLine("Project Air");
               Console.WriteLine("1) Book a Flight");
               Console.WriteLine("2) View Account History");
               Console.WriteLine("3) Print Boarding Pass");
               Console.WriteLine("4) Cancel Flight");
               Console.WriteLine("5) Change Password");
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
                    string pointsSpent = "";
                    string pointsEarned = "";
                    //Console.WriteLine("View Account History (we will probably read if from a csv file)");

                    StreamReader reader = new StreamReader(accfp);
                    using (reader)
                    {
                         string line;
                         string[] split;

                         while ((line = reader.ReadLine()) != null)
                         {
                              split = line.Split(',');
                              //Get the amount of points earned and spent by the user.
                              if (split[6].Contains(userID))
                              {
                                   pointsEarned = split[9];
                                   pointsSpent = split[10];
                              }
                         }
                    }
                    reader.Close();

                    Console.Clear();
                    //Print out the statistics
                    Console.WriteLine("Flights booked: ");
                    Console.WriteLine("Flights taken: ");
                    Console.WriteLine("Flights Cancelled: ");
                    Console.WriteLine("Points available: {0}", pointsEarned);
                    Console.WriteLine("Points used: {0}", pointsSpent);
                    //Go back to home screen
                    startCustomer();
               }
               // if 3) Print Boarding Pass
               else if (inputNum == 3)
               {
                    /*
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
                    */
                    printBoardingPass();
                    return;
               }
               // if 4) Cancel a Flight
               else if (inputNum == 4)
               {
                    cancelAFlight();             
               }
               // if 5) Change Account Details
               else if (inputNum == 5)
               {
                    Console.WriteLine("Change Password");
                    changePass();
                    startCustomer();
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
                    return;
               }
               // if book a one-way flight
               else if (input == 2)
               {
                    bookOneWay();
                    return;
               }
               else if (input == 3)
               {
                    startCustomer();
                    return;
               }
               else
               {
                    // MUST ADD
                    // print error message, then call bookAFlight()
                    // this isn't a perfect idea though, due to what prints out
                    Console.WriteLine("Enter a valid command");
                    bookAFlight();
                    return;
               }
          }

          static void cancelAFlight()
          {
               List<string> lines = new List<string>();
               StreamReader reader1 = new StreamReader(bookedFlightsfp);

               //Used to track the times gathered from the scheduled flights
               string deptDate;
               string deptTime;

               //Used to calculate to see if there is a 1 hour difference.
               int sysTimeInt;
               int deptTimeInt;

               //For incrementing the number of seats
               int seats = 0;

               Console.WriteLine("Enter the Flight Number you wish to cancel");
               string flightToCancel = Console.ReadLine();
               string cancelled = "Cancelled";

               sysDate = DateTime.Now.ToString("M/d/yyyy");         // get system date
               sysTime = DateTime.Now.ToString("h:mm tt");            // and system time
               string dateFormat = "M/D/YYYY";

               using (reader1)
               {
                    string line;

                    while ((line = reader1.ReadLine()) != null)
                    {
                         if (line.Contains(","))
                         {
                              string[] split = line.Split(',');

                              //Check if the flight is actually in the list of transactions
                              if (split[0].Contains(flightToCancel))
                              {
                                   //If the date is the same, we 
                                   if (split[1].Contains(sysDate))
                                   {
                                        sysTimeInt = Convert.ToInt32(sysTime);
                                        deptTimeInt = Convert.ToInt32(split[5]);
                                        seats = Convert.ToInt32(split[10]);

                                        if (deptTimeInt - sysTimeInt <= 1)
                                        {
                                             Console.Clear();
                                             Console.WriteLine("You can't cancel flights that are departing in an hour or less.");
                                             cancelAFlight();
                                             return;
                                        }
                                   }
                                   lines.Add(line);
                              }
                         }
                    }
               }
               reader1.Close();

               StreamWriter writer1 = new StreamWriter(bookedFlightsfp, false);

               seats++;
               string seatsString = seats.ToString();

               //For appending we don't need to use foreach if we can just append one line.
               using (writer1)
               {
                    foreach (string line in lines)
                    {
                         writer1.WriteLine(line);
                    }
                    for (int i = 0; i < lines.Count; i++)
                    {
                         if (lines[i].Contains(flightToCancel))
                         {
                              //Add our incremented seat value here if this works.
                         }
                    }
               }
               writer1.Close();
          }

          /* note: if I want to add "go back" functionality for any steps in bookRoundTrip(),
          * I will need to break bookRoundTrip into a few more methods...NOT DONE HERE
          */
          static void bookRoundTrip()
          {
               //store airports in an array
               string[] airportCode;
               airportCode = new string[12] { "BNA", "CLE", "DEN", "DFW", "DTW", "LAS", "LAX", "LGA", "MCO", "ORD", "PHX", "SEA" };
               Console.WriteLine("Project Air");
               Console.WriteLine("Select a source airport");
               Console.WriteLine("1) BNA");
               Console.WriteLine("2) CLE");
               Console.WriteLine("3) DEN");
               Console.WriteLine("4) DFW");
               Console.WriteLine("5) DTW");
               Console.WriteLine("6) LAS");
               Console.WriteLine("7) LAX");
               Console.WriteLine("8) LGA");
               Console.WriteLine("9) MCO");
               Console.WriteLine("10) ORD");
               Console.WriteLine("11) PHX");
               Console.WriteLine("12) SEA");
               Console.WriteLine("0) Go back");
               Console.WriteLine("Enter a number to select an option");
               int srcAirInput = Convert.ToInt32(Console.ReadLine());

               // to go back...
               if (srcAirInput == 0)
               {
                    Console.Clear();
                    bookAFlight();
                    return;
               }
               // else if the source was incorrectly entered
               else if ((srcAirInput > 12 || srcAirInput < 1) && srcAirInput != 0)
               {
                    Console.Clear();
                    Console.WriteLine("Invalid source airport selection. Try again");
                    bookRoundTrip();
                    return;
               }
               else if (srcAirInput >= 1 && srcAirInput <= 12)
               {
                    srcAirportCode = airportCode[srcAirInput - 1];  // subtract 1 to index, store code to read csv
                    Console.Clear();
                    Console.WriteLine("Source airport: {0}", srcAirportCode);
                    selectDestRT(srcAirInput);
                    return;
               }
          }

          static void selectDestRT(int src)
          {
               //store airports in an array
               string[] airportCode;
               airportCode = new string[12] { "BNA", "CLE", "DEN", "DFW", "DTW", "LAS", "LAX", "LGA", "MCO", "ORD", "PHX", "SEA" };
               Console.WriteLine("Select a destination airport");
               Console.WriteLine("1) BNA");
               Console.WriteLine("2) CLE");
               Console.WriteLine("3) DEN");
               Console.WriteLine("4) DFW");
               Console.WriteLine("5) DTW");
               Console.WriteLine("6) LAS");
               Console.WriteLine("7) LAX");
               Console.WriteLine("8) LGA");
               Console.WriteLine("9) MCO");
               Console.WriteLine("10) ORD");
               Console.WriteLine("11) PHX");
               Console.WriteLine("12) SEA");
               Console.WriteLine("0) Go back");
               Console.WriteLine("Enter a number to select an option");
               int destAirInput = Convert.ToInt32(Console.ReadLine());
               Console.Clear();

               // to go back...
               if (destAirInput == 0)
               {
                    Console.Clear();
                    bookRoundTrip();
                    return;
               }
               else if (src == destAirInput)
               {
                    Console.WriteLine("Destination airport cannot be the same as the source airport. Try again");
                    selectDestRT(src);
                    return;
               }
               else if ((destAirInput > 12 || destAirInput < 1) && destAirInput != 0)
               {
                    Console.Clear();
                    Console.WriteLine("Invalid source airport selection. Try again");
                    selectDestRT(src);
                    return;
               }
               else if (destAirInput >= 1 && destAirInput <= 12)
               {
                    dstAirportCode = airportCode[destAirInput - 1];
                    Console.WriteLine("{0}", dstAirportCode);
                    roundTripDepDate();
                    return;
               }
          }

          static void roundTripDepDate()
          {
               Console.WriteLine("Enter date of departure in the format M/D/YYYY. Flights may be booked up to 6 months in advance"); ;
               string depDate = Console.ReadLine();

               // code to validate date
               DateTime validDepDate;
               string format = "M/d/yyyy";
               if (DateTime.TryParseExact(depDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out validDepDate))
               {
                    // date entered is a valid date...now check if it is a valid booking date
                    string[] depDateTemp = depDate.Split('/');
                    int.TryParse(depDateTemp[0], out int depMonth);   // get the departure month
                    int.TryParse(depDateTemp[1], out int depDay);     // get the departure day

                    sysDate = DateTime.Now.ToString("M/d/yyyy");         // get system date
                    sysTime = DateTime.Now.ToString("h:mm tt");            // and system time
                    string[] dateTemp = sysDate.Split('/');
                    int sysMonth, sysDay;
                    int.TryParse(dateTemp[0], out sysMonth);          // get system month
                    int.TryParse(dateTemp[1], out sysDay);            // get system day

                    DateTime sysDateDT;
                    DateTime.TryParseExact(sysDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out sysDateDT);        // convert to DateTime
                    int ret1 = DateTime.Compare(validDepDate, sysDateDT);          // compare the user entered date to today's date

                    if (ret1 < 0)
                    {
                         Console.WriteLine("Invalid date: {0} has already passed. Try another date on or after {1}", depDate, sysDate);
                         oneWayDate();
                         return;
                    }
                    else if (ret1 == 0)      // if the user is booking a flight for today
                    {
                         deptDate = depDate;
                         //displayOneWay();
                         // diplay flights departing in less than 1 hour
                         // if no flights
                         // display error message
                         // call oneWayDate();
                         // return;
                         // end if

                         // let user select a flight
                    }

                    DateTime maxDateDT = sysDateDT.AddMonths(6);
                    // compare system and user enter dates
                    int ret2 = DateTime.Compare(validDepDate, maxDateDT);     // compare the user entered date to the latest date they can book
                    string maxDate = maxDateDT.ToShortDateString();

                    if (ret2 > 0)       // if the user entered date is > 6 mos out
                    {
                         Console.WriteLine("Invalid date: {0} is more than 6 months in advance. Try another between {1} and {2}", depDate, sysDate, maxDate);
                         oneWayDate();
                         return;
                    }
                    else if (ret2 == 0 || ret2 < 0)         // if the user entered date is earlier or on the latest date available to book
                    {
                         deptDate = depDate;
                         roundTripArrDate();
                         // diplay flights
                         // if no flights (rare unless we add a ton of test data)
                         // display error message
                         // call oneWayDate();
                         // return;
                         // end if

                         // let user select a flight
                    }
               }
               else           // the entered date is not valid
               {
                    Console.Clear();
                    Console.WriteLine("{0} is not a valid date. Try again.", depDate);
                    oneWayDate();
                    return;
               }
          }

          static void roundTripArrDate()
          {
               Console.WriteLine("Enter date of arrival in the format M/D/YYYY. Flights may be booked up to 6 months in advance"); ;
               string arrivDate = Console.ReadLine();

               // code to validate date
               DateTime validDate;
               string format = "M/d/yyyy";
               if (DateTime.TryParseExact(arrivDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out validDate))
               {
                    // date entered is a valid date...now check if it is a valid booking date
                    string[] depDateTemp = arrivDate.Split('/');
                    int.TryParse(depDateTemp[0], out int depMonth);   // get the departure month
                    int.TryParse(depDateTemp[1], out int depDay);     // get the departure day

                    sysDate = DateTime.Now.ToString("M/d/yyyy");         // get system date
                    sysTime = DateTime.Now.ToString("h:mm tt");            // and system time
                    string[] dateTemp = sysDate.Split('/');
                    int sysMonth, sysDay;
                    int.TryParse(dateTemp[0], out sysMonth);          // get system month
                    int.TryParse(dateTemp[1], out sysDay);            // get system day

                    DateTime sysDateDT;
                    DateTime.TryParseExact(sysDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out sysDateDT);        // convert to DateTime
                    int ret1 = DateTime.Compare(validDate, sysDateDT);          // compare the user entered date to today's date

                    if (ret1 < 0)
                    {
                         Console.WriteLine("Invalid date: {0} has already passed. Try another date on or after {1}", arrivDate, sysDate);
                         roundTripArrDate();
                         return;
                    }
                    else if (ret1 == 0)      // if the user is booking a flight for today
                    {
                         arrDate = arrivDate;

                         //displayOneWay();
                         // diplay flights departing in less than 1 hour
                         // if no flights
                         // display error message
                         // call oneWayDate();
                         // return;
                         // end if

                         // let user select a flight
                    }

                    DateTime maxDateDT = sysDateDT.AddMonths(6);;
                    // compare system and user enter dates
                    int ret2 = DateTime.Compare(validDate, maxDateDT);     // compare the user entered date to the latest date they can book
                    string maxDate = maxDateDT.ToShortDateString();

                    if (ret2 > 0)       // if the user entered date is > 6 mos out
                    {
                         Console.WriteLine("Invalid date: {0} is more than 6 months in advance. Try another between {1} and {2}", arrivDate, sysDate, maxDate);
                         roundTripArrDate();
                         return;
                    }
                    else if (ret2 == 0 || ret2 < 0)         // if the user entered date is earlier or on the latest date available to book
                    {
                         arrDate = arrivDate;
                         DateTime deptDT;
                         DateTime.TryParseExact(deptDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out deptDT);        // convert to DateTime
                         int ret3 = DateTime.Compare(validDate, deptDT);
                         if (ret3 <= 0)
                         {
                              Console.WriteLine("Invalid date: {0} is before or on the departure date. Try another between {1} and {2}.", arrivDate, deptDT, maxDate);
                              roundTripArrDate();
                              return;
                         }
                         else if (ret3 > 0)
                         {
                              displayRTOB();
                         }
                    }
               }
               else           // the entered date is not valid
               {
                    Console.Clear();
                    Console.WriteLine("{0} is not a valid date. Try again.", arrivDate);
                    oneWayDate();
                    return;
               }
          }

          static void displayRTOB()
          {
               string srcAP;       //these strings recognize the airport codes in the file
               string destAP;
               string dTime;
               string aTime;
               bool isNotFull = false;
               bool isNotFull1 = false;
               bool isNotFull2 = false;
               bool isNotFull3 = false;
               bool recordExists = false;
               bool record1Exists = false;
               bool record2Exists = false;
               bool record3Exists = false;
               string flightNumber;
               int userSelFlight;
               int tempConnectFlightCount = 0;
               int tempConnCountThree = 0;
               List<double> prices = new List<double>();
               List<double> points = new List<double>();
               //string departDate; TO UPDATE FLIGHT RECORD LATER
               List<string> directRoutes = new List<string>();        // store all valid direct src/dest combos in this array before more checks
               List<string> displayTracker = new List<string>();      // used to keep track of which flight the customer selects to book, so the CSVs can be updated
               List<string> tempConnectFlights = new List<string>();  // used to store a connecting flight to test
               List<string> finalConnectFlights = new List<string>();      // to store valid connections
               List<string> tempThreeLegFlight = new List<string>();       // used to store candidates for a 3 leg flight
               List<string> finalThreeLegFlight = new List<string>();      // used to store actual 3 leg flights
                                                                           //string candidateFlight;       // keeps the departure date and flight number

               // reads direct flights and stores them
               StreamReader routeReader = new StreamReader(routesfp);
               using (routeReader)
               {
                    string line;

                    while ((line = routeReader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];

                         // if the src and dest match
                         if (srcAP == srcAirportCode && destAP == dstAirportCode)
                         {
                              directRoutes.Add(line);
                         }
                    }
               }
               routeReader.Close();

               // read for indirect flights, using a different mechanism
               // gets the first leg of a possible connecting flight
               StreamReader routeReader2 = new StreamReader(routesfp);
               using (routeReader2)
               {
                    string line;
                    while ((line = routeReader2.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];

                         if (srcAP == srcAirportCode && destAP != dstAirportCode)
                         {
                              tempConnectFlightCount++;
                              tempConnectFlights.Add(line);
                         }
                    }
               }
               routeReader2.Close();

               // HANDLES 2nd part of connection
               StreamReader routeReader3 = new StreamReader(routesfp);
               using (routeReader3)
               {
                    // this allows us to test all possible first legs in the temp connect flights array, however there may be a larger number of
                    // flights generated than the number of candidate first legs, which the while loop allows for
                    for (int j = 0; j < tempConnectFlightCount; j++)
                    {
                         string currentRoute = tempConnectFlights[j];
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[2];
                         string line;
                         while ((line = routeReader3.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == dstAirportCode)       // if we found the connection for a possible 2 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival+40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                   {
                                        string twoLegRoute = currentRoute + ',' + line;
                                        finalConnectFlights.Add(twoLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {

                                   }
                              }
                              else if (testSplit[1] == routeDest && testSplit[2] != dstAirportCode)
                              {
                                   // here we check to see if the flight in question leaves in the PM and arrives in the AM
                                   // if it does, we will not add this to our new list because we want cutomers
                                   // to leave and arrive on the same day
                                   string timeFormat = "h:mm tt";
                                   DateTime tempDep;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempDep);
                                   DateTime tempArr;
                                   DateTime.TryParseExact(testSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempArr);
                                   int retNextDay = DateTime.Compare(tempDep, tempArr);
                                   if (retNextDay < 0)
                                   {
                                        // then we test timing
                                        // convert testSplit[4] to dateTime
                                        //string timeFormat = "h:mm tt";
                                        DateTime testTime;
                                        DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                        // convert arrival time to datetime
                                        DateTime firstLegTime;
                                        DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                        // add 40 mins to arrival
                                        DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                        // compare testSplit[4] to arrival+40 mins
                                        int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                        // if ret > 0 then we store in a final list<string>
                                        if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                        {
                                             string twoLegRoute = currentRoute + ',' + line;
                                             tempThreeLegFlight.Add(twoLegRoute);
                                             tempConnCountThree++;
                                        }
                                   }
                              }

                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader3.DiscardBufferedData();
                         routeReader3.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }
               routeReader3.Close();

               // now we find 3rd connections
               StreamReader routeReader4 = new StreamReader(routesfp);
               using (routeReader4)
               {
                    for (int j = 0; j < tempConnCountThree; j++)
                    {
                         string currentRoute = tempThreeLegFlight[j];
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[11];
                         string line;

                         while ((line = routeReader4.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == dstAirportCode)       // if we found the connection for a possible 3 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[15], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival + 40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 3 LEG CONNECTION
                                   {
                                        string threeLegRoute = currentRoute + ',' + line;
                                        finalThreeLegFlight.Add(threeLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {
                                        // nothing
                                   }
                              }
                              else
                              {
                                   // do nothing
                              }
                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader4.DiscardBufferedData();
                         routeReader4.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }

               int i = 1;
               foreach (string line in directRoutes)
               {
                    string[] split = line.Split(',');
                    string candidateFlight;       // keeps the departure date and flight number

                    // need to check the flights file to see if it exists for this date 
                    // check flight number and deptDate
                    recordExists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (recordExists == true)
                    {
                         //we don't have to generate a flight number, but we need to retrieve it
                         // we need to check if the plane is full
                         isNotFull = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         // if plane has seats
                         if (isNotFull == true)
                         {
                              // we will display this flight
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                              candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(candidateFlight);     // add to list
                              i++;      // increment for next
                         }
                    }
                    // if record does not exist
                    else if (recordExists == false)
                    {
                         // the plane is empty, so display this flight
                         Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                         candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(candidateFlight);     // add to list
                         i++;      // increment for next
                    }
               }

               foreach (string line in finalConnectFlights)
               {
                    string[] split = line.Split(',');
                    string cand2LegFlight;       // keeps the departure date and flight number

                    record1Exists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         if (isNotFull1 == true)
                         {
                              record2Exists = flightHasRecord(split[9], deptDate);
                              if (record2Exists == true)
                              {
                                   isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                                   if (isNotFull2 == true)
                                   {
                                        // display both
                                        // we will display this flight
                                        Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                             split[11], split[13], split[15]);
                                        cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                        displayTracker.Add(cand2LegFlight);     // add to list
                                        i++;
                                   }
                                   else
                                   {
                                        // leg 2 is full, DON'T DISPLAY
                                   }
                              }
                              else
                              {
                                   // leg2 is also open, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                         }
                         else
                         {
                              // DON'T DISPLAY
                         }
                    }
                    else
                    {
                         // leg1 doesn't have a record, check record 2
                         record2Exists = flightHasRecord(split[9], deptDate);
                         if (record2Exists == true)
                         {
                              isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                              if (isNotFull2 == true)
                              {
                                   // both legs have seats, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                              else
                              {
                                   // don't display
                              }
                         }
                         else
                         {
                              // both records do not exist, so both are available, display
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                   split[11], split[13], split[15]);
                              cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(cand2LegFlight);     // add to list
                              i++;
                         }
                    }
               }

               foreach (string line in finalThreeLegFlight)
               {
                    string[] split = line.Split(',');
                    string cand3LegFlight;       // keeps the departure date and index with everything
                    record1Exists = flightHasRecord(split[0], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);
                    }
                    record2Exists = flightHasRecord(split[9], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull2 = checkSeats(split[9], deptDate);
                    }
                    record3Exists = flightHasRecord(split[18], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull3 = checkSeats(split[18], deptDate);
                    }

                    if ((isNotFull1 == true || record1Exists == false) && (isNotFull2 == true || record2Exists == false) && (isNotFull3 == true || record3Exists == false))
                    {
                         // all 3 legs have seats, display
                         //Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                         //split[11], split[13], split[15]);
                         Console.WriteLine("{0}) {1} - {2} - {3} - {4}     {5} to {6}", i, split[1], split[10], split[19], split[20], split[4], split[24]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[1], split[2], split[4], split[6]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[10], split[11], split[13], split[15]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[19], split[20], split[22], split[24]);
                         cand3LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(cand3LegFlight);     // add to list
                         i++;
                    }
               }

               // if all flights are booked for this day (highly unlikely, nearly impossible)
               if (displayTracker.Count == 0)
               {
                    Console.WriteLine("No flights available on this day. Try another date.");
                    roundTripArrDate();
                    return;
               }
               else if (displayTracker.Count != 0)
               {
                    Console.WriteLine("Select an outbound flight");
                    userSelFlight = Convert.ToInt32(Console.ReadLine());

                    foreach (string candidateFlight in displayTracker)
                    {
                         string[] canSplit = candidateFlight.Split(',');
                         double ticketPrice = 0;
                         double runningTotalPrice = 0;
                         double tempPoints = 0;
                         double runningTotalPoints = 0;
                         int storedOptNum = Convert.ToInt32((string)canSplit[0]);
                         // if we found the flight the cust chose to book
                         if (storedOptNum == userSelFlight)
                         {
                              int totalElements = canSplit.Count();        // get number of elements in array to see how many connections it has
                              int numLoops = (totalElements - 2) / 9;
                              for (int a = 0; a < numLoops; a++)
                              {
                                   int APDist = Convert.ToInt32((string)canSplit[(a * 9) + 5]);
                                   ticketPrice = 58 + (0.12 * APDist); // base calculation

                                   // check times for discounts
                                   string tempDepTime = canSplit[(a * 9) + 6];
                                   string tempArrTime = canSplit[(a * 9) + 8];
                                   string timeFormat = "h:mm tt";
                                   string string8 = "8:00 AM";
                                   string string7 = "7:00 PM";
                                   string string12 = "12:00 AM";
                                   string string5 = "5:00 AM";
                                   DateTime depTimeDT, arrTimeDT, string8DT, string7DT, string12DT, string5DT;
                                   DateTime.TryParseExact(tempDepTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out depTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(tempArrTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out arrTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(string8, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string8DT);        // convert to DateTime
                                   DateTime.TryParseExact(string7, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string7DT);        // convert to DateTime
                                   DateTime.TryParseExact(string12, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string12DT);        // convert to DateTime
                                   DateTime.TryParseExact(string5, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string5DT);        // convert to DateTime

                                   int ret8AM = DateTime.Compare(depTimeDT, string8DT);
                                   int ret7PM = DateTime.Compare(arrTimeDT, string7DT);
                                   int ret12AMDepart = DateTime.Compare(depTimeDT, string12DT);
                                   int ret12AMArrive = DateTime.Compare(arrTimeDT, string12DT);
                                   int ret5AMDepart = DateTime.Compare(depTimeDT, string5DT);
                                   int ret5AMArrive = DateTime.Compare(arrTimeDT, string5DT);

                                   if ((ret12AMDepart >= 0 && ret5AMDepart <= 0) || (ret12AMArrive >= 0 && ret5AMArrive <= 0))
                                   {
                                        ticketPrice = ticketPrice * 0.8;
                                   }
                                   else if (ret5AMDepart > 0 && ret8AM < 0 || ret7PM > 0) // departing before 8am or arriving after 7pm
                                   {
                                        ticketPrice = ticketPrice * 0.9;        // 10 percent off
                                   }
                                   // no else case, the else is already done
                                   ticketPrice = Math.Round(ticketPrice, 2);
                                   prices.Add(ticketPrice);      // add ticket price to the array
                                   runningTotalPrice += ticketPrice;
                                   tempPoints = ticketPrice * 100;
                                   points.Add(tempPoints);
                                   runningTotalPoints += tempPoints;
                              }
                              //runningTotalPrice = Math.Round(runningTotalPrice, 2);
                              prices.Add(runningTotalPrice);
                              points.Add(runningTotalPoints);
                              displayRTIB(candidateFlight, prices, points);
                         }
                    }
               }
          }

          static void displayRTIB(string OBFlightString, List<double> obCost, List<double> obPointCost)
          {
               string srcAP;       //these strings recoginze the airport codes in the file
               string destAP;
               string dTime;
               string aTime;
               bool isNotFull = false;
               bool isNotFull1 = false;
               bool isNotFull2 = false;
               bool isNotFull3 = false;
               bool recordExists = false;
               bool record1Exists = false;
               bool record2Exists = false;
               bool record3Exists = false;
               string flightNumber;
               int userSelIBFlight;
               int tempConnectFlightCount = 0;
               //int tempIndex = 0;
               int tempConnCountThree = 0;
               List<double> prices = new List<double>();
               List<double> points = new List<double>();
               //string departDate; TO UPDATE FLIGHT RECORD LATER
               List<string> directRoutes = new List<string>();        // store all valid direct src/dest combos in this array before more checks
               List<string> displayTracker = new List<string>();      // used to keep track of which flight the customer selects to book, so the CSVs can be updated
               List<string> tempConnectFlights = new List<string>();  // used to store a connecting flight to test
               List<string> finalConnectFlights = new List<string>();      // to store valid connections
               List<string> tempThreeLegFlight = new List<string>();       // used to store candidates for a 3 leg flight
               List<string> finalThreeLegFlight = new List<string>();      // used to store actual 3 leg flights
                                                                           //string candidateFlight;       // keeps the departure date and flight number

               // reads direct flights and stores them
               StreamReader routeReader = new StreamReader(routesfp);
               using (routeReader)
               {
                    string line;

                    while ((line = routeReader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];

                         // if the src and dest match
                         if (srcAP == dstAirportCode && destAP == srcAirportCode)
                         {
                              directRoutes.Add(line);
                         }
                    }
               }
               routeReader.Close();

               // read for indirect flights, using a different mechanism
               // gets the first leg of a possible connecting flight
               StreamReader routeReader2 = new StreamReader(routesfp);
               using (routeReader2)
               {
                    string line;
                    while ((line = routeReader2.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];

                         if (srcAP == dstAirportCode && destAP != srcAirportCode)
                         {
                              tempConnectFlightCount++;
                              tempConnectFlights.Add(line);
                         }
                    }
               }
               routeReader2.Close();

               // HANDLES 2nd part of connection
               StreamReader routeReader3 = new StreamReader(routesfp);
               using (routeReader3)
               {
                    // this allows us to test all possible first legs in the temp connect flights array, however there may be a larger number of
                    // flights generated than the number of candidate first legs, which the while loop allows for
                    for (int j = 0; j < tempConnectFlightCount; j++)
                    {
                         string currentRoute = tempConnectFlights[j];
                         //Console.WriteLine("string at first index: {0}", currentRoute);
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[2];
                         string line;
                         while ((line = routeReader3.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == srcAirportCode)       // if we found the connection for a possible 2 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival+40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                   {
                                        string twoLegRoute = currentRoute + ',' + line;
                                        finalConnectFlights.Add(twoLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {

                                   }
                              }
                              else if (testSplit[1] == routeDest && testSplit[2] != srcAirportCode)
                              {
                                   // here we check to see if the flight in question leaves in the PM and arrives in the AM
                                   // if it does, we will not add this to our new list because we want cutomers
                                   // to leave and arrive on the same day
                                   string timeFormat = "h:mm tt";
                                   DateTime tempDep;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempDep);
                                   DateTime tempArr;
                                   DateTime.TryParseExact(testSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempArr);
                                   int retNextDay = DateTime.Compare(tempDep, tempArr);
                                   if (retNextDay < 0)
                                   {
                                        // then we test timing
                                        // convert testSplit[4] to dateTime
                                        //string timeFormat = "h:mm tt";
                                        DateTime testTime;
                                        DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                        // convert arrival time to datetime
                                        DateTime firstLegTime;
                                        DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                        // add 40 mins to arrival
                                        DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                        // compare testSplit[4] to arrival+40 mins
                                        int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                        // if ret > 0 then we store in a final list<string>
                                        if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                        {
                                             string twoLegRoute = currentRoute + ',' + line;
                                             tempThreeLegFlight.Add(twoLegRoute);
                                             tempConnCountThree++;
                                        }
                                   }
                              }

                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader3.DiscardBufferedData();
                         routeReader3.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }
               routeReader3.Close();

               // now we find 3rd connections
               StreamReader routeReader4 = new StreamReader(routesfp);
               using (routeReader4)
               {
                    for (int j = 0; j < tempConnCountThree; j++)
                    {
                         string currentRoute = tempThreeLegFlight[j];
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[11];
                         string line;

                         while ((line = routeReader4.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == srcAirportCode)       // if we found the connection for a possible 3 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[15], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival + 40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 3 LEG CONNECTION
                                   {
                                        string threeLegRoute = currentRoute + ',' + line;
                                        finalThreeLegFlight.Add(threeLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {
                                        // nothing
                                   }
                              }
                              else
                              {
                                   // do nothing
                              }
                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader4.DiscardBufferedData();
                         routeReader4.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }

               int i = 1;
               foreach (string line in directRoutes)
               {
                    string[] split = line.Split(',');
                    string candidateFlight;       // keeps the departure date and flight number

                    // need to check the flights file to see if it exists for this date 
                    // check flight number and deptDate
                    recordExists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (recordExists == true)
                    {
                         //we don't have to generate a flight number, but we need to retrieve it
                         // we need to check if the plane is full
                         isNotFull = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         // if plane has seats
                         if (isNotFull == true)
                         {
                              // we will display this flight
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                              candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(candidateFlight);     // add to list
                              i++;      // increment for next
                         }
                    }
                    // if record does not exist
                    else if (recordExists == false)
                    {
                         // the plane is empty, so display this flight
                         Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                         candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(candidateFlight);     // add to list
                         i++;      // increment for next
                    }
               }

               foreach (string line in finalConnectFlights)
               {
                    string[] split = line.Split(',');
                    string cand2LegFlight;       // keeps the departure date and flight number

                    record1Exists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         if (isNotFull1 == true)
                         {
                              record2Exists = flightHasRecord(split[9], deptDate);
                              if (record2Exists == true)
                              {
                                   isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                                   if (isNotFull2 == true)
                                   {
                                        // display both
                                        // we will display this flight
                                        Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                             split[11], split[13], split[15]);
                                        cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                        displayTracker.Add(cand2LegFlight);     // add to list
                                        i++;
                                   }
                                   else
                                   {
                                        // leg 2 is full, DON'T DISPLAY
                                   }
                              }
                              else
                              {
                                   // leg2 is also open, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                         }
                         else
                         {
                              // DON'T DISPLAY
                         }
                    }
                    else
                    {
                         // leg1 doesn't have a record, check record 2
                         record2Exists = flightHasRecord(split[9], deptDate);
                         if (record2Exists == true)
                         {
                              isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                              if (isNotFull2 == true)
                              {
                                   // both legs have seats, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                              else
                              {
                                   // don't display
                              }
                         }
                         else
                         {
                              // both records do not exist, so both are available, display
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                   split[11], split[13], split[15]);
                              cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(cand2LegFlight);     // add to list
                              i++;
                         }
                    }
               }

               foreach (string line in finalThreeLegFlight)
               {
                    string[] split = line.Split(',');
                    string cand3LegFlight;       // keeps the departure date and index with everything
                    record1Exists = flightHasRecord(split[0], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);
                    }
                    record2Exists = flightHasRecord(split[9], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull2 = checkSeats(split[9], deptDate);
                    }
                    record3Exists = flightHasRecord(split[18], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull3 = checkSeats(split[18], deptDate);
                    }

                    if ((isNotFull1 == true || record1Exists == false) && (isNotFull2 == true || record2Exists == false) && (isNotFull3 == true || record3Exists == false))
                    {
                         // all 3 legs have seats, display
                         //Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                         //split[11], split[13], split[15]);
                         Console.WriteLine("{0}) {1} - {2} - {3} - {4}     {5} to {6}", i, split[1], split[10], split[19], split[20], split[4], split[24]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[1], split[2], split[4], split[6]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[10], split[11], split[13], split[15]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[19], split[20], split[22], split[24]);
                         cand3LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(cand3LegFlight);     // add to list
                         i++;
                    }
               }

               // if all flights are booked for this day (highly unlikely, nearly impossible)
               if (displayTracker.Count == 0)
               {
                    Console.WriteLine("No flights available on this day. Try another date.");
                    oneWayDate();
                    return;
               }
               else if (displayTracker.Count != 0)
               {
                    Console.WriteLine("Select an inbound flight");
                    userSelIBFlight = Convert.ToInt32(Console.ReadLine());
                    foreach (string IB in displayTracker)
                    {
                         string[] canSplit = IB.Split(',');
                         double ticketPrice = 0;
                         double runningTotalPrice = 0;
                         double tempPoints = 0;
                         double runningTotalPoints = 0;
                         int storedOptNum = Convert.ToInt32((string)canSplit[0]);
                         // if we found the flight the cust chose to book
                         if (storedOptNum == userSelIBFlight)
                         {
                              int totalElements = canSplit.Count();        // get number of elements in array to see how many connections it has
                              int numLoops = (totalElements - 2) / 9;
                              for (int a = 0; a < numLoops; a++)
                              {
                                   int APDist = Convert.ToInt32((string)canSplit[(a * 9) + 5]);
                                   ticketPrice = 58 + (0.12 * APDist); // base calculation

                                   // check times for discounts
                                   string tempDepTime = canSplit[(a * 9) + 6];
                                   string tempArrTime = canSplit[(a * 9) + 8];
                                   string timeFormat = "h:mm tt";
                                   string string8 = "8:00 AM";
                                   string string7 = "7:00 PM";
                                   string string12 = "12:00 AM";
                                   string string5 = "5:00 AM";
                                   DateTime depTimeDT, arrTimeDT, string8DT, string7DT, string12DT, string5DT;
                                   DateTime.TryParseExact(tempDepTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out depTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(tempArrTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out arrTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(string8, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string8DT);        // convert to DateTime
                                   DateTime.TryParseExact(string7, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string7DT);        // convert to DateTime
                                   DateTime.TryParseExact(string12, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string12DT);        // convert to DateTime
                                   DateTime.TryParseExact(string5, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string5DT);        // convert to DateTime

                                   int ret8AM = DateTime.Compare(depTimeDT, string8DT);
                                   int ret7PM = DateTime.Compare(arrTimeDT, string7DT);
                                   int ret12AMDepart = DateTime.Compare(depTimeDT, string12DT);
                                   int ret12AMArrive = DateTime.Compare(arrTimeDT, string12DT);
                                   int ret5AMDepart = DateTime.Compare(depTimeDT, string5DT);
                                   int ret5AMArrive = DateTime.Compare(arrTimeDT, string5DT);

                                   if ((ret12AMDepart >= 0 && ret5AMDepart <= 0) || (ret12AMArrive >= 0 && ret5AMArrive <= 0))
                                   {
                                        ticketPrice = ticketPrice * 0.8;
                                   }
                                   else if (ret5AMDepart > 0 && ret8AM < 0 || ret7PM > 0) // departing before 8am or arriving after 7pm
                                   {
                                        ticketPrice = ticketPrice * 0.9;        // 10 percent off
                                   }
                                   // no else case, the else is already done
                                   ticketPrice = Math.Round(ticketPrice, 2);
                                   prices.Add(ticketPrice);      // add ticket price to the array
                                   runningTotalPrice += ticketPrice;
                                   tempPoints = ticketPrice * 100;
                                   points.Add(tempPoints);
                                   runningTotalPoints += tempPoints;
                              }
                              //runningTotalPrice = Math.Round(runningTotalPrice, 2);
                              prices.Add(runningTotalPrice);
                              points.Add(runningTotalPoints);

                              // NEW TO ROUNDTRIP
                              string OBroute = OBFlightString;
                              List<double> OBCost = new List<double>(obCost);
                              List<double> OBPoints = new List<double>(obPointCost);
                              int lastCostIndex = OBCost.Count - 1;
                              int lastPointsIndex = OBPoints.Count - 1;

                              double rtCost = OBCost[lastCostIndex] + runningTotalPrice;
                              double rtPoints = OBPoints[lastPointsIndex] + runningTotalPoints;

                              // ticketPrice = Math.Round(ticketPrice, 2);
                              // double ticketPricePoints = ticketPrice * 100;
                              Console.WriteLine("Flight price: ${0} or {1} points", rtCost, rtPoints);
                              Console.WriteLine("Select an payment option");
                              Console.WriteLine("1) Pay with dollars");
                              Console.WriteLine("2) Pay with points");
                              Console.WriteLine("Enter a number to select an option");
                              int payInput = Convert.ToInt32(Console.ReadLine());

                              // separate this out later...to handle case of invalid input
                              if (payInput == 1)
                              {
                                   payWithDollarsRT(OBFlightString, OBCost, IB, prices);
                                   return;
                              }
                              else if (payInput == 2)
                              {
                                   payWithPointsRT(OBFlightString, OBCost, OBPoints, IB, prices, points);
                                   return;
                              }
                         }
                    }
               }
          }

          static void payWithDollarsRT(string obFlightData, List<double> obCost, string ibFlightData, List<double> ibCost)
          {
               int lastOBIndex = obCost.Count() - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               int lastBFN = 0;         // hold's the last booked flight number in transactions
               bool recordExists = false;
               string[] obSplit = obFlightData.Split(',');
               Console.WriteLine("Booking Details:");
               Console.WriteLine("Outbound Flight:");
               if (lastOBIndex == 1)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
               }
               else if (lastOBIndex == 2)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[11], obSplit[12], obSplit[13], obSplit[15], obSplit[17]);
               }
               else if (lastOBIndex == 3)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[11], obSplit[12], obSplit[13], obSplit[15], obSplit[17]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[20], obSplit[21], obSplit[22], obSplit[24], obSplit[26]);
               }

               int lastIBIndex = ibCost.Count() - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               string[] ibSplit = ibFlightData.Split(',');
               Console.WriteLine("Booking Details:");
               Console.WriteLine("Inbound Flight:");
               if (lastIBIndex == 1)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
               }
               else if (lastIBIndex == 2)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[11], ibSplit[12], ibSplit[13], ibSplit[15], ibSplit[17]);
               }
               else if (lastIBIndex == 3)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[11], ibSplit[12], ibSplit[13], ibSplit[15], ibSplit[17]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[20], ibSplit[21], ibSplit[22], ibSplit[24], ibSplit[26]);
               }

               double totalCost = obCost[lastOBIndex] + ibCost[lastIBIndex];
               Console.WriteLine("Amount due: ${0}", totalCost);
               Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
               string confirm = Console.ReadLine();

               if (confirm == "Y")
               {
                    // let's update all relevant CSV files...
                    // starting with BookedFlightRecords
                    List<string> linesOB = new List<string>();
                    List<string> linesIB = new List<string>();
                    List<string> lines2 = new List<string>();
                    List<string> newFlightLines = new List<string>();
                    List<string> newTransact = new List<string>();

                    // read booked flight records to update seats on existing records for OB flights
                    StreamReader recordReaderOB = new StreamReader(bookedFlightsfp);
                    using (recordReaderOB)
                    {
                         string line;
                         while ((line = recordReaderOB.ReadLine()) != null)
                         {
                              Console.WriteLine(line);
                              string[] splitRecord = line.Split(',');
                              if (lastOBIndex == 1)
                              {

                                   if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastOBIndex == 2)
                              {
                                   if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == obSplit[11] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastOBIndex == 3)
                              {
                                   if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == obSplit[11] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == obSplit[20] && splitRecord[1] == obSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              linesOB.Add(line);
                         }
                    }
                    recordReaderOB.Close();

                    // THEN WE REWRITE THE FILE FOR EXISTING RECORD CHANGES for ob
                    StreamWriter recordWritOB = new StreamWriter(bookedFlightsfp, false);

                    using (recordWritOB)
                    {
                         foreach (string line in linesOB)
                         {
                              recordWritOB.WriteLine(line);
                         }
                    }
                    recordWritOB.Close();

                    // read booked flight records to update seats on existing records for IB flights
                    StreamReader recordReaderIB = new StreamReader(bookedFlightsfp);
                    using (recordReaderIB)
                    {
                         string line;
                         while ((line = recordReaderIB.ReadLine()) != null)
                         {
                              Console.WriteLine(line);
                              string[] splitRecord = line.Split(',');
                              if (lastIBIndex == 1)
                              {

                                   if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastIBIndex == 2)
                              {
                                   if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == ibSplit[11] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastIBIndex == 3)
                              {
                                   if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == ibSplit[11] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == ibSplit[20] && splitRecord[1] == ibSplit[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              linesIB.Add(line);
                         }
                    }
                    recordReaderIB.Close();

                    // THEN WE REWRITE THE FILE FOR EXISTING RECORD CHANGES for ib
                    StreamWriter recordWritIB = new StreamWriter(bookedFlightsfp, false);

                    using (recordWritIB)
                    {
                         foreach (string line in linesIB)
                         {
                              recordWritIB.WriteLine(line);
                         }
                    }
                    recordWritIB.Close();

                    // then we check all of them and add new lines as needed for ob
                    string newLine;
                    for (int i = 0; i < lastOBIndex; i++)
                    {
                         recordExists = flightHasRecord(obSplit[(i * 9) + 2], deptDate);
                         if (recordExists == false)
                         {
                              string[] splitAllFlightData = obFlightData.Split(',');
                              string aircraft = splitAllFlightData[(i * 9) + 10];
                              string numSeatsLeft = "";

                              if (aircraft == "737")
                              {
                                   int seats = 188;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "757")
                              {
                                   int seats = 199;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "787")
                              {
                                   int seats = 241;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                   + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                   + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                              newFlightLines.Add(newLine);
                         }
                    }

                    // then we check all of them and add new lines as needed for ib
                    for (int i = 0; i < lastIBIndex; i++)
                    {
                         recordExists = flightHasRecord(ibSplit[(i * 9) + 2], arrDate);
                         if (recordExists == false)
                         {
                              string[] splitAllFlightData = ibFlightData.Split(',');
                              string aircraft = splitAllFlightData[(i * 9) + 10];
                              string numSeatsLeft = "";

                              if (aircraft == "737")
                              {
                                   int seats = 188;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "757")
                              {
                                   int seats = 199;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "787")
                              {
                                   int seats = 241;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                   + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                   + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                              newFlightLines.Add(newLine);
                         }
                    }

                    StreamReader recordReader2 = new StreamReader(bookedFlightsfp);
                    using (recordReader2)
                    {
                         string line;
                         while ((line = recordReader2.ReadLine()) != null)
                         {
                              lines2.Add(line);
                         }
                    }
                    recordReader2.Close();

                    // for OB and IB
                    StreamWriter recordwriter = new StreamWriter(bookedFlightsfp, false);
                    using (recordwriter)
                    {
                         foreach (string line in lines2)
                         {
                              recordwriter.WriteLine(line);
                         }
                         foreach (string newlines in newFlightLines)
                         {
                              recordwriter.WriteLine(newlines);
                         }
                    }
                    recordwriter.Close();

                    // before we set up the strings that hold our transaction data, we need to find the last
                    //BookedFlightNum in the file
                    StreamReader bfnReader = new StreamReader(transactionsfp);
                    using (bfnReader)
                    {
                         string line;
                         while ((line = bfnReader.ReadLine()) != null)
                         {
                              string[] splitT = line.Split(',');
                              if (splitT[0].Contains("BookedFlightNum"))
                              {
                                   // we don't do anything
                              }
                              else
                              {
                                   string stringBFN = splitT[0];
                                   lastBFN = Convert.ToInt32(splitT[0]);
                              }
                         }
                    }
                    bfnReader.Close();

                    int thisBFN = lastBFN + 1;      // increment to new number
                    string thisBFNString = Convert.ToString(thisBFN);

                    // now add ob transaction to transactions.csv
                    string[] lastOBSplit = obFlightData.Split(',');
                    for (int i = 0; i < lastOBIndex; i++)
                    {
                         string flightNumber = lastOBSplit[(i * 9) + 2];
                         string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + obCost[i] + "," + "N";
                         newTransact.Add(newTrans);
                    }

                    // now add ib transaction to transactions.csv
                    string[] lastIBSplit = ibFlightData.Split(',');
                    for (int i = 0; i < lastIBIndex; i++)
                    {
                         string flightNumber = lastIBSplit[(i * 9) + 2];
                         string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + arrDate + "," + ibCost[i] + "," + "N";
                         newTransact.Add(newTrans);
                    }

                    // read all existing transactions
                    List<string> transactions = new List<string>();
                    StreamReader transactionReader = new StreamReader(transactionsfp);

                    using (transactionReader)
                    {
                         string line;
                         while ((line = transactionReader.ReadLine()) != null)
                         {
                              transactions.Add(line);
                         }
                    }
                    transactionReader.Close();

                    // write all old and new transactions
                    StreamWriter transactionWriter = new StreamWriter(transactionsfp, false);
                    using (transactionWriter)
                    {
                         foreach (string line in transactions)
                         {
                              transactionWriter.WriteLine(line);
                         }
                         foreach (string nline in newTransact)
                         {
                              transactionWriter.WriteLine(nline);
                         }
                    }
                    transactionWriter.Close();

                    // now add points to their account for giving us $
                    double ptBal;
                    double newPts = (obCost[lastOBIndex] / 10) + (ibCost[lastIBIndex] / 10);
                    newPts = Math.Round(newPts, 0, MidpointRounding.ToEven);         // rounding
                    List<string> accLines = new List<string>();
                    StreamReader accReader = new StreamReader(accfp);

                    using (accReader)
                    {
                         string line;
                         while ((line = accReader.ReadLine()) != null)
                         {
                              string[] splitRecord = line.Split(',');
                              if (splitRecord[6] == userID)
                              {
                                   double.TryParse(splitRecord[9], out ptBal);
                                   ptBal += newPts;
                                   string ptBalString = Convert.ToString(ptBal);
                                   splitRecord[9] = ptBalString;
                                   line = string.Join(",", splitRecord);
                              }
                              accLines.Add(line);
                         }
                    }
                    accReader.Close();

                    StreamWriter accWriter = new StreamWriter(accfp, false);

                    using (accWriter)
                    {
                         foreach (string line in accLines)
                         {
                              accWriter.WriteLine(line);
                         }
                    }
                    accWriter.Close();

                    // ADD RECEIPT
                    Console.Clear();
                    Console.WriteLine("Flight successfully booked!");
                    Thread.Sleep(3000);
                    Console.Clear();
                    startCustomer();
                    return;
               }
               else if (confirm == "N")
               {
                    Console.Clear();
                    startCustomer();
                    return;
               }
               else
               {
                    // split this up later to handle this check
                    Console.WriteLine("Enter a valid command.");
                    payWithDollarsRT(obFlightData, obCost, ibFlightData, ibCost);
                    return;
               }
          }

          static void payWithPointsRT(string obFlightData, List<double> obCost, List<double> obPoints, string ibFlightData, List<double> ibCost, List<double> ibPoints)
          {
               // read Accounts.csv to check if the customer has enough points
               // if they have nough points, proceed
               // if they do not have enough points, ask if they want to book with dollars or cancel booking process

               int lastOBIndex = obPoints.Count - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               int lastIBIndex = ibPoints.Count - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               double totalPointCost = obPoints[lastOBIndex] + ibPoints[lastIBIndex];
               int lastBFN = 0;
               bool recordExists = false;
               bool hasEnoughPoints = false;
               double savedPts = 0;

               // gets the number of points in the user's account
               StreamReader pointReader = new StreamReader(accfp);
               using (pointReader)
               {
                    string line;
                    while ((line = pointReader.ReadLine()) != null)
                    {
                         string[] split = line.Split(',');
                         if (split[6] == userID)
                         {
                              double.TryParse(split[9], out savedPts);
                              if (savedPts >= totalPointCost)
                              {
                                   hasEnoughPoints = true;
                              }
                              break;
                         }
                    }
               }
               pointReader.Close();

               // insufficient points case
               if (hasEnoughPoints == false)
               {
                    Console.WriteLine("Insufficient number of points: ");
                    Console.WriteLine("You have {0} points available and the flight costs {1} points.", savedPts, totalPointCost);
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1) Pay with dollars.");
                    Console.WriteLine("2) Cancel booking process and return to homepage.");
                    int selection = Convert.ToInt32(Console.ReadLine());

                    if (selection == 1)
                    {
                         Console.Clear();
                         payWithDollarsRT(obFlightData, obCost, ibFlightData, ibCost);
                         return;
                    }
                    else if (selection == 2)
                    {
                         Console.Clear();
                         startCustomer();
                         return;
                    }
                    else
                    {
                         // handle exception here, probably separate into another method
                         return;
                    }
               }
               // if the customer has enough points...
               if (hasEnoughPoints == true)
               {
                    string[] obSplit = obFlightData.Split(',');
                    Console.WriteLine("Booking Details:");
                    Console.WriteLine("Outbound Flight:");
                    if (lastOBIndex == 1)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
                    }
                    else if (lastOBIndex == 2)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[11], obSplit[12], obSplit[13], obSplit[15], obSplit[17]);
                    }
                    else if (lastOBIndex == 3)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", obSplit[1], obSplit[2], obSplit[3], obSplit[4], obSplit[6], obSplit[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[11], obSplit[12], obSplit[13], obSplit[15], obSplit[17]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", obSplit[20], obSplit[21], obSplit[22], obSplit[24], obSplit[26]);
                    }

                    string[] ibSplit = ibFlightData.Split(',');
                    Console.WriteLine("Booking Details:");
                    Console.WriteLine("Inbound Flight:");
                    if (lastIBIndex == 1)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
                    }
                    else if (lastIBIndex == 2)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[11], ibSplit[12], ibSplit[13], ibSplit[15], ibSplit[17]);
                    }
                    else if (lastIBIndex == 3)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", ibSplit[1], ibSplit[2], ibSplit[3], ibSplit[4], ibSplit[6], ibSplit[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[11], ibSplit[12], ibSplit[13], ibSplit[15], ibSplit[17]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", ibSplit[20], ibSplit[21], ibSplit[22], ibSplit[24], ibSplit[26]);
                    }

                    Console.WriteLine("Amount due: {0} points", totalPointCost);
                    Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
                    string confirm = Console.ReadLine();

                    // let's book now
                    if (confirm == "Y")
                    {
                         List<string> linesOB = new List<string>();
                         List<string> linesIB = new List<string>();
                         List<string> lines2 = new List<string>();
                         List<string> newFlightLines = new List<string>();
                         List<string> newTransact = new List<string>();

                         // read booked flight records to update seats on existing records for OB flights
                         StreamReader recordReaderOB = new StreamReader(bookedFlightsfp);
                         using (recordReaderOB)
                         {
                              string line;
                              while ((line = recordReaderOB.ReadLine()) != null)
                              {
                                   Console.WriteLine(line);
                                   string[] splitRecord = line.Split(',');
                                   if (lastOBIndex == 1)
                                   {

                                        if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastOBIndex == 2)
                                   {
                                        if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == obSplit[11] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastOBIndex == 3)
                                   {
                                        if (splitRecord[0] == obSplit[2] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == obSplit[11] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == obSplit[20] && splitRecord[1] == obSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   linesOB.Add(line);
                              }
                         }
                         recordReaderOB.Close();

                         // THEN WE REWRITE THE FILE FOR EXISTING RECORD CHANGES for ob
                         StreamWriter recordWritOB = new StreamWriter(bookedFlightsfp, false);

                         using (recordWritOB)
                         {
                              foreach (string line in linesOB)
                              {
                                   recordWritOB.WriteLine(line);
                              }
                         }
                         recordWritOB.Close();

                         // read booked flight records to update seats on existing records for IB flights
                         StreamReader recordReaderIB = new StreamReader(bookedFlightsfp);
                         using (recordReaderIB)
                         {
                              string line;
                              while ((line = recordReaderIB.ReadLine()) != null)
                              {
                                   Console.WriteLine(line);
                                   string[] splitRecord = line.Split(',');
                                   if (lastIBIndex == 1)
                                   {

                                        if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastIBIndex == 2)
                                   {
                                        if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == ibSplit[11] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastIBIndex == 3)
                                   {
                                        if (splitRecord[0] == ibSplit[2] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == ibSplit[11] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == ibSplit[20] && splitRecord[1] == ibSplit[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   linesIB.Add(line);
                              }
                         }
                         recordReaderIB.Close();

                         // THEN WE REWRITE THE FILE FOR EXISTING RECORD CHANGES for ib
                         StreamWriter recordWritIB = new StreamWriter(bookedFlightsfp, false);

                         using (recordWritIB)
                         {
                              foreach (string line in linesIB)
                              {
                                   recordWritIB.WriteLine(line);
                              }
                         }
                         recordWritIB.Close();

                         // then we check all of them and add new lines as needed for ob
                         string newLine;
                         for (int i = 0; i < lastOBIndex; i++)
                         {
                              recordExists = flightHasRecord(obSplit[(i * 9) + 2], deptDate);
                              if (recordExists == false)
                              {
                                   string[] splitAllFlightData = obFlightData.Split(',');
                                   string aircraft = splitAllFlightData[(i * 9) + 10];
                                   string numSeatsLeft = "";

                                   if (aircraft == "737")
                                   {
                                        int seats = 188;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "757")
                                   {
                                        int seats = 199;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "787")
                                   {
                                        int seats = 241;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                        + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                        + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                                   newFlightLines.Add(newLine);
                              }
                         }

                         // then we check all of them and add new lines as needed for ib
                         for (int i = 0; i < lastIBIndex; i++)
                         {
                              recordExists = flightHasRecord(ibSplit[(i * 9) + 2], arrDate);
                              if (recordExists == false)
                              {
                                   string[] splitAllFlightData = ibFlightData.Split(',');
                                   string aircraft = splitAllFlightData[(i * 9) + 10];
                                   string numSeatsLeft = "";

                                   if (aircraft == "737")
                                   {
                                        int seats = 188;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "757")
                                   {
                                        int seats = 199;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "787")
                                   {
                                        int seats = 241;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                        + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                        + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                                   newFlightLines.Add(newLine);
                              }
                         }

                         StreamReader recordReader2 = new StreamReader(bookedFlightsfp);
                         using (recordReader2)
                         {
                              string line;
                              while ((line = recordReader2.ReadLine()) != null)
                              {
                                   lines2.Add(line);
                              }
                         }
                         recordReader2.Close();

                         // for OB and IB
                         StreamWriter recordwriter = new StreamWriter(bookedFlightsfp, false);
                         using (recordwriter)
                         {
                              foreach (string line in lines2)
                              {
                                   recordwriter.WriteLine(line);
                              }
                              foreach (string newlines in newFlightLines)
                              {
                                   recordwriter.WriteLine(newlines);
                              }
                         }
                         recordwriter.Close();

                         // before we set up the strings that hold our transaction data, we need to find the last
                         //BookedFlightNum in the file
                         StreamReader bfnReader = new StreamReader(transactionsfp);
                         using (bfnReader)
                         {
                              string line;
                              while ((line = bfnReader.ReadLine()) != null)
                              {
                                   string[] splitT = line.Split(',');
                                   if (splitT[0].Contains("BookedFlightNum"))
                                   {
                                        // we don't do anything
                                   }
                                   else
                                   {
                                        string stringBFN = splitT[0];
                                        lastBFN = Convert.ToInt32(splitT[0]);
                                   }
                              }
                         }
                         bfnReader.Close();

                         int thisBFN = lastBFN + 1;      // increment to new number
                         string thisBFNString = Convert.ToString(thisBFN);

                         // now add ob transaction to transactions.csv
                         string[] lastOBSplit = obFlightData.Split(',');
                         for (int i = 0; i < lastOBIndex; i++)
                         {
                              string flightNumber = lastOBSplit[(i * 9) + 2];
                              string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + obPoints[i] + " points" + "," + "N";
                              newTransact.Add(newTrans);
                         }

                         // now add ib transaction to transactions.csv
                         string[] lastIBSplit = ibFlightData.Split(',');
                         for (int i = 0; i < lastIBIndex; i++)
                         {
                              string flightNumber = lastIBSplit[(i * 9) + 2];
                              string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + arrDate + "," + ibPoints[i] + " points" + "," + "N";
                              newTransact.Add(newTrans);
                         }

                         // read all existing transactions
                         List<string> transactions = new List<string>();
                         StreamReader transactionReader = new StreamReader(transactionsfp);

                         using (transactionReader)
                         {
                              string line;
                              while ((line = transactionReader.ReadLine()) != null)
                              {
                                   transactions.Add(line);
                              }
                         }
                         transactionReader.Close();

                         // write all old and new transactions
                         StreamWriter transactionWriter = new StreamWriter(transactionsfp, false);
                         using (transactionWriter)
                         {
                              foreach (string line in transactions)
                              {
                                   transactionWriter.WriteLine(line);
                              }
                              foreach (string nline in newTransact)
                              {
                                   transactionWriter.WriteLine(nline);
                              }
                         }
                         transactionWriter.Close();

                         // now remove the points they spent from PointsSaved, then add them to PointsSpent
                         double PointsSaved;
                         double PointsSpent;
                         List<string> accLines = new List<string>();
                         StreamReader accReader = new StreamReader(accfp);

                         using (accReader)
                         {
                              string line;
                              while ((line = accReader.ReadLine()) != null)
                              {
                                   string[] splitRecord = line.Split(',');
                                   if (splitRecord[6] == userID)
                                   {
                                        // subtract from points saved and reassign in the index
                                        double.TryParse(splitRecord[9], out PointsSaved);
                                        PointsSaved -= totalPointCost;
                                        string PointsSavedString = Convert.ToString(PointsSaved);
                                        splitRecord[9] = PointsSavedString;

                                        // add to points spent and reassign in index
                                        double.TryParse(splitRecord[10], out PointsSpent);
                                        PointsSpent += totalPointCost;
                                        string PointsSpentString = Convert.ToString(PointsSpent);
                                        splitRecord[10] = PointsSpentString;

                                        line = string.Join(",", splitRecord);
                                   }
                                   accLines.Add(line);
                              }
                         }
                         accReader.Close();

                         // now we write all records to the file (because it has updated rows)
                         StreamWriter recordWriter = new StreamWriter(accfp, false);

                         using (recordWriter)
                         {
                              foreach (string line in accLines)
                              {
                                   recordWriter.WriteLine(line);
                              }
                         }
                         recordWriter.Close();


                         // ADD RECEIPT
                         Console.Clear();
                         Console.WriteLine("Flight successfully booked!");
                         Thread.Sleep(3000);
                         Console.Clear();
                         startCustomer();
                         return;
                    }
               }
          }

          static void bookOneWay()
          {
               //store airports in an array
               string[] airportCode;
               airportCode = new string[12] { "BNA", "CLE", "DEN", "DFW", "DTW", "LAS", "LAX", "LGA", "MCO", "ORD", "PHX", "SEA" };
               Console.WriteLine("Project Air");
               Console.WriteLine("Select a source airport");
               Console.WriteLine("1) BNA");
               Console.WriteLine("2) CLE");
               Console.WriteLine("3) DEN");
               Console.WriteLine("4) DFW");
               Console.WriteLine("5) DTW");
               Console.WriteLine("6) LAS");
               Console.WriteLine("7) LAX");
               Console.WriteLine("8) LGA");
               Console.WriteLine("9) MCO");
               Console.WriteLine("10) ORD");
               Console.WriteLine("11) PHX");
               Console.WriteLine("12) SEA");
               Console.WriteLine("0) Go back");
               Console.WriteLine("Enter a number to select an option");
               int srcAirInput = Convert.ToInt32(Console.ReadLine());

               // to go back...
               if (srcAirInput == 0)
               {
                    Console.Clear();
                    bookAFlight();
                    return;
               }
               // else if the source was incorrectly entered
               else if ((srcAirInput > 12 || srcAirInput < 1) && srcAirInput != 0)
               {
                    Console.Clear();
                    Console.WriteLine("Invalid source airport selection. Try again");
                    bookOneWay();
                    return;
               }
               else if (srcAirInput >= 1 && srcAirInput <= 12)
               {
                    srcAirportCode = airportCode[srcAirInput - 1];  // subtract 1 to index, store code to read csv
                    Console.Clear();
                    Console.WriteLine("Source airport: {0}", srcAirportCode);
                    selectDest(srcAirInput);
                    return;
               }
          }

          // ADDED
          static void selectDest(int src)
          {
               //store airports in an array
               string[] airportCode;
               airportCode = new string[12] { "BNA", "CLE", "DEN", "DFW", "DTW", "LAS", "LAX", "LGA", "MCO", "ORD", "PHX", "SEA" };
               Console.WriteLine("Select a destination airport");
               Console.WriteLine("1) BNA");
               Console.WriteLine("2) CLE");
               Console.WriteLine("3) DEN");
               Console.WriteLine("4) DFW");
               Console.WriteLine("5) DTW");
               Console.WriteLine("6) LAS");
               Console.WriteLine("7) LAX");
               Console.WriteLine("8) LGA");
               Console.WriteLine("9) MCO");
               Console.WriteLine("10) ORD");
               Console.WriteLine("11) PHX");
               Console.WriteLine("12) SEA");
               Console.WriteLine("0) Go back");
               Console.WriteLine("Enter a number to select an option");
               int destAirInput = Convert.ToInt32(Console.ReadLine());
               Console.Clear();

               // to go back...
               if (destAirInput == 0)
               {
                    Console.Clear();
                    bookOneWay();
                    return;
               }
               else if (src == destAirInput)
               {
                    Console.WriteLine("Destination airport cannot be the same as the source airport. Try again");
                    selectDest(src);
                    return;
               }
               else if ((destAirInput > 12 || destAirInput < 1) && destAirInput != 0)
               {
                    Console.Clear();
                    Console.WriteLine("Invalid source airport selection. Try again");
                    selectDest(src);
                    return;
               }
               else if (destAirInput >= 1 && destAirInput <= 12)
               {
                    dstAirportCode = airportCode[destAirInput - 1];
                    Console.WriteLine("{0}", dstAirportCode);
                    oneWayDate();
                    return;
               }
          }

          static void oneWayDate()
          {
               // figure out how to read dates, reading the as string for now, will change later
               Console.WriteLine("Enter date of departure in the format M/D/YYYY. Flights may be booked up to 6 months in advance"); ;
               string depDate = Console.ReadLine();
               Console.WriteLine("depDate: {0}", depDate);

               // code to validate date
               DateTime validDate;
               string format = "M/d/yyyy";
               if (DateTime.TryParseExact(depDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out validDate))
               {
                    // date entered is a valid date...now check if it is a valid booking date
                    string[] depDateTemp = depDate.Split('/');
                    int.TryParse(depDateTemp[0], out int depMonth);   // get the departure month
                    int.TryParse(depDateTemp[1], out int depDay);     // get the departure day

                    sysDate = DateTime.Now.ToString("M/d/yyyy");         // get system date
                    sysTime = DateTime.Now.ToString("h:mm tt");            // and system time
                    string[] dateTemp = sysDate.Split('/');
                    int sysMonth, sysDay;
                    int.TryParse(dateTemp[0], out sysMonth);          // get system month
                    int.TryParse(dateTemp[1], out sysDay);            // get system day

                    DateTime sysDateDT;
                    DateTime.TryParseExact(sysDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out sysDateDT);        // convert to DateTime
                    int ret1 = DateTime.Compare(validDate, sysDateDT);          // compare the user entered date to today's date

                    if (ret1 < 0)
                    {
                         Console.WriteLine("Invalid date: {0} has already passed. Try another date on or after {1}", depDate, sysDate);
                         oneWayDate();
                         return;
                    }
                    else if (ret1 == 0)      // if the user is booking a flight for today
                    {
                         deptDate = depDate;
                         //displayOneWay();
                         // diplay flights departing in less than 1 hour
                         // if no flights
                         // display error message
                         // call oneWayDate();
                         // return;
                         // end if

                         // let user select a flight
                    }

                    DateTime maxDateDT = sysDateDT.AddMonths(6);
                    Console.WriteLine("Maximum Date: {0}", maxDateDT);
                    //DateTime.TryParseExact(maxDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out maxDateDT);
                    // compare system and user enter dates
                    int ret2 = DateTime.Compare(validDate, maxDateDT);     // compare the user entered date to the latest date they can book
                    string maxDate = maxDateDT.ToShortDateString();

                    if (ret2 > 0)       // if the user entered date is > 6 mos out
                    {
                         Console.WriteLine("Invalid date: {0} is more than 6 months in advance. Try another between {1} and {2}", depDate, sysDate, maxDate);
                         oneWayDate();
                         return;
                    }
                    else if (ret2 == 0 || ret2 < 0)         // if the user entered date is earlier or on the latest date available to book
                    {
                         deptDate = depDate;
                         //Console.WriteLine("Display one way called");
                         displayOneWay();
                         // diplay flights
                         // if no flights (rare unless we add a ton of test data)
                         // display error message
                         // call oneWayDate();
                         // return;
                         // end if

                         // let user select a flight
                    }
               }
               else           // the entered date is not valid
               {
                    Console.Clear();
                    Console.WriteLine("{0} is not a valid date. Try again.", depDate);
                    oneWayDate();
                    return;
               }
          }

          static void displayOneWay()
          {
               string srcAP;       //these strings recoginze the airport codes in the file
               string destAP;
               string dTime;
               string aTime;
               bool isNotFull = false;
               bool isNotFull1 = false;
               bool isNotFull2 = false;
               bool isNotFull3 = false;
               bool recordExists = false;
               bool record1Exists = false;
               bool record2Exists = false;
               bool record3Exists = false;
               string flightNumber;
               int userSelFlight;
               int tempConnectFlightCount = 0;
               int tempConnCountThree = 0;
               List<double> prices = new List<double>();
               List<double> points = new List<double>();
               //string departDate; TO UPDATE FLIGHT RECORD LATER
               List<string> directRoutes = new List<string>();        // store all valid direct src/dest combos in this array before more checks
               List<string> displayTracker = new List<string>();      // used to keep track of which flight the customer selects to book, so the CSVs can be updated
               List<string> tempConnectFlights = new List<string>();  // used to store a connecting flight to test
               List<string> finalConnectFlights = new List<string>();      // to store valid connections
               List<string> tempThreeLegFlight = new List<string>();       // used to store candidates for a 3 leg flight
               List<string> finalThreeLegFlight = new List<string>();      // used to store actual 3 leg flights
                                                                           //string candidateFlight;       // keeps the departure date and flight number

               // reads direct flights and stores them
               StreamReader routeReader = new StreamReader(routesfp);
               using (routeReader)
               {
                    string line;

                    while ((line = routeReader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];

                         // if the src and dest match
                         if (srcAP == srcAirportCode && destAP == dstAirportCode)
                         {
                              directRoutes.Add(line);
                         }
                    }
               }
               routeReader.Close();

               // read for indirect flights, using a different mechanism
               // gets the first leg of a possible connecting flight
               StreamReader routeReader2 = new StreamReader(routesfp);
               using (routeReader2)
               {
                    string line;
                    while ((line = routeReader2.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         flightNumber = row[0];
                         srcAP = row[1];
                         destAP = row[2];
                         dTime = row[4];
                         aTime = row[6];
                         //string modLine;

                         if (srcAP == srcAirportCode && destAP != dstAirportCode)
                         {
                              tempConnectFlightCount++;
                              //tempIndex++;
                              //modLine = tempIndex + "," + line;
                              tempConnectFlights.Add(line);
                         }
                    }
               }
               routeReader2.Close();

               // HANDLES 2nd part of connection
               StreamReader routeReader3 = new StreamReader(routesfp);
               using (routeReader3)
               {
                    // this allows us to test all possible first legs in the temp connect flights array, however there may be a larger number of
                    // flights generated than the number of candidate first legs, which the while loop allows for
                    for (int j = 0; j < tempConnectFlightCount; j++)
                    {
                         string currentRoute = tempConnectFlights[j];
                         //Console.WriteLine("string at first index: {0}", currentRoute);
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[2];
                         string line;
                         while ((line = routeReader3.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == dstAirportCode)       // if we found the connection for a possible 2 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival+40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                   {
                                        string twoLegRoute = currentRoute + ',' + line;
                                        finalConnectFlights.Add(twoLegRoute);
                                        //Console.WriteLine(twoLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {

                                   }
                              }
                              else if (testSplit[1] == routeDest && testSplit[2] != dstAirportCode)
                              {
                                   // here we check to see if the flight in question leaves in the PM and arrives in the AM
                                   // if it does, we will not add this to our new list because we want cutomers
                                   // to leave and arrive on the same day
                                   string timeFormat = "h:mm tt";
                                   DateTime tempDep;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempDep);
                                   DateTime tempArr;
                                   DateTime.TryParseExact(testSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tempArr);
                                   int retNextDay = DateTime.Compare(tempDep, tempArr);
                                   if (retNextDay < 0)
                                   {
                                        // then we test timing
                                        // convert testSplit[4] to dateTime
                                        //string timeFormat = "h:mm tt";
                                        DateTime testTime;
                                        DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                        // convert arrival time to datetime
                                        DateTime firstLegTime;
                                        DateTime.TryParseExact(routeSplit[6], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                        // add 40 mins to arrival
                                        DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                        // compare testSplit[4] to arrival+40 mins
                                        int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                        // if ret > 0 then we store in a final list<string>
                                        if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 2 LEG CONNECTION
                                        {
                                             string twoLegRoute = currentRoute + ',' + line;
                                             tempThreeLegFlight.Add(twoLegRoute);
                                             tempConnCountThree++;
                                             //Console.WriteLine(twoLegRoute);
                                        }
                                   }
                              }

                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader3.DiscardBufferedData();
                         routeReader3.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }
               //}
               routeReader3.Close();

               // now we find 3rd connections
               StreamReader routeReader4 = new StreamReader(routesfp);
               using (routeReader4)
               {
                    for (int j = 0; j < tempConnCountThree; j++)
                    {
                         string currentRoute = tempThreeLegFlight[j];
                         string[] routeSplit = currentRoute.Split(',');
                         string routeDest = routeSplit[11];
                         string line;

                         while ((line = routeReader4.ReadLine()) != null)
                         {
                              string[] testSplit = line.Split(',');
                              string testSrc = testSplit[1];
                              string testDest = testSplit[2];
                              if (testSplit[1] == routeDest && testSplit[2] == dstAirportCode)       // if we found the connection for a possible 3 leg flight
                              {
                                   // then we test timing
                                   // convert testSplit[4] to dateTime
                                   string timeFormat = "h:mm tt";
                                   DateTime testTime;
                                   DateTime.TryParseExact(testSplit[4], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out testTime);
                                   // convert arrival time to datetime
                                   DateTime firstLegTime;
                                   DateTime.TryParseExact(routeSplit[15], timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out firstLegTime);
                                   // add 40 mins to arrival
                                   DateTime firstLeg40 = firstLegTime.AddMinutes(40);
                                   // compare testSplit[4] to arrival + 40 mins
                                   int retTimeComp = DateTime.Compare(testTime, firstLeg40);
                                   // if ret > 0 then we store in a final list<string>
                                   if (retTimeComp >= 0)         // the layover is 40 mins or more, WE FOUND A 3 LEG CONNECTION
                                   {
                                        string threeLegRoute = currentRoute + ',' + line;
                                        finalThreeLegFlight.Add(threeLegRoute);
                                   }
                                   // else we move on
                                   else
                                   {
                                        // nothing
                                   }
                              }
                              else
                              {
                                   // do nothing
                              }
                         }
                         // this clears the buffer and takes us back to the beginning of the file to continue
                         // finding other connections for the same first leg
                         routeReader4.DiscardBufferedData();
                         routeReader4.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
               }

               int i = 1;
               foreach (string line in directRoutes)
               {
                    string[] split = line.Split(',');
                    string candidateFlight;       // keeps the departure date and flight number

                    // need to check the flights file to see if it exists for this date 
                    // check flight number and deptDate
                    recordExists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (recordExists == true)
                    {
                         //we don't have to generate a flight number, but we need to retrieve it
                         // we need to check if the plane is full
                         isNotFull = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         // if plane has seats
                         if (isNotFull == true)
                         {
                              // we will display this flight
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                              candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(candidateFlight);     // add to list
                              i++;      // increment for next
                         }
                    }
                    // if record does not exist
                    else if (recordExists == false)
                    {
                         // the plane is empty, so display this flight
                         Console.WriteLine("{0}) {1} - {2}       {3} to {4}", i, split[1], split[2], split[4], split[6]);
                         candidateFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(candidateFlight);     // add to list
                         i++;      // increment for next
                    }
               }

               foreach (string line in finalConnectFlights)
               {
                    string[] split = line.Split(',');
                    string cand2LegFlight;       // keeps the departure date and flight number

                    record1Exists = flightHasRecord(split[0], deptDate);
                    // if it exists...
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);     // returns false if there are no seats left
                         if (isNotFull1 == true)
                         {
                              record2Exists = flightHasRecord(split[9], deptDate);
                              if (record2Exists == true)
                              {
                                   isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                                   if (isNotFull2 == true)
                                   {
                                        // display both
                                        // we will display this flight
                                        Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                             split[11], split[13], split[15]);
                                        cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                        displayTracker.Add(cand2LegFlight);     // add to list
                                        i++;
                                   }
                                   else
                                   {
                                        // leg 2 is full, DON'T DISPLAY
                                   }
                              }
                              else
                              {
                                   // leg2 is also open, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                         }
                         else
                         {
                              // DON'T DISPLAY
                         }
                    }
                    else
                    {
                         // leg1 doesn't have a record, check record 2
                         record2Exists = flightHasRecord(split[9], deptDate);
                         if (record2Exists == true)
                         {
                              isNotFull2 = checkSeats(split[9], deptDate);     // returns false if there are no seats left
                              if (isNotFull2 == true)
                              {
                                   // both legs have seats, display
                                   Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                        split[11], split[13], split[15]);
                                   cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                                   displayTracker.Add(cand2LegFlight);     // add to list
                                   i++;
                              }
                              else
                              {
                                   // don't display
                              }
                         }
                         else
                         {
                              // both records do not exist, so both are available, display
                              Console.WriteLine("{0}) {1} - {2}       {3} to {4}, {5} - {6}         {7} to {8}", i, split[1], split[2], split[4], split[6], split[10],
                                   split[11], split[13], split[15]);
                              cand2LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                              displayTracker.Add(cand2LegFlight);     // add to list
                              i++;
                         }
                    }
               }

               foreach (string line in finalThreeLegFlight)
               {
                    string[] split = line.Split(',');
                    string cand3LegFlight;       // keeps the departure date and index with everything
                    record1Exists = flightHasRecord(split[0], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull1 = checkSeats(split[0], deptDate);
                    }
                    record2Exists = flightHasRecord(split[9], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull2 = checkSeats(split[9], deptDate);
                    }
                    record3Exists = flightHasRecord(split[18], deptDate);
                    if (record1Exists == true)
                    {
                         isNotFull3 = checkSeats(split[18], deptDate);
                    }

                    if ((isNotFull1 == true || record1Exists == false) && (isNotFull2 == true || record2Exists == false) && (isNotFull3 == true || record3Exists == false))
                    {
                         // all 3 legs have seats, display;
                         Console.WriteLine("{0}) {1} - {2} - {3} - {4}     {5} to {6}", i, split[1], split[10], split[19], split[20], split[4], split[24]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[1], split[2], split[4], split[6]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[10], split[11], split[13], split[15]);
                         Console.WriteLine("   {0} - {1}       {2} to {3}", split[19], split[20], split[22], split[24]);
                         cand3LegFlight = i + "," + deptDate + "," + line;      // combine the number option to be displayed, date, and all of the line
                         displayTracker.Add(cand3LegFlight);     // add to list
                         i++;
                    }
               }

               // if all flights are booked for this day (highly unlikely, nearly impossible)
               if (displayTracker.Count == 0)
               {
                    Console.WriteLine("No flights available on this day. Try another date.");
                    oneWayDate();
                    return;
               }
               else if (displayTracker.Count != 0)
               {
                    Console.WriteLine("Select an outbound flight");
                    userSelFlight = Convert.ToInt32(Console.ReadLine());
                    foreach (string candidateFlight in displayTracker)
                    {
                         string[] canSplit = candidateFlight.Split(',');
                         double ticketPrice = 0;
                         double runningTotalPrice = 0;
                         double tempPoints = 0;
                         double runningTotalPoints = 0;
                         int storedOptNum = Convert.ToInt32((string)canSplit[0]);
                         // if we found the flight the cust chose to book
                         if (storedOptNum == userSelFlight)
                         {
                              int totalElements = canSplit.Count();        // get number of elements in array to see how many connections it has
                              int numLoops = (totalElements - 2) / 9;
                              for (int a = 0; a < numLoops; a++)
                              {
                                   int APDist = Convert.ToInt32((string)canSplit[(a * 9) + 5]);
                                   ticketPrice = 58 + (0.12 * APDist); // base calculation

                                   // check times for discounts
                                   string tempDepTime = canSplit[(a * 9) + 6];
                                   string tempArrTime = canSplit[(a * 9) + 8];
                                   string timeFormat = "h:mm tt";
                                   string string8 = "8:00 AM";
                                   string string7 = "7:00 PM";
                                   string string12 = "12:00 AM";
                                   string string5 = "5:00 AM";
                                   DateTime depTimeDT, arrTimeDT, string8DT, string7DT, string12DT, string5DT;
                                   DateTime.TryParseExact(tempDepTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out depTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(tempArrTime, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out arrTimeDT);        // convert to DateTime
                                   DateTime.TryParseExact(string8, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string8DT);        // convert to DateTime
                                   DateTime.TryParseExact(string7, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string7DT);        // convert to DateTime
                                   DateTime.TryParseExact(string12, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string12DT);        // convert to DateTime
                                   DateTime.TryParseExact(string5, timeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out string5DT);        // convert to DateTime

                                   int ret8AM = DateTime.Compare(depTimeDT, string8DT);
                                   int ret7PM = DateTime.Compare(arrTimeDT, string7DT);
                                   int ret12AMDepart = DateTime.Compare(depTimeDT, string12DT);
                                   int ret12AMArrive = DateTime.Compare(arrTimeDT, string12DT);
                                   int ret5AMDepart = DateTime.Compare(depTimeDT, string5DT);
                                   int ret5AMArrive = DateTime.Compare(arrTimeDT, string5DT);

                                   if ((ret12AMDepart >= 0 && ret5AMDepart <= 0) || (ret12AMArrive >= 0 && ret5AMArrive <= 0))
                                   {
                                        ticketPrice = ticketPrice * 0.8;
                                   }
                                   else if (ret5AMDepart > 0 && ret8AM < 0 || ret7PM > 0) // departing before 8am or arriving after 7pm
                                   {
                                        ticketPrice = ticketPrice * 0.9;        // 10 percent off
                                   }
                                   // no else case, the else is already done
                                   ticketPrice = Math.Round(ticketPrice, 2);
                                   prices.Add(ticketPrice);      // add ticket price to the array
                                   runningTotalPrice += ticketPrice;
                                   tempPoints = ticketPrice * 100;
                                   points.Add(tempPoints);
                                   runningTotalPoints += tempPoints;
                              }
                              //runningTotalPrice = Math.Round(runningTotalPrice, 2);
                              prices.Add(runningTotalPrice);
                              points.Add(runningTotalPoints);
                              // ticketPrice = Math.Round(ticketPrice, 2);
                              // double ticketPricePoints = ticketPrice * 100;
                              Console.WriteLine("Flight price: ${0} or {1} points", runningTotalPrice, runningTotalPoints);
                              Console.WriteLine("Select an payment option");
                              Console.WriteLine("1) Pay with dollars");
                              Console.WriteLine("2) Pay with points");
                              Console.WriteLine("Enter a number to select an option");
                              int payInput = Convert.ToInt32(Console.ReadLine());

                              // separate this out later...to handle case of invalid input
                              if (payInput == 1)
                              {
                                   payWithDollars(candidateFlight, prices);
                                   return;
                              }
                              else if (payInput == 2)
                              {
                                   payWithPoints(candidateFlight, prices, points);
                                   return;
                              }
                         }
                    }
               }
          }

          // used to check if a flight has a record, check date and flight num
          static bool flightHasRecord(string flightNum, string date)
          {
               bool hasRecord = false;
               StreamReader flightReader = new StreamReader(bookedFlightsfp);
               using (flightReader)
               {
                    string line;
                    while ((line = flightReader.ReadLine()) != null)
                    {
                         string[] split = line.Split(',');
                         if (split[0] == flightNum && split[1] == date)
                         {
                              hasRecord = true; break;
                         }
                    }
               }
               flightReader.Close();
               return hasRecord;
          }

          // used to check if seats are remaining before displaying the option to the customer
          static bool checkSeats(string flightNum, string date)
          {
               bool seatsLeft = false;
               bool flightFound = false;          // used for loop
               StreamReader seatReader = new StreamReader(bookedFlightsfp);
               using (seatReader)
               {
                    string line;
                    while ((line = seatReader.ReadLine()) != null && flightFound == false)
                    {
                         string[] split = line.Split(',');
                         if (split[0] == flightNum && split[1] == date)
                         {
                              int numSeats = Convert.ToInt32(split[10]);
                              if (numSeats > 0)
                              {
                                   seatsLeft = true;
                              }
                              flightFound = true;
                         }
                    }

               }
               seatReader.Close();
               return seatsLeft;
          }

          static void payWithDollars(string allFlightData, List<double> cost)
          {
               //string newLine;
               int lastIndex = cost.Count() - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               int lastBFN = 0;         // hold's the last booked flight number in transactions
               string[] split = allFlightData.Split(',');
               bool recordExists = false;
               Console.WriteLine("Booking Details:");
               if (lastIndex == 1)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
               }
               else if (lastIndex == 2)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[11], split[12], split[13], split[15], split[17]);
               }
               else if (lastIndex == 3)
               {
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[11], split[12], split[13], split[15], split[17]);
                    Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[20], split[21], split[22], split[24], split[26]);
               }
               Console.WriteLine("Amount due: ${0}", cost[lastIndex]);
               Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
               string confirm = Console.ReadLine();
               if (confirm == "Y")
               {
                    // let's update all relevant CSV files...
                    // starting with BookedFlightRecords
                    List<string> lines = new List<string>();
                    List<string> lines2 = new List<string>();
                    List<string> newFlightLines = new List<string>();
                    List<string> newTransact = new List<string>();

                    StreamReader recordReader = new StreamReader(bookedFlightsfp);
                    using (recordReader)
                    {
                         string line;
                         while ((line = recordReader.ReadLine()) != null)
                         {
                              Console.WriteLine(line);
                              string[] splitRecord = line.Split(',');
                              if (lastIndex == 1)
                              {

                                   if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastIndex == 2)
                              {
                                   if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == split[11] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              else if (lastIndex == 3)
                              {
                                   if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == split[11] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   else if (splitRecord[0] == split[20] && splitRecord[1] == split[1])
                                   {
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                              }
                              lines.Add(line);
                         }
                    }
                    recordReader.Close();

                    // THEN WE REWRITE THE FILE FOR EXISTING RECORD CHANGES
                    StreamWriter recordWrit = new StreamWriter(bookedFlightsfp, false);

                    using (recordWrit)
                    {
                         foreach (string line in lines)
                         {
                              recordWrit.WriteLine(line);
                         }
                    }
                    recordWrit.Close();

                    // then we check all of them and add new lines as needed
                    string newLine;
                    for (int i = 0; i < lastIndex; i++)
                    {
                         recordExists = flightHasRecord(split[(i * 9) + 2], deptDate);
                         if (recordExists == false)
                         {
                              string[] splitAllFlightData = allFlightData.Split(',');
                              string aircraft = splitAllFlightData[(i * 9) + 10];
                              string numSeatsLeft = "";

                              if (aircraft == "737")
                              {
                                   int seats = 188;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "757")
                              {
                                   int seats = 199;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              else if (aircraft == "787")
                              {
                                   int seats = 241;
                                   numSeatsLeft = Convert.ToString(seats);
                              }
                              newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                   + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                   + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                              newFlightLines.Add(newLine);
                         }
                    }

                    StreamReader recordReader2 = new StreamReader(bookedFlightsfp);
                    using (recordReader2)
                    {
                         string line;
                         while ((line = recordReader2.ReadLine()) != null)
                         {
                              lines2.Add(line);
                         }
                    }
                    recordReader2.Close();


                    StreamWriter recordwriter = new StreamWriter(bookedFlightsfp, false);
                    using (recordwriter)
                    {
                         foreach (string line in lines2)
                         {
                              recordwriter.WriteLine(line);
                         }
                         foreach (string newlines in newFlightLines)
                         {
                              recordwriter.WriteLine(newlines);
                         }
                    }
                    recordwriter.Close();

                    // before we set up the strings that hold or transaction data, we need to find the last
                    //BookedFlightNum in the file
                    StreamReader bfnReader = new StreamReader(transactionsfp);
                    using (bfnReader)
                    {
                         string line;
                         while ((line = bfnReader.ReadLine()) != null)
                         {
                              string[] splitT = line.Split(',');
                              if (splitT[0].Contains("BookedFlightNum"))
                              {
                                   // we don't do anything
                              }
                              else
                              {
                                   string stringBFN = splitT[0];
                                   lastBFN = Convert.ToInt32(splitT[0]);
                              }
                         }
                    }
                    bfnReader.Close();
                    int thisBFN = lastBFN + 1;      // increment to new number
                    string thisBFNString = Convert.ToString(thisBFN);

                    // now add transaction to transactions.csv
                    string[] lastSplit = allFlightData.Split(',');
                    for (int i = 0; i < lastIndex; i++)
                    {
                         string flightNumber = lastSplit[(i * 9) + 2];
                         string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + cost[i] + "," + "N";
                         newTransact.Add(newTrans);
                    }

                    // read all existing transactions
                    List<string> transactions = new List<string>();
                    StreamReader transactionReader = new StreamReader(transactionsfp);

                    using (transactionReader)
                    {
                         string line;
                         while ((line = transactionReader.ReadLine()) != null)
                         {
                              transactions.Add(line);
                         }
                    }
                    transactionReader.Close();

                    StreamWriter transactionWriter = new StreamWriter(transactionsfp, false);
                    using (transactionWriter)
                    {
                         foreach (string line in transactions)
                         {
                              transactionWriter.WriteLine(line);
                         }
                         foreach (string nline in newTransact)
                         {
                              transactionWriter.WriteLine(nline);
                         }
                    }
                    transactionWriter.Close();


                    // now add points to their account for giving us $
                    double ptBal;
                    double newPts = cost[lastIndex] / 10;
                    newPts = Math.Round(newPts, 0, MidpointRounding.ToEven);         // rounding
                    List<string> accLines = new List<string>();
                    StreamReader accReader = new StreamReader(accfp);

                    using (accReader)
                    {
                         string line;
                         while ((line = accReader.ReadLine()) != null)
                         {
                              string[] splitRecord = line.Split(',');
                              if (splitRecord[6] == userID)
                              {
                                   double.TryParse(splitRecord[9], out ptBal);
                                   ptBal += newPts;
                                   string ptBalString = Convert.ToString(ptBal);
                                   splitRecord[9] = ptBalString;
                                   line = string.Join(",", splitRecord);
                              }
                              accLines.Add(line);
                         }
                    }
                    accReader.Close();

                    StreamWriter recordWriter = new StreamWriter(accfp, false);

                    using (recordWriter)
                    {
                         foreach (string line in accLines)
                         {
                              recordWriter.WriteLine(line);
                         }
                    }
                    recordWriter.Close();

                    // ADD RECEIPT
                    Console.Clear();
                    Console.WriteLine("Flight successfully booked!");
                    Thread.Sleep(3000);
                    Console.Clear();
                    startCustomer();
                    return;
               }
               else if (confirm == "N")
               {
                    Console.Clear();
                    startCustomer();
                    return;
               }
               else
               {
                    // split this up later to handle this check
                    Console.WriteLine("Enter a valid command.");
                    payWithDollars(allFlightData, cost);
                    return;
               }
          }

          static void payWithPoints(string allFlightData, List<double> cost, List<double> points)
          {
               // read Accounts.csv to check if the customer has enough points
               // if they have nough points, proceed
               // if they do not have enough points, ask if they want to book with dollars or cancel booking process

               int lastIndex = points.Count() - 1;       // indicates how many ticket prices we have, which indicates how many routes we have
               Console.WriteLine("lastIndex points: {0}", lastIndex);
               int lastBFN = 0;
               bool hasEnoughPoints = false;
               double savedPts = 0;
               // gets the number of points in the user's account
               StreamReader pointReader = new StreamReader(accfp);
               using (pointReader)
               {
                    string line;
                    while ((line = pointReader.ReadLine()) != null)
                    {
                         string[] split = line.Split(',');
                         if (split[6] == userID)
                         {
                              double.TryParse(split[9], out savedPts);
                              if (savedPts >= points[lastIndex])
                              {
                                   hasEnoughPoints = true;
                              }
                              break;
                         }
                    }
               }
               pointReader.Close();

               // insufficient points case
               if (hasEnoughPoints == false)
               {
                    Console.WriteLine("Insufficient number of points: ");
                    Console.WriteLine("You have {0} points available and the flight costs {1} points.", savedPts, points[lastIndex]);
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1) Pay with dollars.");
                    Console.WriteLine("2) Cancel booking process and return to homepage.");
                    int selection = Convert.ToInt32(Console.ReadLine());

                    if (selection == 1)
                    {
                         Console.Clear();
                         payWithDollars(allFlightData, cost);
                         return;
                    }
                    else if (selection == 2)
                    {
                         Console.Clear();
                         startCustomer();
                         return;
                    }
                    else
                    {
                         // handle exception here, probably separate into another method
                         return;
                    }
               }
               // if the customer has enough points...
               if (hasEnoughPoints == true)
               {
                    string[] split = allFlightData.Split(',');
                    bool recordExists = false;
                    // display booking details before asking them to proceed
                    Console.WriteLine("Booking Details:");
                    if (lastIndex == 1)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                    }
                    else if (lastIndex == 2)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[11], split[12], split[13], split[15], split[17]);
                    }
                    else if (lastIndex == 3)
                    {
                         Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[11], split[12], split[13], split[15], split[17]);
                         Console.WriteLine("         {0}        {1} to {2}      {3} to {4}", split[20], split[21], split[22], split[24], split[26]);
                    }
                    Console.WriteLine("Amount due: {0} points", points[lastIndex]);
                    Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
                    string confirm = Console.ReadLine();

                    // let's book now
                    if (confirm == "Y")
                    {
                         // let's update all relevant CSV files...
                         // starting with BookedFlightRecords
                         List<string> lines = new List<string>();
                         List<string> lines2 = new List<string>();
                         List<string> newFlightLines = new List<string>();
                         List<string> newTransact = new List<string>();

                         // here, we read through the file and decrement the seats left column if that route
                         // exists on that day, for each leg
                         StreamReader recordReader = new StreamReader(bookedFlightsfp);
                         using (recordReader)
                         {
                              string line;
                              while ((line = recordReader.ReadLine()) != null)
                              {
                                   Console.WriteLine(line);
                                   string[] splitRecord = line.Split(',');
                                   if (lastIndex == 1)
                                   {

                                        if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastIndex == 2)
                                   {
                                        if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == split[11] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   else if (lastIndex == 3)
                                   {
                                        if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == split[11] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        else if (splitRecord[0] == split[20] && splitRecord[1] == split[1])
                                        {
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft--;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                   }
                                   lines.Add(line);
                              }
                         }
                         recordReader.Close();

                         // then we rewrite the file for any existing flight changes.
                         // doing this before we add flights makes the process of updating BookedFlightRecords easier
                         StreamWriter recordWrit = new StreamWriter(bookedFlightsfp, false);

                         using (recordWrit)
                         {
                              foreach (string line in lines)
                              {
                                   recordWrit.WriteLine(line);
                              }
                         }
                         recordWrit.Close();

                         // handle all legs that dp not have a record yet in BookedFlightRecords...
                         // then we check all of them and add new lines as needed
                         string newLine;
                         for (int i = 0; i < lastIndex; i++)
                         {
                              recordExists = flightHasRecord(split[(i * 9) + 2], deptDate);
                              if (recordExists == false)
                              {
                                   string[] splitAllFlightData = allFlightData.Split(',');
                                   string aircraft = splitAllFlightData[(i * 9) + 10];
                                   string numSeatsLeft = "";

                                   if (aircraft == "737")
                                   {
                                        int seats = 188;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "757")
                                   {
                                        int seats = 199;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   else if (aircraft == "787")
                                   {
                                        int seats = 241;
                                        numSeatsLeft = Convert.ToString(seats);
                                   }
                                   newLine = splitAllFlightData[(i * 9) + 2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[(i * 9) + 3] + "," + splitAllFlightData[(i * 9) + 4]
                                        + "," + splitAllFlightData[(i * 9) + 5] + "," + splitAllFlightData[(i * 9) + 6] + "," + splitAllFlightData[(i * 9) + 7] + "," + splitAllFlightData[(i * 9) + 8]
                                        + "," + splitAllFlightData[(i * 9) + 9] + "," + splitAllFlightData[(i * 9) + 10] + "," + numSeatsLeft;
                                   newFlightLines.Add(newLine);
                              }
                         }

                         // so we read the file again to capture the updates to existing records (there may or may have not been any)
                         StreamReader recordReader2 = new StreamReader(bookedFlightsfp);
                         using (recordReader2)
                         {
                              string line;
                              while ((line = recordReader2.ReadLine()) != null)
                              {
                                   lines2.Add(line);
                              }
                         }
                         recordReader2.Close();

                         // then we write the old and new lines back to the csv!
                         StreamWriter recordwriter = new StreamWriter(bookedFlightsfp, false);
                         using (recordwriter)
                         {
                              foreach (string line in lines2)
                              {
                                   recordwriter.WriteLine(line);
                              }
                              foreach (string newlines in newFlightLines)
                              {
                                   recordwriter.WriteLine(newlines);
                              }
                         }
                         recordwriter.Close();

                         // before we set up the strings that hold or transaction data, we need to find the last
                         //BookedFlightNum in the file
                         StreamReader bfnReader = new StreamReader(transactionsfp);
                         using (bfnReader)
                         {
                              string line;
                              while ((line = bfnReader.ReadLine()) != null)
                              {
                                   string[] splitT = line.Split(',');
                                   if (splitT[0].Contains("BookedFlightNum"))
                                   {
                                        // we don't do anything
                                   }
                                   else
                                   {
                                        string stringBFN = splitT[0];
                                        lastBFN = Convert.ToInt32(splitT[0]);
                                   }
                              }
                         }
                         bfnReader.Close();
                         int thisBFN = lastBFN + 1;      // increment to new number
                         string thisBFNString = Convert.ToString(thisBFN);

                         // now add transaction to transactions.csv
                         string[] lastSplit = allFlightData.Split(',');
                         for (int i = 0; i < lastIndex; i++)
                         {
                              string flightNumber = lastSplit[(i * 9) + 2];
                              string newTrans = thisBFNString + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + points[i] + " points" + "," + "N";
                              newTransact.Add(newTrans);
                         }

                         // now we update transactions...
                         List<string> transactions = new List<string>();
                         StreamReader transactionReader = new StreamReader(transactionsfp);
                         // read and store existing transactions
                         using (transactionReader)
                         {
                              string line;
                              while ((line = transactionReader.ReadLine()) != null)
                              {
                                   transactions.Add(line);
                              }
                         }
                         transactionReader.Close();

                         // write old and new transactions
                         StreamWriter transactionWriter = new StreamWriter(transactionsfp, false);
                         using (transactionWriter)
                         {
                              foreach (string line in transactions)
                              {
                                   transactionWriter.WriteLine(line);
                              }
                              foreach (string nline in newTransact)
                              {
                                   transactionWriter.WriteLine(nline);
                              }
                         }
                         transactionWriter.Close();

                         // now remove the points they spent from PointsSaved, then add them to PointsSpent
                         double PointsSaved;
                         double PointsSpent;
                         List<string> accLines = new List<string>();
                         StreamReader accReader = new StreamReader(accfp);

                         using (accReader)
                         {
                              string line;
                              while ((line = accReader.ReadLine()) != null)
                              {
                                   string[] splitRecord = line.Split(',');
                                   if (splitRecord[6] == userID)
                                   {
                                        // subtract from points saved and reassign in the index
                                        double.TryParse(splitRecord[9], out PointsSaved);
                                        Console.WriteLine("Points @ index i: {0}", points[lastIndex]);
                                        PointsSaved -= points[lastIndex];
                                        string PointsSavedString = Convert.ToString(PointsSaved);
                                        splitRecord[9] = PointsSavedString;

                                        // add to points spent and reassign in index
                                        double.TryParse(splitRecord[10], out PointsSpent);
                                        PointsSpent += points[lastIndex];
                                        string PointsSpentString = Convert.ToString(PointsSpent);
                                        splitRecord[10] = PointsSpentString;

                                        line = string.Join(",", splitRecord);
                                   }
                                   accLines.Add(line);
                              }
                         }
                         recordReader.Close();

                         // now we write all records to the file (because it has updated rows)
                         StreamWriter recordWriter = new StreamWriter(accfp, false);

                         using (recordWriter)
                         {
                              foreach (string line in accLines)
                              {
                                   recordWriter.WriteLine(line);
                              }
                         }
                         recordWriter.Close();


                         // ADD RECEIPT
                         Console.Clear();
                         Console.WriteLine("Flight successfully booked!");
                         Thread.Sleep(3000);
                         Console.Clear();
                         startCustomer();
                         return;
                    }
                    else if (confirm == "N")
                    {
                         Console.Clear();
                         startCustomer();
                         return;
                    }
                    else
                    {
                         // split this up later to handle this check
                         Console.WriteLine("Enter a valid command.");
                         return;
                    }
               }
          }

          // CUSTOMER METHODS END HERE

          //-------------------------------------------------READ ME
          // Functionality done but there are no checks to make sure that the user doesnt enter information that isnt possible
          // also have to make sure to change the timezone and distance when they change airports
          static void editRoute()
          {
               Console.WriteLine("Available airports: BNA CLE DEN DFW DTW LAS LAX LGA MCO ORD PHX SEA");
               Console.WriteLine("Enter a source airport for the flight route you want to edit:");
               string usrsourceAP = Console.ReadLine();
               Console.Clear();

               String filePath = routesfp; // to change to something else later

               StreamReader reader = new StreamReader(filePath);

               using (reader)
               {
                    string line;
                    line = reader.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         string readsourceAP = row[1];

                         if (readsourceAP == usrsourceAP) // if the user ID exists then exit the loop
                         {
                              Console.WriteLine("Route Number: " + row[0] + ". Source Airport " + row[1] + ". Destination " + row[2] + ". Distance " + row[3] + ". Departure Time " + row[4] + ". Source Timezone " + row[5] + ". Arrival Time " + row[6] + ". Destination Timezone " + row[7]);
                         }

                    }
               }
               reader.Close();

               Console.WriteLine("Enter the Route number for the route you want to edit: ");
               string rninput = Console.ReadLine();
               Console.WriteLine("Source Airport = 1, Destination = 2, Distance = 3, Departure Time = 4, Arrival Time = 6");
               Console.WriteLine("Enter the part you would like to change: ");
               int partinput = Convert.ToInt32(Console.ReadLine());
               Console.WriteLine("Enter what to change the part to: ");
               string changeinput = Console.ReadLine();
               Console.Clear();

               string path = routesfp;
               List<String> lines = new List<String>();
               StreamReader reader1 = new StreamReader(path);

               if (File.Exists(path))
               {
                    using (reader1)
                    {
                         String line;

                         while ((line = reader1.ReadLine()) != null)
                         {
                              if (line.Contains(","))
                              {
                                   String[] split = line.Split(',');

                                   if (split[0] == rninput) // if the route is the route that the user wants changed
                                   {
                                        split[partinput] = changeinput;
                                        line = String.Join(",", split);
                                   }
                              }

                              lines.Add(line);
                         }
                    }

                    reader1.Close();

                    StreamWriter writer = new StreamWriter(path, false);

                    using (writer)
                    {
                         foreach (String line in lines)
                              writer.WriteLine(line);
                    }
                    writer.Close();

                    Console.WriteLine("Route successfully changed!");
                    Thread.Sleep(3000);
                    Console.Clear();
               }
          }


          //functionality works, but there are no checks to see if the times are valid and no checks to see if the airports and aircraft are valid.
          static void addRoute()
          {
               String filePath = routesTZfp; // to change to something else later

               Console.WriteLine("Enter the source airport for the new route: ");
               string sainput = Console.ReadLine();
               Console.WriteLine("Enter the destination airport for the new route: ");
               string dainput = Console.ReadLine();
               Console.WriteLine("Enter the departure time for the new route: ");
               string dtinput = Console.ReadLine();
               Console.WriteLine("Enter the arrival time for the new route: ");
               string atinput = Console.ReadLine();
               Console.WriteLine("Enter the aircraft desired for the new route: ");
               string rtinput = Console.ReadLine();

               string sourcetz = "", desttz = "";
               string distance = "";
               //assume for now user is entering valid input

               // this reader is going to be used to find the distance, source tz and dest tz from the routesdestwithTZ csv file
               StreamReader reader = new StreamReader(filePath);

               using (reader)
               {
                    string line;
                    line = reader.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         string readsourceAP = row[0];
                         string readdestAP = row[1];

                         if ((readsourceAP == sainput) && (readdestAP == dainput)) // the source and destination airport destination are the same as on the csv
                         {
                              distance = row[2];
                              sourcetz = row[3];
                              desttz = row[4];
                         }

                    }
               }
               reader.Close();

               String writefp = routesfp;
               //reader1 is is used to build the file so we can append a new row to it
               List<String> lines = new List<String>();
               StreamReader reader1 = new StreamReader(writefp);

               if (File.Exists(writefp))
               {
                    using (reader1)
                    {
                         String line;
                         reader1.ReadLine();
                         while ((line = reader1.ReadLine()) != null)
                         {

                              String[] split = line.Split(',');
                              lines.Add(line);
                         }
                    }

                    reader1.Close();


                    //now we have all the data to write the new line
                    string writeLine = "PA " + (Convert.ToInt32(getLastFlightNum()) + 1).ToString() + "," + sainput + "," + dainput + "," + distance + "," + dtinput + "," + sourcetz + "," + atinput + "," + desttz + "," + rtinput;

                    StreamWriter writer = new StreamWriter(writefp, false);
                    using (writer)
                    {
                         foreach (String line in lines)
                              writer.WriteLine(line);
                         writer.WriteLine(writeLine); // writes the last line to the file
                    }
                    writer.Close();

                    Console.WriteLine("Route successfully added!");
                    Thread.Sleep(3000);
                    Console.Clear();
               }

          }

          
          static string getLastFlightNum()
          {
               string flightNum = "";
               // CHANGE FILE PATH TO YOURS
               String filePath = routesfp;
               StreamReader flightNumReader = new StreamReader(filePath);
               using (flightNumReader)
               {
                    string line;
                    while ((line = flightNumReader.ReadLine()) != null)
                    {
                         string[] split = line.Split(',');
                         flightNum = split[0];
                    }
               }
               flightNumReader.Close();
               flightNum = flightNum.Substring(3, 4);

               return flightNum;
          }


          static void removeRoute()
          {
            String filePath = routesfp; // to change to something else later

            //first read in display all of the flights and let the user choose what they want to remove

            StreamReader reader = new StreamReader(filePath);

            using (reader)
            {
                string line;
                line = reader.ReadLine(); // use this line so that we don't start on the title row

                while ((line = reader.ReadLine()) != null)
                {
                    string[] row = line.Split(',');

                    Console.WriteLine(row[0] + " " + row[1] + " " + row[2] + ".");


                }
            }
            reader.Close();


            Console.WriteLine("Enter the route number for the route that you would like to delete (PA Included): ");
            string rninput = Console.ReadLine();
            Console.Clear();


            String writefp = routesfp;
            List<String> lines = new List<String>();
            StreamReader reader1 = new StreamReader(writefp);

            if (File.Exists(writefp))
            {
                using (reader1)
                {
                    String line;
                    line = reader1.ReadLine();
                    lines.Add(line);

                    while ((line = reader1.ReadLine()) != null)
                    {

                        String[] split = line.Split(',');

                        if (split[0] != rninput)
                        {
                            lines.Add(line);
                        }
                        
                    }
                }

                reader1.Close();


                StreamWriter writer = new StreamWriter(writefp, false);
                using (writer)
                {
                    foreach (String line in lines)
                        writer.WriteLine(line);
                }
                writer.Close();

                Console.WriteLine("Route successfully deleted!");
                Thread.Sleep(3000);
                Console.Clear();
            }

        }
          static void startLoadEngineer()
          {
               Console.WriteLine("Load Engineer");
               Console.WriteLine("1) Add Flight Route");
               Console.WriteLine("2) Edit Flight Routes");
               Console.WriteLine("3) Delete Flight Routes");
               Console.WriteLine("4) Sign out");

               int selection = Convert.ToInt32(Console.ReadLine());
               Console.Clear();


            if (selection == 1)
            {
                addRoute();
                startLoadEngineer();
            }
            else if (selection == 2)
            {
                editRoute();
                startLoadEngineer();
            }
            else if (selection == 3)
            {
                removeRoute();
                startLoadEngineer();
            }
            else if (selection == 4)
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
                    string usrID, password, creditcardnum, firstName, lastName, bday, phone, address;
                    //StreamReader readerForUsrID = new StreamReader(accfp);
                    bool isValidCC = true;
                    List<string> lines = new List<string>();

                    Console.WriteLine("Enter into the following fields to make your account");

                    Console.WriteLine("First Name: ");
                    firstName = Console.ReadLine();
                    if(firstName == "")
                    {
                    Console.Clear();
                        Console.WriteLine("Must enter a first name. Try again");
                        startUserLogin();
                        return;
                    }

                    Console.WriteLine("Last Name: ");
                    lastName = Console.ReadLine();
                    if (lastName == "")
                    {
                    Console.Clear();
                    Console.WriteLine("Must enter a last name. Try again");
                        startUserLogin();
                        return;
                    }

                    Console.WriteLine("Address: ");
                    address = Console.ReadLine();
                if (address == "")
                {
                    Console.Clear();
                    Console.WriteLine("Must enter an address. Try again");
                    startUserLogin();
                    return;
                }

                Console.WriteLine("Phone Number: ");
                    phone = Console.ReadLine();
                if (phone == "")
                {
                    Console.Clear();
                    Console.WriteLine("Must enter a phone number. Try again");
                    startUserLogin();
                    return;
                }

                Console.WriteLine("Birthday: ");
                    bday = Console.ReadLine();

                    // code to validate date
                    DateTime newDateVariableName;        // create a new DateTime to save the string to
                    string format = "M/d/yyyy";
                    if (DateTime.TryParseExact(bday, format, new CultureInfo("en-US"), DateTimeStyles.None, out newDateVariableName))
                    {
                        //Do nothing, continue on with bday as it is.
                    }
                    else
                    {
                        // clear console
                        Console.Clear();
                        // display error message
                        Console.WriteLine("Invalid Birthday entered, please try again.");
                        startUserLogin();
                        return;
                    }

                    Console.WriteLine("Credit Card Number: ");

                    //Code to validate credit card number
                    creditcardnum = Console.ReadLine();
                    if (creditcardnum.Length != 16)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Credit Card Number, please enter again.");
                        startUserLogin();
                        return;
                    }
                    else
                    {
                        string ccnLine;
                        string[] ccnSplit;
                        StreamReader readerForCCN = new StreamReader(accfp);

                        using (readerForCCN)
                        {
                            while ((ccnLine = readerForCCN.ReadLine()) != null)
                            {
                                ccnSplit = ccnLine.Split(',');
                                if (ccnSplit[5].Contains(creditcardnum))
                                {
                                    isValidCC = false;
                                    
                                }
                            }
                        }
                        readerForCCN.Close();
                        if(isValidCC == false)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Credit Card Number, please enter again.");
                        startUserLogin();
                        return;
                    }
                    }

                    Console.WriteLine("Password: ");
                    password = Console.ReadLine();
                    if(password == "")
                {
                        Console.Clear();
                        Console.WriteLine("Must enter a password. Try again");
                        startUserLogin();
                        return;
                }

                    usrID = randomNumberGenerator();
                    StreamReader readerForUsrID = new StreamReader(accfp);
                    using (readerForUsrID)
                        {
                            string line;
                            string[] split;

                            while ((line = readerForUsrID.ReadLine()) != null)
                            {
                                split = line.Split(',');

                                //We don't want duplicate user IDs
                                if (split[6].Contains(usrID))
                                {
                                    //Generate a new random number in case we encounter a duplicate (extremely unlikely)
                                    usrID = randomNumberGenerator();
                                }

                                lines.Add(line);
                            }
                        }
                        readerForUsrID.Close();
                        //If something is invalid, and then you try to do it right, we get an IO exception.
                        //StreamWriter writerForUsrID = new StreamWriter(accfp, false);
                string specialAcct = "";
                string pointsSaved = "0";
                string pointsSpent = "0";
                string dollarCredit = "0";
                string hashPass = StrToSHAD(password);
                        string newLine = firstName + "," + lastName + "," + address + "," + phone + "," + bday + "," + creditcardnum + "," + usrID + "," + hashPass
                            + "," + specialAcct + "," + pointsSaved + "," + pointsSpent + "," + dollarCredit;

                    //For appending we don't need to use foreach if we can just append one line.
                    StreamWriter writerForUsrID = new StreamWriter(accfp, false);
                    using (writerForUsrID)
                        {
                            foreach (string line in lines)
                            {
                                writerForUsrID.WriteLine(line);
                            }
                            writerForUsrID.WriteLine(newLine);
                        }
                        writerForUsrID.Close();
                    
                //Return to user login screen
                Console.WriteLine("Account Made! Sign in to use your account.");
                Console.WriteLine("Please save your User ID and Password in a secure location:");
                Console.WriteLine("User ID: {0}", usrID);
                Console.WriteLine("Password: {0}", password);
                Console.WriteLine("Press any key to return to main menu.");
                string returnToMain = Console.ReadLine();
                Console.Clear();
                startUserLogin();
                return;

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

               }
               else if (selection == 3)
               {
                    Console.WriteLine("Logging off");
                    // end the program
               }

          }

        static string randomNumberGenerator()
        {
            Random rng = new Random();
            int userID1;
            int userIDRemaining;
            int fillerZero = 0;
            string userID;
            //Generate the first digit of our 6 digit user ID. This can't be 0. 
            userID1 = rng.Next(1, 10);
            //Generate the remaining 5 digits of our user ID. These can be anything.
            userIDRemaining = rng.Next(00000, 100000);
            //Combine these two values together via concatenation to complete our user ID.
            if (userIDRemaining < 10000)
            {
                userID = userID1.ToString() + fillerZero.ToString() + userIDRemaining.ToString();
                return userID;
            }
            else if (userIDRemaining < 1000)
            {
                userID = userID1.ToString() + fillerZero.ToString() + fillerZero.ToString() + userIDRemaining.ToString();
                return userID;
            }
            else if (userIDRemaining < 100)
            {
                userID = userID1.ToString() + fillerZero.ToString() + fillerZero.ToString() + fillerZero.ToString() + userIDRemaining.ToString();
                return userID;
            }
            else if (userIDRemaining < 10)
            {
                userID = userID1.ToString() + fillerZero.ToString() + fillerZero.ToString() + fillerZero.ToString() + fillerZero.ToString() + userIDRemaining.ToString();
                return userID;
            }
            else
            {
                userID = userID1.ToString() + userIDRemaining.ToString();
                return userID;
            }
        }

        //functionality has been added, but no checks for to make sure information is valid
        static void assignFlight()
          {
               Console.WriteLine("Available airports: BNA CLE DEN DFW DTW LAS LAX LGA MCO ORD PHX SEA");
               Console.WriteLine("Enter a source airport for the flight route you want to change the aircraft for:");
               string usrsourceAP = Console.ReadLine();
               Console.Clear();

               String filePath = routesfp; // to change to something else later

               StreamReader reader = new StreamReader(filePath);

               using (reader)
               {
                    string line;
                    line = reader.ReadLine(); // use this line so that we don't start on the title row

                    while ((line = reader.ReadLine()) != null)
                    {
                         string[] row = line.Split(',');
                         string readsourceAP = row[1];

                         if (readsourceAP == usrsourceAP) // if the user ID exists then exit the loop
                         {
                              Console.WriteLine("Route Number: " + row[0] + ". Source Airport " + row[1] + ". Destination " + row[2] + ". Distance " + row[3] + ". Departure Time " + row[4] + ". Source Timezone " + row[5] + ". Arrival Time " + row[6] + ". Destination Timezone " + row[7]);
                         }

                    }
               }
               reader.Close();

               Console.WriteLine("Enter the Route number for the route you want to edit (include the PA): ");
               string rninput = Console.ReadLine();
               Console.WriteLine("Available Aircrafts: 737, 757, 787");
               Console.WriteLine("Enter what you want to change the aircraft to: ");
               string acinput = Console.ReadLine();
               Console.Clear();

               string path = routesfp;
               List<String> lines = new List<String>();
               StreamReader reader1 = new StreamReader(path);

               if (File.Exists(path))
               {
                    using (reader1)
                    {
                         String line;

                         while ((line = reader1.ReadLine()) != null)
                         {
                              if (line.Contains(","))
                              {
                                   String[] split = line.Split(',');

                                   if (split[0] == rninput) // if the route is the route that the user wants changed
                                   {
                                        split[8] = acinput;
                                        line = String.Join(",", split);
                                   }
                              }

                              lines.Add(line);
                         }
                    }

                    reader1.Close();

                    StreamWriter writer = new StreamWriter(path, false);

                    using (writer)
                    {
                         foreach (String line in lines)
                              writer.WriteLine(line);
                    }
                    writer.Close();

                    Console.WriteLine("Aircraft successfully changed!");
                    Thread.Sleep(3000);
                    Console.Clear();
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
                    assignFlight();
                    startMarkMNG();
               }
               if (selection == 2)
               {
                    signOut();
               }
               return;
          }

        static void flightSummary()
        {
            //FIRST I need to get the income per flight
            //
            String filePath2 = transactionsfp;
            StreamReader reader2 = new StreamReader(filePath2);

            
            Hashtable hash = new Hashtable(); // is a table of unique flightnums and dates so we can add the amtpaids together
            string dateandfn = "";

            using (reader2)
            {
                string line;
                bool firsttime = true;
                line = reader2.ReadLine(); // starts us off in the collumns that contain information
                while ((line = reader2.ReadLine()) != null)
                {
                    firsttime = false;
                    string[] row = line.Split(',');
                    string temp = row[6].Substring(1);
                    double convrow6 = Convert.ToDouble(row[6].Substring(1));
                    dateandfn = row[0] + "_" + row[4]; // the divider is an underscore

                    if (hash.ContainsKey(dateandfn) == false)//if the date and time are unique
                    {
                        hash.Add(dateandfn, convrow6);
                    }
                    else // if they are not unique
                    {
                        hash[dateandfn] = (double)hash[dateandfn] + convrow6;
                    }

                }
                if (firsttime)
                {
                    Console.WriteLine("No flights have been booked yet.");
                    reader2.Close();
                    return;
                }
            }
            reader2.Close();

            // to find the income that each flight has generated, we first want to find all unique flight numbers in booked flights. Then I want to run the unique numbers in transactions and add together all of the flight numbers
            String filePath = transactionsfp;

            //use this reader to find the number of flights booked and the total amount paid
            StreamReader reader = new StreamReader(filePath);
            string readFnum = "";
            int numflights = 0;
            double totalincome = 0.0;
            double readamtpaid = 0.0;


            using (reader)
            {
                string line;

                line = reader.ReadLine(); // starts us off in the collumns that contain information

                while ((line = reader.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    readFnum = row[0];

                    if (row[6].Contains("$"))
                    {
                        readamtpaid = Convert.ToDouble(row[6].Substring(1));
                    }
                    else if (row[6].Contains("points"))
                    {
                        string[] split = row[6].Split(' ');
                        readamtpaid = Convert.ToDouble(split[0]);
                    }

                    totalincome += readamtpaid;
                    numflights++;
                }

            }

            reader.Close();
            Console.WriteLine("Flight Manifest");
            Console.WriteLine("The number of flights that customers have booked are " + numflights + ".");
            Console.WriteLine("The total amount of income generated by the flights is $" + totalincome + ".");
            //this is for displaying the flight and the capacity full
            String filePath1 = bookedFlightsfp;
            StreamReader reader1 = new StreamReader(filePath1);
            string readsource = "", readdestination = "", readdate = "", readseatsleft = "", readac = "", printcapacity = "";
            double capacity = 0;

            using (reader1)
            {
                string line;

                line = reader1.ReadLine(); // starts us off in the collumns that contain information

                while ((line = reader1.ReadLine()) != null)
                {
                    string[] row = line.Split(',');

                    readdate = row[1];
                    readsource = row[2];
                    readdestination = row[3];
                    readac = row[9];
                    readseatsleft = row[10];

                    if (readac == "737") // 189 seats
                    {
                        capacity = (Convert.ToDouble(readseatsleft) / 189.0) * 100.0;
                    }
                    else if (readac == "757") // 200 seats
                    {
                        capacity = (Convert.ToDouble(readseatsleft) / 200.0) * 100.0;
                    }
                    else if (readac == "787") // 242 seats
                    {
                        capacity = (Convert.ToDouble(readseatsleft) / 242.0) * 100.0;
                    }

                    printcapacity = string.Format("{0:0.00}", capacity);
                    string hashKey = readdate + "_" + row[0];

                    if (hash.ContainsKey(hashKey) == false) // if the flight numbers match in booked csv and transactions csv
                    {
                        Console.WriteLine("Date: " + readdate + " Source: " + readsource + " Destination: " + readdestination + " Capacity empty: " + printcapacity + "%. Income earned this flight 0.");
                    }
                    else
                    {
                        Console.WriteLine("Date: " + readdate + " Source: " + readsource + " Destination: " + readdestination + " Capacity empty: " + printcapacity + "%. Income earned this flight " + hash[hashKey] + ".");
                    }



                }
            }
            reader1.Close();

            Thread.Sleep(5000);
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
                    flightSummary();
                    startAccoMNG();
               }
               if (selection == 2)
               {
                    signOut();
               }
          }

        static void flightManifest()
        {
            // look through the booked flights to see if the flight is there, If we know that someone had booked it, then check transactions and see who booked it

            String filePath = bookedFlightsfp;
            //use this reader to find the number of flights booked and the total amount paid
            StreamReader reader = new StreamReader(filePath);
            string readFnum = "";
            bool firsttime = true;

            using (reader)
            {
                string line;

                line = reader.ReadLine(); // starts us off in the collumns that contain information

                while ((line = reader.ReadLine()) != null)
                {
                    firsttime = false;
                    string[] row = line.Split(',');
                    readFnum = row[0];

                    Console.WriteLine(readFnum);
                }
                if (firsttime)
                {
                    Console.WriteLine("No flights have been made.");
                    reader.Close();
                    return;
                }
            }
            reader.Close();

            Console.WriteLine("Enter the flight number you would like to see the flight manifest for: ");
            string fninput = Console.ReadLine();
            Console.Clear();

            //now we know which flight number the user wants to see the flight manifest for. Now check transactions to see who is on the flight

            String filePath1 = transactionsfp;
            //use this reader to find the number of flights booked and the total amount paid
            StreamReader reader1 = new StreamReader(filePath1);
            string readFnum1 = "";
            Console.WriteLine("Flight Manifest");
            Console.WriteLine("Customers on the flight: ");

            using (reader1)
            {
                string line;

                line = reader1.ReadLine(); // starts us off in the collumns that contain information

                while ((line = reader1.ReadLine()) != null)
                {
                    firsttime = false;
                    string[] row = line.Split(',');
                    readFnum1 = row[4];

                    if (readFnum1 == fninput)
                    {
                        Console.WriteLine(row[1] + " " + row[2]);
                    }
                }
            }
            reader1.Close();
            Thread.Sleep(5000);

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
                flightManifest();
                startFligMNG();
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
               String filePath = accfp;        // change to your own filepath
               StreamReader reader = new StreamReader(filePath);

               using (reader)
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
                              if (StrToSHAD(entPass) == csvPassword)
                              {
                                   if (csvAccType == "LE")
                                   {
                                        reader.Close();
                                        startLoadEngineer();
                                        return;
                                   }
                                   else if (csvAccType == "MM")
                                   {
                                        reader.Close();
                                        startMarkMNG();
                                        return;
                                   }
                                   else if (csvAccType == "AM")
                                   {
                                        reader.Close();
                                        startAccoMNG();
                                        return;
                                   }
                                   else if (csvAccType == "FM")
                                   {
                                        reader.Close();
                                        startFligMNG();
                                        return;
                                   }
                                   else
                                   {
                                        reader.Close();
                                        // olivia added for cust methods
                                        fName = row[0];
                                        lName = row[1];
                                        ccnum = row[5];
                                        userID = row[6];
                                        startCustomer();
                                        return;
                                   }
                              }
                              else if (StrToSHAD(entPass) != csvPassword)
                              {
                                   reader.Close();
                                   Console.WriteLine("Incorrect password. Try again or create an account.");
                                   startUserLogin();
                                   return;

                              }
                         }
                    }
                    reader.Close();
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