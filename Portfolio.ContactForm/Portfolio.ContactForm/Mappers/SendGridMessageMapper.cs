using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Portfolio.ContactForm.Models;
using Portfolio.ContactForm.Models.Settings;
using SendGrid.Helpers.Mail;
using System;

namespace Portfolio.ContactForm.Mappers
{
    public class SendGridMessageMapper : ISendGridMessageMapper
    {
        private readonly FunctionOptions _settings;

        public SendGridMessageMapper(IOptions<FunctionOptions> options)
        {
            _settings = options.Value;
        }

        public SendGridMessage MapRequestToMessage(string requestBody)
        {
            if (string.IsNullOrEmpty(_settings.RecipientEmail))
                throw new ArgumentNullException($"Missing value for parameter: {nameof(_settings.RecipientEmail)}");
            if (string.IsNullOrEmpty(_settings.RecipientName))
                throw new ArgumentNullException($"Missing value for parameter: {nameof(_settings.RecipientName)}");

            var jsonInput = JsonConvert.DeserializeObject<EmailMessageRequest>(requestBody);

            var message = new SendGridMessage
            {
                From = new EmailAddress(jsonInput.SenderEmail, jsonInput.SenderName),
                Subject = jsonInput.EmailSubject,
                PlainTextContent = jsonInput.EmailBody,
                HtmlContent = jsonInput.EmailBody
            };

            message.AddTo(_settings.RecipientEmail, _settings.RecipientName);

            return message;
        }
    }
}
