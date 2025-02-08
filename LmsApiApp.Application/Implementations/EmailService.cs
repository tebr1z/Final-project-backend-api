using LmsApiApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace LmsApiApp.Application.Implementations
{
    public class EmailService : IEmailService
    {
        public void SendEmail(List<string> emails, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Thon Software", "noreply@thonsoftware.com"));
            foreach (var email in emails)
            {
                message.To.Add(new MailboxAddress(email, email));
            }
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect("mail.thonsoftware.com", 465, true);
                    client.Authenticate("noreply@thonsoftware.com", "Thonsoftware005!");
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
