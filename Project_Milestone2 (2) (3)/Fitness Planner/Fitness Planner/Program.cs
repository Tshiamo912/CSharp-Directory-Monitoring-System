using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Fitness_Planner
{
    // Class: Program
    // Main entry point of the application
    public class Program
    {
        // Method: Main
        // Entry point
        public static void Main(string[] args)
        {
            try
            {
                ShowWelcomeScreen();
                RunApplication();
            }
            catch (Exception ex)
            {
                ShowErrorScreen(ex);
            }
        }

        // Method: LoadingAnimation
        // Shows a loading spinner
        public static void LoadingAnimation()
        {
            Console.CursorVisible = false;
            Console.Write("Loading ");

            string[] spinner = { "|", "/", "-", "\\" };
            for (int i = 0; i < 10; i++)
            {
                Console.Write(spinner[i % 4]);
                Thread.Sleep(200);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }

            Console.CursorVisible = true;
            Console.WriteLine();
        }

        // Method: RunApplication
        // Core logic
        private static void RunApplication()
        {
            Staff staff = new Staff();
            staff.GetLoginDetails(); // Login
        }

        // Method: ShowWelcomeScreen
        // Displays title and loading
        private static void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("    FITNESS PLANNER SYSTEM");
            Console.WriteLine("=================================");
            LoadingAnimation();
        }

        // Method: ShowErrorScreen
        // Displays error info
        private static void ShowErrorScreen(Exception ex)
        {
            Console.Clear();
            Console.WriteLine("=== APPLICATION ERROR ===");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            Console.WriteLine("\nPlease restart the application.");
            Console.ReadKey();
        }

        // Method: ShowExitScreen
        // Displays exit message
        private static void ShowExitScreen()
        {
            Console.Clear();
            Console.WriteLine("Thank you for using Fitness Planner!");
            Console.WriteLine("The application will now close...");
            Thread.Sleep(2000);
        }
    }
}
