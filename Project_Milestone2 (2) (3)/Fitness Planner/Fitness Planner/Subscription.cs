using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fitness_Planner
{
    // Class
    internal class Subscription
    {
        // Constants (Prices for each subscription package)
        public const double BronzePrice = 199.99;
        public const double SilverPrice = 299.99;
        public const double GoldPrice = 399.99;

        // Properties (store chosen package and activation date)
        public string PackageName { get; set; }
        public DateTime SubActivated { get; set; }

        // Event (triggered when subscription is activated)
        public event EventHandler<string> OnSubscriptionActivated;

        // Enum (available package choices)
        private enum Packages
        {
            Bronze = 1,
            Silver,
            Gold
        }

        // Method: SubscriptionDetail
        // Lets staff choose a subscription package
        public void SubscriptionDetail()
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("      CHOOSE SUBSCRIPTION PACKAGE  ");
            Console.WriteLine("===================================");
            Console.WriteLine($"1. Bronze Package - {BronzePrice:C}");
            Console.WriteLine($"2. Silver Package - {SilverPrice:C}");
            Console.WriteLine($"3. Gold Package   - {GoldPrice:C}");
            Console.WriteLine("-----------------------------------");
            Console.Write("Enter your choice (1-3): ");

            // Validate user input
            if (!int.TryParse(Console.ReadLine(), out int pick))
            {
                Console.WriteLine("Invalid input! Defaulting to Bronze package...");
                pick = 1;
            }

            Packages main = (Packages)pick;

            // Decide which package to assign
            switch (main)
            {
                case Packages.Bronze:
                    PackageName = "Bronze";
                    SubActivated = DateTime.Now;
                    TriggerEvent($"Bronze subscription activated at {SubActivated}");
                    break;

                case Packages.Silver:
                    PackageName = "Silver";
                    SubActivated = DateTime.Now;
                    TriggerEvent($"Silver subscription activated at {SubActivated}");
                    break;

                case Packages.Gold:
                    PackageName = "Gold";
                    SubActivated = DateTime.Now;
                    TriggerEvent($"Gold subscription activated at {SubActivated}");
                    break;

                default:
                    PackageName = "Bronze"; // fallback
                    SubActivated = DateTime.Now;
                    TriggerEvent($"Bronze subscription activated at {SubActivated}");
                    break;
            }
        }

        // Method: TriggerEvent
        // Helper to notify subscribers when a package is activated
        protected virtual void TriggerEvent(string message)
        {
            OnSubscriptionActivated?.Invoke(this, message);
        }
    }
}
