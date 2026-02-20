using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    // This is a "blueprint" for making reports
    // <T> means it works for any type (Member, Payment, etc.)
    public interface IReportGenerator<T>
    {
        // Make the report as text
        string GenerateReport(List<T> items);

        // Save the report text into a file
        void SaveReportToFile(string filePath, string reportContent);

        // Show the report text on the screen
        void DisplayReport(string reportContent);
    }
}