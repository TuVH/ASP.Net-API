using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace IdentityDemo.API.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMail(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hoangtu13092001@gmail.com", "JWT Authentication");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
