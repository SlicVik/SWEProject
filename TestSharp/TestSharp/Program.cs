using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

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

        // CUSTOMER METHODS BEGIN HERE
        public static string routesfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Routes - Sheet1.csv";
        public static string routesTZfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\RouteDistWithTZ - Sheet1.csv";
        public static string accfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Accounts - Accounts.csv";

        //Vikram's filepaths
        /*public static string routesfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Routes - Sheet1.csv";
        public static string routesTZfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\RouteDistWithTZ - Sheet1.csv";
        public static string accfp = @"C:\Users\vadda\OneDrive\Documents\OS and sus\Accounts - Accounts.csv";*/
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
                // code for cancel a flight               
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
                string writeLine =  "PA " + (Convert.ToInt32(getLastFlightNum()) + 1).ToString() + "," + sainput + "," + dainput + "," + distance + "," + dtinput + "," + sourcetz + "," + atinput + "," + desttz + "," + rtinput;

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

           //Vikram - The way I used this is that input = nothing, output = the last 4 digits of the Routenum in the Routes csv. This would mean that this program would only work with 9999 routes.
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
                addRoute();
                startLoadEngineer();
            }
            if (selection == 2)
            {
                editRoute();
                startLoadEngineer();
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

            }
            else if (selection == 3)
            {
                Console.WriteLine("Logging off");
                // end the program
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
            String filePath = accfp;
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
                                startCustomer();
                                return;
                            }
                        }
                        else if (StrToSHAD(entPass)!= csvPassword)
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
