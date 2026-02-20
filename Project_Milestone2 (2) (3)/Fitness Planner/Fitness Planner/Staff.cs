using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    // Class: Staff (inherits User)
    public class Staff : User
    {
        // Field: member list
        private List<Member> members;

        // Field: payment list
        private List<Payment> payments;

        // Field: file path for members
        private const string MemberFile = "FitnessPlanner_Members.txt";

        // Field: file path for payments
        private const string PaymentFile = "Payments.txt";

        // Constructor: loads saved members and payments
        public Staff()
        {
            members = FileManager<Member>.Load(MemberFile);
            payments = FileManager<Payment>.Load(PaymentFile);
        }

        // Method: MemberDetails
        // Asks staff for new member info
        public void MemberDetails()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Add New Member ===");

                Console.Write("Enter Member Name: ");
                string memberName = Console.ReadLine();

                Console.Write("Enter Member Surname: ");
                string memberSurname = Console.ReadLine();

                Console.Write("Enter Member ID: ");
                string memberID = Console.ReadLine();

                Console.Write("Enter Member Age: ");
                if (!int.TryParse(Console.ReadLine(), out int memberAge) || memberAge < 18)
                {
                    Console.WriteLine("Member must be at least 18 years old.");
                    Thread.Sleep(1500);
                    return;
                }

                bool memberExists = AddMember(memberName, memberSurname, memberID, memberAge);
                if (!memberExists) break;
            }
        }

        // Method: AddMember
        // Adds new member if not already existing
        public bool AddMember(string memberName, string memberSurname, string memberID, int memberAge)
        {
            if (members.Any(m => m.ID.Equals(memberID, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("A member with this ID already exists.");
                Thread.Sleep(1500);
                return true;
            }

            Subscription subscription = new Subscription();
            subscription.OnSubscriptionActivated += (sender, msg) =>
            {
                Console.WriteLine($"[EVENT] {msg}");
                Console.WriteLine("Reminder: Please process payment for this member.");
            };
            subscription.SubscriptionDetail();

            var member = new Member
            {
                Name = memberName,
                Surname = memberSurname,
                ID = memberID,
                Age = memberAge,
                StaffName = Username,
                Package = subscription.PackageName,
                JoinDate = DateTime.Now,
                LastPaidDate = DateTime.MinValue
            };

            FileManager<Member>.Save(MemberFile, member);
            members.Add(member);

            Console.Clear();
            Console.WriteLine("Adding Member...");
            Program.LoadingAnimation();

            Console.WriteLine("Member added successfully!");
            Console.WriteLine("Press any key to return to the Staff Menu...");
            Console.ReadKey();

            return false;
        }

        // Method: RemoveMember
        // Removes member by name + ID
        public void RemoveMember()
        {
            Console.Clear();
            Console.WriteLine("=== Remove Member ===");

            Console.Write("Enter the Member's Name: ");
            string memberName = Console.ReadLine();

            Console.Write("Enter the Member's ID: ");
            string memberId = Console.ReadLine();

            FileManager<Member>.Remove(MemberFile, m =>
                m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) &&
                m.ID.Equals(memberId, StringComparison.OrdinalIgnoreCase));

            members = FileManager<Member>.Load(MemberFile);

            Console.WriteLine($"Checked and removed member {memberName} (ID: {memberId}) if they existed.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        // Method: ProcessPayment
        // Records payment and updates member
        public void ProcessPayment()
        {
            Console.Clear();
            Console.WriteLine("=== Process Payment ===");

            Console.Write("Enter Member ID: ");
            string id = Console.ReadLine();

            var member = members.FirstOrDefault(m => m.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (member == null)
            {
                Console.WriteLine("No member found with that ID.");
                Thread.Sleep(1500);
                return;
            }

            Console.Write("Enter Payment Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid amount.");
                Thread.Sleep(1000);
                return;
            }

            var payment = new Payment
            {
                MemberName = member.Name + " " + member.Surname,
                Amount = amount,
                DatePaid = DateTime.Now
            };

            FileManager<Payment>.Save(PaymentFile, payment);
            payments.Add(payment);

            member.LastPaidDate = DateTime.Now;
            FileManager<Member>.Remove(MemberFile, m => m.ID == member.ID);
            FileManager<Member>.Save(MemberFile, member);

            Console.WriteLine($"Payment of {amount:C} recorded for {member.Name} {member.Surname}.");
            Console.WriteLine("Press any key to return to the Staff Menu...");
            Console.ReadKey();
        }

        // Enum: StaffMenu
        enum StaffMenu
        {
            AddMember = 1,
            RemoveMember,
            ProcessPayment,
            GenerateReport,
            MemberLogin,
            AddStaff,
            Exit
        }

        // Method: Display
        // Main staff menu
        public override void Display()
        {
            while (IsLoggedIn)
            {
                Console.Clear();
                Console.WriteLine("=== Staff Menu ===");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. Remove Member");
                Console.WriteLine("3. Process Payment");
                Console.WriteLine("4. Generate Report");
                Console.WriteLine("5. Member Login");
                Console.WriteLine("6. Add Staff");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int pick))
                {
                    Console.WriteLine("Invalid input.");
                    Thread.Sleep(1000);
                    continue;
                }

                switch ((StaffMenu)pick)
                {
                    case StaffMenu.AddMember:
                        MemberDetails();
                        break;

                    case StaffMenu.RemoveMember:
                        RemoveMember();
                        break;

                    case StaffMenu.ProcessPayment:
                        ProcessPayment();
                        break;

                    case StaffMenu.GenerateReport:
                        Console.Clear();
                        Console.WriteLine("Which report?");
                        Console.WriteLine("1. Membership Report");
                        Console.WriteLine("2. Payment Report");
                        Console.WriteLine("3. Reminder Report");
                        Console.Write("Enter choice: ");

                        if (!int.TryParse(Console.ReadLine(), out int reportChoice))
                        {
                            Console.WriteLine("Invalid input.");
                            Thread.Sleep(1000);
                            break;
                        }

                        if (reportChoice == 1)
                        {
                            var loadedMembers = FileManager<Member>.Load(MemberFile);
                            var memberReport = new MembershipReport();
                            string content = memberReport.GenerateReport(loadedMembers);
                            memberReport.DisplayReport(content);
                            memberReport.SaveReportToFile("MembershipReport.txt", content);
                        }
                        else if (reportChoice == 2)
                        {
                            var loadedPayments = FileManager<Payment>.Load(PaymentFile);
                            var paymentReport = new PaymentReport();
                            string content = paymentReport.GenerateReport(loadedPayments);
                            paymentReport.DisplayReport(content);
                            paymentReport.SaveReportToFile("PaymentReport.txt", content);
                        }
                        else if (reportChoice == 3)
                        {
                            var loadedMembers = FileManager<Member>.Load(MemberFile);
                            var loadedPayments = FileManager<Payment>.Load(PaymentFile);
                            var reminderService = new ReminderService(loadedMembers, loadedPayments);
                            string content = reminderService.GenerateReport(loadedMembers);
                            reminderService.DisplayReport(content);
                            reminderService.SaveReportToFile("ReminderReport.txt", content);
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.");
                        }

                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;

                    case StaffMenu.MemberLogin:
                        var attendanceLog = new AttendanceLog();
                        attendanceLog.Attendance();
                        Console.ReadKey();
                        break;

                    case StaffMenu.Exit:
                        Console.WriteLine("Exiting...");
                        Console.Write("Confirm exit? Y/N: ");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Logging out...");
                            Logout();
                        }
                        break;

                    case StaffMenu.AddStaff:
                        Console.Clear();
                        Console.WriteLine("=== Add New Staff ===");

                        Console.Write("Enter New Staff Username: ");
                        string newUsername = Console.ReadLine();

                        Console.Write("Enter New Staff Password: ");
                        string newPassword = ReadMaskedInput();

                        File.AppendAllText("Staff.txt", $"Staff Name:{newUsername},Password:{newPassword}{Environment.NewLine}");

                        Console.WriteLine($"New staff account '{newUsername}' created.");
                        Thread.Sleep(1500);
                        break;

                    default:
                        Console.WriteLine("Invalid menu choice.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
    }
}
