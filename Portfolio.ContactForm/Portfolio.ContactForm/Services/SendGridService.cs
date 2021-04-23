using Microsoft.Extensions.Options;
using Portfolio.ContactForm.Models.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Portfolio.ContactForm.Services
{
    public class SendGridService : ISendGridService
    {
        private SendGridClient _sendGridClient;
        private readonly FunctionOptions _settings;

        public SendGridService(IOptions<FunctionOptions> options)
        {
            _settings = options.Value;
        }

        public SendGridClient Initialize(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))           
                throw new ArgumentNullException($"Missing value for parameter: {nameof(_settings.SendGridAPIKey)}");

            _sendGridClient = new SendGridClient(apiKey);

            return _sendGridClient;
        }

        public async Task<Response> SendEmail(SendGridMessage sendGridMessage)
        {
            _sendGridClient = Initialize(_settings.SendGridAPIKey);
            return await _sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }
}
