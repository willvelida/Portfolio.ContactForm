using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Models;
using Portfolio.ContactForm.Models.Settings;
using SendGrid.Helpers.Mail;
using System;
using Xunit;

namespace Portfolio.ContactForm.UnitTests.MapperTests
{
    public class SendGridMessageMapperShould
    {
        private Mock<IOptions<FunctionOptions>> _mockOptions;
        private Mock<ILogger<SendGridMessageMapper>> _mockLogger;

        private SendGridMessageMapper _func;

        public SendGridMessageMapperShould()
        {
            _mockOptions = new Mock<IOptions<FunctionOptions>>();
            _mockLogger = new Mock<ILogger<SendGridMessageMapper>>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenMissingRecipientEmailParameter()
        {
            // Arrange
            FunctionOptions functionOptions = new FunctionOptions
            {
                RecipientName = "TestName"
            };
            _mockOptions.Setup(settings => settings.Value).Returns(functionOptions);

            EmailMessageRequest emailMessageRequest = new EmailMessageRequest();
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockOptions.Object, _mockLogger.Object);

            // Act
            Action mapRequestToMessageAction = () => _func.MapRequestToMessage(emailMessageBody);

            // Assert
            mapRequestToMessageAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: RecipientEmail')");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenMissingRecipientEmailName()
        {
            // Arrange
            FunctionOptions functionOptions = new FunctionOptions
            {
                RecipientEmail = "test@test.com"
            };
            _mockOptions.Setup(settings => settings.Value).Returns(functionOptions);

            EmailMessageRequest emailMessageRequest = new EmailMessageRequest();
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockOptions.Object, _mockLogger.Object);

            // Act
            Action mapRequestToMessageAction = () => _func.MapRequestToMessage(emailMessageBody);

            // Assert
            mapRequestToMessageAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: RecipientName')");
        }

        [Fact]
        public void MapMessageRequestToSendGridMessageSuccessfully()
        {
            // Arrange
            FunctionOptions functionOptions = new FunctionOptions
            {
                RecipientEmail = "test@test.com",
                RecipientName = "TestName"
            };

            _mockOptions.Setup(settings => settings.Value).Returns(functionOptions);

            EmailMessageRequest emailMessageRequest = new EmailMessageRequest
            {
                SenderEmail = "sender@sender.com",
                SenderName = "Sender Name",
                EmailSubject = "Test Email",
                EmailBody = "This is a test email"
            };
            string emailMessageBody = JsonConvert.SerializeObject(emailMessageRequest);

            _func = new SendGridMessageMapper(_mockOptions.Object, _mockLogger.Object);

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
