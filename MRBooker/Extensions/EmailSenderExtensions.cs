using System.Threading.Tasks;

namespace MRBooker.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            var emailBody = $"<html><body><p>Please confirm your account by clicking <a href=\"{link}\">this</a> link</body></html>";
            return emailSender.SendEmailAsync(email, "Confirm your email", emailBody);
        }
    }
}
