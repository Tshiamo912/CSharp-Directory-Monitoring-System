using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fitness_Planner
{
    // Class: PaymentReport
    // Makes reports about payments
    public class PaymentReport : IReportGenerator<Payment>
    {
        // Method: GenerateReport
        // Builds the payment report as text
        public string GenerateReport(List<Payment> payments)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== Payment Report ===");
            report.AppendLine($"Generated on: {DateTime.Now}");
            report.AppendLine("---------------------------------------------------");

            if (payments.Count == 0)
            {
                report.AppendLine("No payments found.");
            }
            else
            {
                foreach (var p in payments)
                {
                    report.AppendLine($"Member: {p.MemberName}, Amount: {p.Amount:C}, Date: {p.DatePaid}");
                }
            }

            return report.ToString();
        }

        // Method: SaveReportToFile
        // Saves the report into a file
        public void SaveReportToFile(string filePath, string reportContent)
        {
            File.WriteAllText(filePath, reportContent);
        }

        // Method: DisplayReport
        // Shows the report on the screen
        public void DisplayReport(string reportContent)
        {
            Console.WriteLine(reportContent);
        }
    }
}
