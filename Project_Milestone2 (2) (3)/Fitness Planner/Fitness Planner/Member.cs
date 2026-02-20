using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    // Class: Member
    // Represents a member of the fitness center
    public class Member : ISerializableEntity<Member>
    {
        // Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ID { get; set; }
        public string StaffName { get; set; }
        public string Package { get; set; }
        public int Age { get; set; }

        // Properties with default values
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public DateTime LastPaidDate { get; set; } = DateTime.MinValue;
        public DateTime LastReminderDate { get; set; } = DateTime.MinValue;

        // Method: Serialize
        // Converts the object to a string for saving
        public string Serialize()
        {
            return $"{Name}|{Surname}|{ID}|{StaffName}|{Package}|{Age}|{JoinDate:yyyy/MM/dd}|{LastPaidDate:yyyy/MM/dd}|{LastReminderDate:yyyy/MM/dd}";
        }

        // Method: Deserialize
        // Recreates the object from a string
        public Member Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return null;

            var parts = data.Split('|');
            if (parts.Length < 9) return null;

            return new Member
            {
                Name = parts[0],
                Surname = parts[1],
                ID = parts[2],
                StaffName = parts[3],
                Package = parts[4],
                Age = int.TryParse(parts[5], out int age) ? age : 0,
                JoinDate = DateTime.TryParse(parts[6], out DateTime jd) ? jd : DateTime.Now,
                LastPaidDate = DateTime.TryParse(parts[7], out DateTime lp) ? lp : DateTime.MinValue,
                LastReminderDate = DateTime.TryParse(parts[8], out DateTime lr) ? lr : DateTime.MinValue
            };
        }

        // Method: IsValid
        // Checks if member details are valid
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Surname)
                && !string.IsNullOrWhiteSpace(ID)
                && Age >= 18;
        }
    }
}
