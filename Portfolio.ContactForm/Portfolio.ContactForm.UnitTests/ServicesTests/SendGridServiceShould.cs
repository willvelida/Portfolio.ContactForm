using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Portfolio.ContactForm.Models.Settings;
using Portfolio.ContactForm.Services;
using System;
using Xunit;

namespace Portfolio.ContactForm.UnitTests.ServicesTests
{
    public class SendGridServiceShould
    {
        private Mock<IOptions<FunctionOptions>> _mockOptions;

        private SendGridService _sut;

        public SendGridServiceShould()
        {
            _mockOptions = new Mock<IOptions<FunctionOptions>>();
            FunctionOptions functionOptions = new FunctionOptions
            {

            };
            _mockOptions.Setup(settings => settings.Value).Returns(functionOptions);
            _sut = new SendGridService(_mockOptions.Object);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenSendGridAPIKeyIsNotProvided()
        {
            // Act
            Action initializeAction = () => _sut.Initialize(null);

            // Assert
            initializeAction.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'Missing value for parameter: SendGridAPIKey')");
        }
    }
}
