using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Fitness_Planner
{
    internal class AttendanceLog : ISerializableEntity<AttendanceLog>
    {
        private const string MemberFile = "AttendanceLog.txt";

        // ✅ Use generic FileManager with Member
        private List<Member> members = FileManager<Member>.Load("FitnessPlanner_Members.txt");

        public string MemberName { get; set; }
        public DateTime Timestamp { get; set; }

        // Required by FileManager<T>
        public AttendanceLog() { }

        public AttendanceLog(string memberName, DateTime timestamp)
        {
            MemberName ="Member:"+ memberName + " logged in at";
            Timestamp = timestamp;
        }

        public void Attendance()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Attendance Log");
            Console.WriteLine("Please enter the member's name:");
            string memberName = Console.ReadLine();
            DateTime currentTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(memberName))
            {
                Console.WriteLine("Member name cannot be empty. Please try again.");
                return;
            }
            else if (members.Any(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    Console.WriteLine($"Attendance logged for {memberName} at {currentTime}");

                    var log = new AttendanceLog(memberName, currentTime);

                    // ✅ Save serialized entry
                    File.AppendAllText(MemberFile, log.Serialize() + Environment.NewLine);

                    Console.WriteLine("Press any key to continue...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not log attendance: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Member {memberName} not found. Please check the name and try again.");
                Console.WriteLine("Press any key to continue...");
            }
        }

        // ✅ Implement interface
        public string Serialize()
        {
            return $"{MemberName}|{Timestamp:yyyy/MM/dd HH:mm:ss}";
        }

        public AttendanceLog Deserialize(string line)
        {
            var parts = line.Split('|');
            if (parts.Length == 2 && DateTime.TryParse(parts[1], out DateTime ts))
            {
                return new AttendanceLog(parts[0], ts);
            }

            // Return null if bad format (so FileManager skips it)
            return null;
        }
    }
}
