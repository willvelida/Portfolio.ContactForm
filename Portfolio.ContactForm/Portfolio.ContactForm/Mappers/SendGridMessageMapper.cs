using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Portfolio.ContactForm.Models;
using SendGrid.Helpers.Mail;
using System;

namespace Portfolio.ContactForm.Mappers
{
    public class SendGridMessageMapper : ISendGridMessageMapper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendGridMessageMapper> _logger;

        public SendGridMessageMapper(
            IConfiguration configuration,
            ILogger<SendGridMessageMapper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public SendGridMessage MapRequestToMessage(string requestBody)
        {
            _logger.LogInformation($"RecipientEmail: {_configuration["RecipientEmail"]}");
            _logger.LogInformation($"RecipientName: {_configuration["RecipientName"]}");
            if (string.IsNullOrEmpty(_configuration["RecipientEmail"]))
                throw new ArgumentNullException($"Missing value for parameter: 'RecipientEmail'");
            if (string.IsNullOrEmpty(_configuration["RecipientName"]))
                throw new ArgumentNullException($"Missing value for parameter: 'RecipientName'");

            var jsonInput = JsonConvert.DeserializeObject<EmailMessageRequest>(requestBody);

            var message = new SendGridMessage
            {
                From = new EmailAddress(jsonInput.SenderEmail, jsonInput.SenderName),
                Subject = jsonInput.EmailSubject,
                PlainTextContent = jsonInput.EmailBody,
                HtmlContent = jsonInput.EmailBody
            };

            message.AddTo(_configuration["RecipientEmail"], _configuration["RecipientName"]);

            return message;
        }
    }
}
