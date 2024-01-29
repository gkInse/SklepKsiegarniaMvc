using System.Net.Mail;
using System.Net;
using SklepKsiegarniaMvcUI.Areas.Identity.Pages.Account;
using Microsoft.Extensions.Logging;

namespace SklepKsiegarniaMvcUI.Services
{
    public class EmailSender : IEmailSender
    {

        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //var client = new SmtpClient("smtp.ethereal.email", 587)
            var client = new SmtpClient("smtp.wp.pl", 587)
            {
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("krztyniec@wp.pl", "Krztyn1ec")

            };

            _logger.LogWarning("client ok");

            return client.SendMailAsync(
                new MailMessage(from: "krztyniec@wp.pl",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
