using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using ClosedXML.Excel;

namespace UniversityClassAutomation.Utilities
{
    public class ExcelUtility
    {
        public List<OpenClass> GetOpenClasses(string filePath)
        {
            List<OpenClass> openClasses = new List<OpenClass>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // Assuming data is in the first sheet
                Console.WriteLine("Displaying Worksheet Contents:");
                foreach (var row in worksheet.RowsUsed())
                {
                    foreach (var cell in row.Cells())
                    {
                        Console.Write($"{cell.Value}\t");
                    }
                    Console.WriteLine();
                }

                // Assuming the first row contains headers
                var headerRow = worksheet.Row(1);
                var columnMapping = new Dictionary<string, int>();
                foreach (var cell in headerRow.Cells())
                {
                    columnMapping[cell.GetValue<string>()] = cell.Address.ColumnNumber;
                }

                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    string status = row.Cell(columnMapping["ENRL_STATUS"]).GetValue<string>();
                    int capacity = row.Cell(columnMapping["ENRL_CAP"]).GetValue<int>();
                    int totalEnrolled = row.Cell(columnMapping["ENRL_TOT"]).GetValue<int>();
                    int availableSeats = capacity - totalEnrolled;

                    if (status == "Open" && availableSeats > 1)
                    {
                        openClasses.Add(new OpenClass
                        {
                            ClassTitle = row.Cell(columnMapping["CW_CLASS_TITLE"]).GetValue<string>(),
                            AvailableSeats = availableSeats
                        });
                    }
                }
            }

            SendEmail(openClasses);
            return openClasses;
        }

        private void SendEmail(List<OpenClass> openClasses)
        {
            string emailBody = BuildEmailBody(openClasses);

            string smtpServer = ConfigReader.GetValue("EmailSettings", "SmtpServer");
            int smtpPort = int.Parse(ConfigReader.GetValue("EmailSettings", "SmtpPort"));
            string senderEmail = ConfigReader.GetValue("EmailSettings", "SenderEmail");
            string senderPassword = ConfigReader.GetValue("EmailSettings", "SenderPassword");
            string recipientEmail = ConfigReader.GetValue("EmailSettings", "RecipientEmail");

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Open Classes Notification",
                    Body = emailBody,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(recipientEmail); // Use recipient email from config

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }


        private string BuildEmailBody(List<OpenClass> openClasses)
        {
            var sb = new StringBuilder();
            sb.AppendLine("The following classes are open with more than one available seat:\n");

            foreach (var openClass in openClasses)
            {
                sb.AppendLine($"Class: {openClass.ClassTitle}, Available Seats: {openClass.AvailableSeats}");
            }

            return sb.ToString();
        }
    }

    public class OpenClass
    {
        public string ClassTitle { get; set; }
        public int AvailableSeats { get; set; }
    }
}
