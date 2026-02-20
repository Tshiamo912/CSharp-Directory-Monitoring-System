using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    // Class: ReminderService
    // Checks which members need payment reminders
    public class ReminderService : IReportGenerator<Member>
    {
        // Field: list of members
        private List<Member> _members;

        // Field: list of payments
        private List<Payment> _payments;

        // Field: timer for periodic checks
        private Timer _reminderTimer;

        // Field: tracks if service is running
        private bool _isRunning;

        // Field: check interval in minutes
        private const int CheckInterval = 60;

        // Constructor
        public ReminderService(List<Member> members, List<Payment> payments)
        {
            _members = members;
            _payments = payments;
            StartService();
        }

        // Method: StartService
        // Starts background timer
        public void StartService()
        {
            _isRunning = true;
            _reminderTimer = new Timer(CheckReminders, null, 0, CheckInterval * 60 * 1000);
            Console.WriteLine("Reminder service started.");
        }

        // Method: StopService
        // Stops background timer
        public void StopService()
        {
            _isRunning = false;
            _reminderTimer?.Dispose();
            Console.WriteLine("Reminder service stopped.");
        }

        // Method: CheckReminders
        // Finds members needing reminders
        private void CheckReminders(object state)
        {
            if (!_isRunning) return;

            var membersNeedingReminders = _members
                .Where(NeedsRenewalReminder)
                .ToList();

            if (membersNeedingReminders.Any())
            {
                Console.WriteLine($"[Reminder] Sending reminders to {membersNeedingReminders.Count} members...");
                SendReminders(membersNeedingReminders);
            }
        }

        // Method: NeedsRenewalReminder
        // True if never paid or over 30 days
        private bool NeedsRenewalReminder(Member member)
        {
            return member.LastPaidDate == DateTime.MinValue ||
                   (DateTime.Now - member.LastPaidDate).TotalDays > 30;
        }

        // Method: SendReminders
        // Sends reminder to overdue members
        private void SendReminders(List<Member> members)
        {
            foreach (var member in members)
            {
                Console.WriteLine($"Reminder sent to: {member.Name} {member.Surname}");
                member.LastReminderDate = DateTime.Now;
            }
        }

        // Method: GenerateReport
        // Makes report of overdue members
        public string GenerateReport(List<Member> members)
        {
            var report = new StringBuilder();
            report.AppendLine("===== RENEWAL REMINDER REPORT =====");
            report.AppendLine($"Generated on: {DateTime.Now}");
            report.AppendLine("Members needing attention:");
            report.AppendLine("----------------------------------------------------------");

            var overdueMembers = members.Where(NeedsRenewalReminder).ToList();

            if (overdueMembers.Count == 0)
            {
                report.AppendLine("All members are up to date with payments.");
            }
            else
            {
                foreach (var member in overdueMembers)
                {
                    report.AppendLine($"Name: {member.Name} {member.Surname}");
                    report.AppendLine($"Age: {member.Age}");
                    report.AppendLine($"Joined: {member.JoinDate:yyyy-MM-dd}");
                    report.AppendLine($"Last Paid: {(member.LastPaidDate == DateTime.MinValue ? "Never" : member.LastPaidDate.ToString("yyyy-MM-dd"))}");
                    report.AppendLine($"Days Since Payment: {(member.LastPaidDate == DateTime.MinValue ? "N/A" : (DateTime.Now - member.LastPaidDate).Days.ToString())}");
                    report.AppendLine("----------------------------------------------------------");
                }
            }

            return report.ToString();
        }

        // Method: SaveReportToFile
        // Writes report into file
        public void SaveReportToFile(string filePath, string reportContent)
        {
            File.WriteAllText(filePath, reportContent);
        }

        // Method: DisplayReport
        // Shows report on console
        public void DisplayReport(string reportContent)
        {
            Console.WriteLine(reportContent);
        }
    }
}
