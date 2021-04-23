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
            _sendGridClient = new SendGridClient(_settings.SendGridAPIKey) ?? throw new ArgumentNullException($"Missing value for parameter: {nameof(_settings.SendGridAPIKey)}");
        }

        public async Task<Response> SendEmail(SendGridMessage sendGridMessage)
        {
            return await _sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }
}
