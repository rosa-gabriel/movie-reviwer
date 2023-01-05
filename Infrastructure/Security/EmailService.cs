using Application.Core;
using Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Security
{
    public class EmailService : IEmailService
    {
        public readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            this._config = config;
        }
        public void Send(Email emailInfo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(this._config.GetSection("EmailAddress").Value));
            email.To.Add(MailboxAddress.Parse(emailInfo.To));
            email.Subject = emailInfo.Subjetct;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailInfo.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(this._config.GetSection("EmailHost").Value, 465, true);
            smtp.Authenticate(this._config.GetSection("EmailAddress").Value, this._config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}