using SendGrid.Helpers.Mail;

namespace Portfolio.ContactForm.Mappers
{
    public interface ISendGridMessageMapper
    {
        /// <summary>
        /// Maps the incoming request body to a SendGridMessage object.
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        SendGridMessage MapRequestToMessage(string requestBody);
    }
}
