using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fitness_Planner
{
    // Class: PaymentFileManager
    // Handles saving and loading payment records from file
    public static class PaymentFileManager
    {
        // Field
        private const string PaymentFile = "Payments.txt";

        // Method: SavePayment
        // Saves one payment to file
        public static void SavePayment(Payment payment)
        {
            File.AppendAllLines(PaymentFile, new[] { payment.Serialize() });
        }

        // Method: LoadPayments
        // Loads all payments from file
        public static List<Payment> LoadPayments()
        {
            if (!File.Exists(PaymentFile)) return new List<Payment>();

            return File.ReadAllLines(PaymentFile)
                       .Select(line => new Payment().Deserialize(line))
                       .Where(p => p != null)
                       .ToList();
        }
    }
}
