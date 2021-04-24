using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Models;
using SendGrid.Helpers.Mail;
using System;
using Xunit;

namespace Portfolio.ContactForm.UnitTests.MapperTests
{
    public class SendGridMessageMapperShould
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<SendGridMessageMapper>> _mockLogger;

        private SendGridMessageMapper _func;

        public SendGridMessageMapperShould()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<SendGridMessageMapper>>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenMissingRecipientEmailParameter()
        {
            // Arrange
            EmailMessageRequest emailMessageRequest = new EmailMessageRequest();
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            Action mapRequestToMessageAction = () => _func.MapRequestToMessage(emailMessageBody);

            // Assert
            mapRequestToMessageAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: 'RecipientEmail'')");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenMissingRecipientEmailName()
        {
            // Arrange
            _mockConfiguration.Setup(c => c["RecipientEmail"]).Returns("test@test.com");
            EmailMessageRequest emailMessageRequest = new EmailMessageRequest();
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            Action mapRequestToMessageAction = () => _func.MapRequestToMessage(emailMessageBody);

            // Assert
            mapRequestToMessageAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: 'RecipientName'')");
        }

        [Fact]
        public void MapMessageRequestToSendGridMessageSuccessfully()
        {
            // Arrange
            _mockConfiguration.Setup(c => c["RecipientEmail"]).Returns("test@test.com");
            _mockConfiguration.Setup(c => c["RecipientName"]).Returns("Test Name");
            EmailMessageRequest emailMessageRequest = new EmailMessageRequest
            {
                SenderEmail = "sender@sender.com",
                SenderName = "Sender Name",
                EmailSubject = "Test Email",
                EmailBody = "This is a test email"
            };
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var response = _func.MapRequestToMessage(emailMessageBody);

            // Assert
            Assert.Equal(new EmailAddress(emailMessageRequest.SenderEmail, emailMessageRequest.SenderName), response.From);
            Assert.Equal(emailMessageRequest.EmailSubject, response.Subject);
            Assert.Equal(emailMessageRequest.EmailBody, response.PlainTextContent);
            Assert.Equal(emailMessageRequest.EmailBody, response.HtmlContent);
        }
    }
}
