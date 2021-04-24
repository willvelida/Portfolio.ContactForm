using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Portfolio.ContactForm.Services;
using System;
using Xunit;

namespace Portfolio.ContactForm.UnitTests.ServicesTests
{
    public class SendGridServiceShould
    {
        private Mock<IConfiguration> _mockConfiguration;

        private SendGridService _sut;

        public SendGridServiceShould()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _sut = new SendGridService(_mockConfiguration.Object);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenSendGridAPIKeyIsNotProvided()
        {
            // Act
            Action initializeAction = () => _sut.Initialize(null);

            // Assert
            initializeAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: 'SendGridAPIKey'')");
        }
    }
}
