using System.Net;
using System.Net.Mail;

namespace JobSeeker.Services
{
    public class EmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "youremail@gmail.com";
        private readonly string smtpPass = "yourpassword";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(smtpServer, smtpPort);
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Jika ingin mengirim email dalam format HTML
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            client.Dispose();
        }
    }
}