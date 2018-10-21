using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MRBooker.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            // we can use this smtp for now, but we need to create an a
            client.Credentials = new NetworkCredential("themrbooker@gmail.com", "P@ssw0rd123");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@themrbooker.com");
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.SendAsync(mailMessage, null);

            return Task.CompletedTask;
        }
    }
}
