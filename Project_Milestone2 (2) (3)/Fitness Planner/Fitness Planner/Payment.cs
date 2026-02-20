using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    // Class-Payment
    // Represents a payment made by a member
    public class Payment : ISerializableEntity<Payment>
    {
        // Properties
        public string MemberName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }

        // Method: Serialize
        // Converts payment to text for saving
        public string Serialize()
        {
            return $"{MemberName}|{Amount}|{DatePaid:yyyy/MM/dd HH:mm:ss}";
        }

        // Method- Deserialize
        // Recreates a payment from text
        public Payment Deserialize(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            var parts = line.Split('|');
            if (parts.Length != 3) return null;

            if (!decimal.TryParse(parts[1], out decimal amount)) return null;
            if (!DateTime.TryParse(parts[2], out DateTime datePaid)) return null;

            return new Payment
            {
                MemberName = parts[0],
                Amount = amount,
                DatePaid = datePaid
            };
        }
    }
}
