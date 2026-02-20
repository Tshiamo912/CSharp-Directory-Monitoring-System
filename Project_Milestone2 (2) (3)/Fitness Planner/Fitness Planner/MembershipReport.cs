using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fitness_Planner
{
    // Class: MembershipReport
    // Generates and handles membership reports
    public class MembershipReport : IReportGenerator<Member>
    {
        // Method: GenerateReport
        // Builds a report from a list of members
        public string GenerateReport(List<Member> members)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== MEMBERSHIP REPORT ===");
            report.AppendLine($"Generated on: {DateTime.Now}");
            report.AppendLine("------------------------------------------------------------------------------------");

            if (members.Count == 0)
            {
                report.AppendLine("No members found.");
            }
            else
            {
                foreach (var m in members)
                {
                    report.AppendLine($"Name       : {m.Name} {m.Surname}");
                    report.AppendLine($"ID         : {m.ID}");
                    report.AppendLine($"Staff      : {m.StaffName}");
                    report.AppendLine($"Package    : {m.Package}");
                    report.AppendLine($"Age        : {m.Age}");
                    report.AppendLine($"Join Date  : {m.JoinDate:yyyy/MM/dd}");
                    report.AppendLine($"Last Paid  : {m.LastPaidDate:yyyy/MM/dd}");
                    report.AppendLine($"Reminder   : {(m.LastReminderDate == DateTime.MinValue ? "No reminder sent" : m.LastReminderDate.ToString("yyyy/MM/dd"))}");
                    report.AppendLine("------------------------------------------------------------------------------------");
                }
            }

            return report.ToString();
        }

        // Method: SaveReportToFile
        // Saves report into a file
        public void SaveReportToFile(string filePath, string reportContent)
        {
            File.WriteAllText(filePath, reportContent);
        }

        // Method: DisplayReport
        // Prints report on console
        public void DisplayReport(string reportContent)
        {
            Console.WriteLine(reportContent);
        }
    }
}
