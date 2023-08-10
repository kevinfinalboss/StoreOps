using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace StoreOps.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            string host = "smtp.hostinger.com";
            int port = 587;
            bool enableSsl = true;
            string username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            string password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

            _smtpClient = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(username, password)
            };
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            CancellationToken cancellationToken
        )
        {
            var message = new MailMessage
            {
                From = new MailAddress("noreply@kevindev.com.br"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await _smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
