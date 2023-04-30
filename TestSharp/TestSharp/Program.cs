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
        public static string routesfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Routes - Sheet1.csv";
        public static string routesTZfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\RouteDistWithTZ - Sheet1.csv";
        public static string accfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Accounts - Accounts.csv";
        public static string transactionsfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Transactions.csv";
        public static string bookedFlightsfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\BookedFlightRecords.csv";

        //Garrett's filepaths
        /*public static string routesfp = @"C:\Users\knowl\Downloads\Routes - Sheet1.csv";
        public static string routesTZfp = @"C:\Users\knowl\Downloads\RoutesDistWithTZ - Sheet1.csv";
        public static string accfp = @"C:\Users\knowl\Downloads\Accounts - Accounts.csv";
        public static string transactionsfp = @"C:\Users\knowl\Downloads\Transactions.csv";
        public static string bookedFlightsfp = @"C:\Users\knowl\Downloads\BookedFlightRecords.csv";*/

        //Olivia's filepaths
        /*public static string accfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Accounts.csv";
        public static string transactionsfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Transactions.csv";
        public static string routesfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Routes.csv";
        public static string bookedFlightsfp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\BookedFlightRecords.csv";*/


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
                    cancelAFlight();             
               }
               // if 5) Change Account Details
               // GET RID OF THIS ENTIRE CASE
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
                    //payWithDollars();      now has parameters, change
               }
               else if (payInput == 2)
               {
                    //payWithPoints();
               }
               else
               {
                    // see note on exceptions in bookAFlight
                    Console.WriteLine("Enter a valid command (then handle exceptions here)");
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
                    //Console.Clear();
                    Console.WriteLine("{0}", dstAirportCode);
                    oneWayDate();
                    return;
               }
          }

          static void oneWayDate()
          {
               //Console.Clear();
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
                         Console.WriteLine("Display one way called");
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
               bool recordExists = false;
               string flightNumber;
               int userSelFlight;
               //string departDate; TO UPDATE FLIGHT RECORD LATER
               // change filepath to match where your Accounts.csv file resides
               //String filePath = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Routes.csv";
               StreamReader routeReader = new StreamReader(routesfp);
               List<string> directRoutes = new List<string>();        // store all valid direct src/dest combos in this array before more checks
               List<string> displayTracker = new List<string>();      // used to keep track of which flight the customer selects to book, so the CSVs can be updated
               //string candidateFlight;       // keeps the departure date and flight number

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

                         // handles direct options
                         // if a direct option has been found...
                         if (srcAP == srcAirportCode && destAP == dstAirportCode)
                         {
                              directRoutes.Add(line);
                         }

                    }
               }
               routeReader.Close();

               int i = 1;
               foreach (string line in directRoutes)
               {
                    string[] split = line.Split(',');
                    string candidateFlight;       // keeps the departure date and flight number

                    // need to check the flights file to see if it exists for this date 
                    // check flight number and deptDate
                    recordExists = flightHasRecord(split[1], deptDate);
                    // if it exists...
                    if (recordExists == true)
                    {
                         //we don't have to generate a flight number, but we need to retrieve it
                         // we need to check if the plane is full
                         isNotFull = checkSeats(split[1], deptDate);     // returns false if there are no seats left
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
                         int storedOptNum = Convert.ToInt32((string)canSplit[0]);
                         int APDist = Convert.ToInt32((string)canSplit[5]);
                         // if we found the flight the cust chose to book
                         if (storedOptNum == userSelFlight)
                         {
                              double ticketPrice = 58 + (0.12 * APDist); // base calculation

                              // check times for discounts
                              string tempDepTime = canSplit[6];
                              string tempArrTime = canSplit[8];
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
                              double ticketPricePoints = ticketPrice * 100;
                              Console.WriteLine("Flight price: ${0} or {1} points", ticketPrice, ticketPricePoints);
                              Console.WriteLine("Select an payment option");
                              Console.WriteLine("1) Pay with dollars");
                              Console.WriteLine("2) Pay with points");
                              Console.WriteLine("Enter a number to select an option");
                              int payInput = Convert.ToInt32(Console.ReadLine());

                              // separate this out later...to handle case of invalid input
                              if (payInput == 1)
                              {
                                   payWithDollars(candidateFlight, ticketPrice);
                                   return;
                              }
                              else if (payInput == 2)
                              {
                                   payWithPoints(candidateFlight, ticketPrice, ticketPricePoints);
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
               //String filePath = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\BookedFlightRecords.csv";
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
               //String filePath = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\BookedFlightRecords.csv";
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

          static void payWithDollars(string allFlightData, double cost)
          {
               string[] split = allFlightData.Split(',');
               bool recordExists = false;
               Console.WriteLine("Booking Details:");
               Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
               Console.WriteLine("Amount due: ${0}", cost);
               Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
               string confirm = Console.ReadLine();
               if (confirm == "Y")
               {
                    // let's update all relevant CSV files...
                    // starting with BookedFlightRecords
                    //String filePath = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\BookedFlightRecords.csv";
                    List<string> lines = new List<string>();
                    StreamReader recordReader = new StreamReader(bookedFlightsfp);
                    string newLine;

                    if (File.Exists(bookedFlightsfp))
                    {
                         using (recordReader)
                         {
                              string line;
                              //recordReader.ReadLine();
                              while ((line = recordReader.ReadLine()) != null)
                              {
                                   string[] splitRecord = line.Split(',');
                                   if (splitRecord[0] == split[2] && splitRecord[1] == split[1])
                                   {
                                        recordExists = true;
                                        int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                        seatsLeft--;
                                        string seatsLeftString = Convert.ToString(seatsLeft);
                                        splitRecord[10] = seatsLeftString;
                                        line = string.Join(",", splitRecord);
                                   }
                                   lines.Add(line);
                              }
                         }
                         recordReader.Close();

                         if (recordExists == true)
                         {
                              StreamWriter recordWriter = new StreamWriter(bookedFlightsfp, false);

                              using (recordWriter)
                              {
                                   foreach (string line in lines)
                                   {
                                        recordWriter.WriteLine(line);
                                   }
                              }
                              recordWriter.Close();
                         }
                         else if (recordExists == false)
                         {
                              string[] splitAllFlightData = allFlightData.Split(',');
                              string aircraft = splitAllFlightData[10];
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
                              newLine = splitAllFlightData[2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[3] + "," + splitAllFlightData[4]
                                   + "," + splitAllFlightData[5] + "," + splitAllFlightData[6] + "," + splitAllFlightData[7] + "," + splitAllFlightData[8]
                                   + "," + splitAllFlightData[9] + "," + splitAllFlightData[10] + "," + numSeatsLeft;

                              StreamWriter recordWriter = new StreamWriter(bookedFlightsfp, false);
                              using (recordWriter)
                              {
                                   foreach (string line in lines)
                                   {
                                        recordWriter.WriteLine(line);
                                   }
                                   recordWriter.WriteLine(newLine);
                              }
                              recordWriter.Close();
                         }
                    }

                    // now add transaction to transactions.csv
                    string[] lastSplit = allFlightData.Split(',');
                    string flightNumber = lastSplit[2];
                    string newTrans = sysDate + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + "$" + cost;

                    String fp = @"C:\Users\12482\Documents\School\Spring 2023\EECS 3550 Software Engineering\Transactions.csv";
                    List<string> transactions = new List<string>();
                    StreamReader transactionReader = new StreamReader(fp);

                    if (File.Exists(fp))
                    {
                         using (transactionReader)
                         {
                              string line;
                              //recordReader.ReadLine();
                              while ((line = transactionReader.ReadLine()) != null)
                              {
                                   transactions.Add(line);
                              }
                         }
                         transactionReader.Close();

                         StreamWriter transactionWriter = new StreamWriter(fp, false);
                         using (transactionWriter)
                         {
                              foreach (string line in transactions)
                              {
                                   transactionWriter.WriteLine(line);
                              }
                              transactionWriter.WriteLine(newTrans);
                         }
                         transactionWriter.Close();
                    }

                    // now add points to their account for giving us $
                    double ptBal;
                    double newPts = cost / 10;
                    newPts = Math.Round(newPts, 0, MidpointRounding.ToEven);         // rounding
                    //filePath = accfp;
                    List<string> accLines = new List<string>();
                    StreamReader accReader = new StreamReader(accfp);

                    if (File.Exists(accfp))
                    {
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
                         recordReader.Close();

                         StreamWriter recordWriter = new StreamWriter(accfp, false);

                         using (recordWriter)
                         {
                              foreach (string line in accLines)
                              {
                                   recordWriter.WriteLine(line);
                              }
                         }
                         recordWriter.Close();
                    }

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

          static void payWithPoints(string allFlightData, double cost, double points)
          {
               // read Accounts.csv to check if the customer has enough points
               // if they have nough points, proceed
               // if they do not have enough points, ask if they want to book with dollars or cancel booking process
               bool hasEnoughPoints = false;
               double savedPts = 0;
               String filePath = accfp;
               StreamReader pointReader = new StreamReader(filePath);
               using (pointReader)
               {
                    string line;
                    while ((line = pointReader.ReadLine()) != null)
                    {
                         string[] split = line.Split(',');
                         if (split[6] == userID)
                         {
                              double.TryParse(split[9], out savedPts);
                              if (savedPts >= points)
                              {
                                   hasEnoughPoints = true;
                              }
                              break;
                         }
                    }
               }
               pointReader.Close();

               if (hasEnoughPoints == false)
               {
                    Console.WriteLine("Insufficient number of points: ");
                    Console.WriteLine("You have {0} points available and the flight costs {1} points.", savedPts, points);
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1) Pay with dollars.");
                    Console.WriteLine("2) Cancel booking process and return to homepage.");
                    int selection = Convert.ToInt32(Console.ReadLine());

                    if (selection == 1)
                    {
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

               if (hasEnoughPoints == true)
               {
                    string[] split = allFlightData.Split(',');
                    bool recordExists = false;
                    Console.WriteLine("Booking Details:");
                    Console.WriteLine("{0}: {1}        {2} to {3}      {4} to {5}", split[1], split[2], split[3], split[4], split[6], split[8]);
                    Console.WriteLine("Amount due: {0} points", points);
                    Console.WriteLine("Review booking summary. Enter 'Y' to reserve flight or 'N' to cancel. 'N' will take you back to the homepage.");
                    string confirm = Console.ReadLine();

                    if (confirm == "Y")
                    {
                         // let's update all relevant CSV files...
                         // starting with BookedFlightRecords
                         filePath = bookedFlightsfp;
                         List<string> lines = new List<string>();
                         StreamReader recordReader = new StreamReader(filePath);
                         string newLine;

                         if (File.Exists(filePath))
                         {
                              using (recordReader)
                              {
                                   string line;
                                   while ((line = recordReader.ReadLine()) != null)
                                   {
                                        string[] splitRecord = line.Split(',');
                                        if (splitRecord[0] == split[0] && splitRecord[1] == split[1])
                                        {
                                             recordExists = true;
                                             int seatsLeft = Convert.ToInt32(splitRecord[10]);
                                             seatsLeft++;
                                             string seatsLeftString = Convert.ToString(seatsLeft);
                                             splitRecord[10] = seatsLeftString;
                                             line = string.Join(",", splitRecord);
                                        }
                                        lines.Add(line);
                                   }
                              }
                              recordReader.Close();

                              if (recordExists == true)
                              {
                                   StreamWriter recordWriter = new StreamWriter(filePath, false);

                                   using (recordWriter)
                                   {
                                        foreach (string line in lines)
                                        {
                                             recordWriter.WriteLine(line);
                                        }
                                   }
                                   recordWriter.Close();
                              }
                              else if (recordExists == false)
                              {
                                   string[] splitAllFlightData = allFlightData.Split(',');
                                   string aircraft = splitAllFlightData[10];
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
                                   newLine = splitAllFlightData[2] + "," + splitAllFlightData[1] + "," + splitAllFlightData[3] + "," + splitAllFlightData[4]
                                        + "," + splitAllFlightData[5] + "," + splitAllFlightData[6] + "," + splitAllFlightData[7] + "," + splitAllFlightData[8]
                                        + "," + splitAllFlightData[9] + "," + splitAllFlightData[10] + "," + numSeatsLeft;

                                   StreamWriter recordWriter = new StreamWriter(filePath, false);
                                   using (recordWriter)
                                   {
                                        foreach (string line in lines)
                                        {
                                             recordWriter.WriteLine(line);
                                        }
                                        recordWriter.WriteLine(newLine);
                                   }
                                   recordWriter.Close();
                              }
                         }

                         // now add transaction to transactions.csv
                         string[] lastSplit = allFlightData.Split(',');
                         string flightNumber = lastSplit[2];
                         string newTrans = sysDate + "," + fName + "," + lName + "," + ccnum + "," + flightNumber + "," + deptDate + "," + points + "points";

                         String fp = transactionsfp;
                         List<string> transactions = new List<string>();
                         StreamReader transactionReader = new StreamReader(fp);

                         if (File.Exists(fp))
                         {
                              using (transactionReader)
                              {
                                   string line;
                                   //recordReader.ReadLine();
                                   while ((line = transactionReader.ReadLine()) != null)
                                   {
                                        transactions.Add(line);
                                   }
                              }
                              transactionReader.Close();

                              StreamWriter transactionWriter = new StreamWriter(fp, false);
                              using (transactionWriter)
                              {
                                   foreach (string line in transactions)
                                   {
                                        transactionWriter.WriteLine(line);
                                   }
                                   transactionWriter.WriteLine(newTrans);
                              }
                              transactionWriter.Close();
                         }

                         // now remove the points they spent from PointsSaved, then add them to PointsSpent
                         double PointsSaved;
                         double PointsSpent;
                         filePath = accfp;
                         List<string> accLines = new List<string>();
                         StreamReader accReader = new StreamReader(filePath);

                         if (File.Exists(filePath))
                         {
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
                                             PointsSaved -= points;
                                             string PointsSavedString = Convert.ToString(PointsSaved);
                                             splitRecord[9] = PointsSavedString;

                                             // add to points spent and reassign in index
                                             double.TryParse(splitRecord[10], out PointsSpent);
                                             PointsSpent += points;
                                             string PointsSpentString = Convert.ToString(PointsSpent);
                                             splitRecord[10] = PointsSpentString;

                                             line = string.Join(",", splitRecord);
                                        }
                                        accLines.Add(line);
                                   }
                              }
                              recordReader.Close();

                              StreamWriter recordWriter = new StreamWriter(filePath, false);

                              using (recordWriter)
                              {
                                   foreach (string line in accLines)
                                   {
                                        recordWriter.WriteLine(line);
                                   }
                              }
                              recordWriter.Close();
                         }


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