using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Fitness_Planner
{
    // Class: User
    // Base class for all users in the fitness system
    public class User
    {
        // Properties
        public string StaffUsername { get; set; }
        public string StaffPassword { get; set; }

        // Fields
        protected string Username, Password;
        public bool IsLoggedIn;

        // Constant
        private const string StaffFile = "Staff.txt";

        // Static collection of staff accounts
        private static List<User> staff;

        // Static constructor (runs once at program start)
        static User()
        {
            // If Staff.txt doesn’t exist, create it with a default admin/admin account
            if (!File.Exists(StaffFile))
            {
                File.WriteAllText(StaffFile, "Staff Name:admin,Password:admin" + Environment.NewLine);
            }

            // Load staff users into memory
            staff = LoadStaff();
        }

        // Method: GetLoginDetails
        // Gets login credentials from the user
        public void GetLoginDetails()
        {
            Console.WriteLine("Please Login Using Staff Credentials:\n");

            Console.Write("Enter Staff Username: ");
            this.Username = Console.ReadLine();

            Console.Write("Enter Password: ");
            this.Password = ReadMaskedInput();

            // Attempt login
            Login(this.Username, this.Password);
        }

        // Method: Display (virtual, to be overridden by child classes)
        public virtual void Display() { }

        // Method: ReadMaskedInput
        // Reads password input while hiding characters with *
        public static string ReadMaskedInput()
        {
            string result = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace && result.Length > 0)
                {
                    result = result.Substring(0, result.Length - 1);
                    Console.Write("\b \b"); // erase *
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    result += key.KeyChar;
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return result;
        }

        // Method: LoadStaff
        // Loads staff accounts from file
        public static List<User> LoadStaff()
        {
            var users = new List<User>();

            if (!File.Exists(StaffFile))
                return users;

            foreach (var line in File.ReadAllLines(StaffFile))
            {
                var user = FromStaffString(line);
                if (user != null)
                    users.Add(user);
            }

            return users;
        }

        // Method: FromStaffString
        // Converts a line from Staff.txt into a User object
        public static User FromStaffString(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            try
            {
                string[] parts = line.Split(',');

                if (parts.Length < 2)
                    return null;

                return new User
                {
                    StaffUsername = parts[0].Replace("Staff Name:", "").Trim(),
                    StaffPassword = parts[1].Replace("Password:", "").Trim()
                };
            }
            catch
            {
                return null;
            }
        }

        // Method: Login
        // Handles the login process
        public void Login(string n, string p)
        {
            this.Username = n?.Trim();
            this.Password = p?.Trim();

            if (string.IsNullOrWhiteSpace(this.Username))
            {
                Console.WriteLine("Please enter a Username");
                Thread.Sleep(1000);
                GetLoginDetails();
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Password))
            {
                Console.WriteLine("Please enter a Password");
                Thread.Sleep(1000);
                GetLoginDetails();
                return;
            }

            // Check against loaded staff accounts
            if (staff.Any(u => u.StaffUsername.Equals(this.Username, StringComparison.OrdinalIgnoreCase)
                            && u.StaffPassword.Equals(this.Password, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("\n>>>>> Login Successful! <<<<<");
                this.IsLoggedIn = true;
                Thread.Sleep(1000);

                // Proceed to staff menu
                this.Display();
            }
            else
            {
                Console.WriteLine("Invalid username or password. Try again.");
                Thread.Sleep(1500);
                GetLoginDetails();
            }
        }

        // Method: Logout
        // Ends the program
        public void Logout()
        {
            Console.WriteLine("Logging out...");
            Thread.Sleep(1000);
            Environment.Exit(0); // Close the application
        }
    }
}
