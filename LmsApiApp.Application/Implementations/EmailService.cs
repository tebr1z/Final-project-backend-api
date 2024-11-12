using LmsApiApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(List<string> emails, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tabrizyh@code.edu.az", "Lms APP");
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("tabrizyh@code.edu.az", "hacc owpo nqwl mpiu\r\n"); // Google giris password


            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
