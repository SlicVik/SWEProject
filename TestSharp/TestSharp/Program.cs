using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSharp
{
     internal class Program
     {
          static void Main(string[] args)
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
            else if(input == 1)
            { 
            }

            //  Account Creation Screen

          }
     }
}
