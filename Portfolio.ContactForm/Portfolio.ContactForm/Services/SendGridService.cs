using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Portfolio.ContactForm.Services
{
    public class SendGridService : ISendGridService
    {
        private SendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SendGridClient Initialize(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException($"Missing value for parameter: 'SendGridAPIKey'");

            _sendGridClient = new SendGridClient(apiKey);

            return _sendGridClient;
        }

        public async Task<Response> SendEmail(SendGridMessage sendGridMessage)
        {
            _sendGridClient = Initialize(_configuration["SendGridAPIKey"]);
            return await _sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }
}
