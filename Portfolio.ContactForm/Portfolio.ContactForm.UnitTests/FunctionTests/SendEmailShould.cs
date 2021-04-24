using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Portfolio.ContactForm.Functions;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Models;
using Portfolio.ContactForm.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Portfolio.ContactForm.UnitTests.FunctionTests
{
    public class SendEmailShould
    {
        private Mock<ILogger<SendEmail>> _mockLogger;
        private Mock<ISendGridMessageMapper> _mockSendGridMessageMapper;
        private Mock<ISendGridService> _mockSendGridService;
        private Mock<HttpRequest> _mockHttpRequest;

        private SendEmail _func;

        public SendEmailShould()
        {
            _mockLogger = new Mock<ILogger<SendEmail>>();
            _mockSendGridMessageMapper = new Mock<ISendGridMessageMapper>();
            _mockSendGridService = new Mock<ISendGridService>();
            _mockHttpRequest = new Mock<HttpRequest>();

            _func = new SendEmail(_mockLogger.Object, _mockSendGridMessageMapper.Object, _mockSendGridService.Object);
        }

        [Fact]
        public async Task ReturnOkResultWhenSendEmailSuccessfullySendsEmail()
        {
            // Arrange
            var emailRequest = new EmailMessageRequest
            {
                SenderEmail = "test@test.com",
                SenderName = "Test Sender",
                EmailSubject = "Test Subject",
                EmailBody = "This is a test message"
            };

            Response expectedResponse = new Response(System.Net.HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>());

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(emailRequest));
            MemoryStream memeoryStream = new MemoryStream(byteArray);
            _mockHttpRequest.Setup(r => r.Body).Returns(memeoryStream);
            _mockSendGridMessageMapper.Setup(m => m.MapRequestToMessage(It.IsAny<string>())).Returns(It.IsAny<SendGridMessage>());
            _mockSendGridService.Setup(s => s.SendEmail(It.IsAny<SendGridMessage>())).ReturnsAsync(expectedResponse);

            // Act
            var response = await _func.Run(_mockHttpRequest.Object);

            // Assert
            Assert.Equal(typeof(OkResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(200, responseAsStatusCode.StatusCode);

            _mockSendGridMessageMapper.Verify(m => m.MapRequestToMessage(It.IsAny<string>()), Times.Once);
            _mockSendGridService.Verify(s => s.SendEmail(It.IsAny<SendGridMessage>()), Times.Once);
        }

        [Fact]
        public async Task ThrowBadRequestResultWhenSendGridServiceThrowsBadRequest()
        {
            // Arrange
            var emailRequest = new EmailMessageRequest
            {
                SenderEmail = "test@test.com",
                SenderName = "Test Sender",
                EmailSubject = "Test Subject",
                EmailBody = "This is a test message"
            };
            Response expectedResponse = new Response(System.Net.HttpStatusCode.BadRequest, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>());

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(emailRequest));
            MemoryStream memeoryStream = new MemoryStream(byteArray);
            _mockHttpRequest.Setup(r => r.Body).Returns(memeoryStream);
            _mockSendGridMessageMapper.Setup(m => m.MapRequestToMessage(It.IsAny<string>())).Returns(It.IsAny<SendGridMessage>());
            _mockSendGridService.Setup(s => s.SendEmail(It.IsAny<SendGridMessage>())).ReturnsAsync(expectedResponse);

            // Act
            var response = await _func.Run(_mockHttpRequest.Object);

            // Assert
            Assert.Equal(typeof(BadRequestResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(400, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw500WhenSendEmailFails()
        {
            // Arrange
            var emailRequest = new EmailMessageRequest();

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(emailRequest));
            MemoryStream memeoryStream = new MemoryStream(byteArray);
            _mockHttpRequest.Setup(r => r.Body).Returns(memeoryStream);
            _mockSendGridMessageMapper.Setup(m => m.MapRequestToMessage(It.IsAny<string>())).Returns(It.IsAny<SendGridMessage>());
            _mockSendGridService.Setup(s => s.SendEmail(It.IsAny<SendGridMessage>())).ThrowsAsync(It.IsAny<Exception>());

            // Act
            Func<Task> responseAction = async () => await _func.Run(_mockHttpRequest.Object);

            // Assert
            responseAction.Should().ThrowAsync<Exception>();
        }
    }
}
