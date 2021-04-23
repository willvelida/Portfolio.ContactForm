using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Portfolio.ContactForm.Services
{
    public interface ISendGridService
    {
        /// <summary>
        /// Sends the provided SendGridMessage content as an email.
        /// </summary>
        /// <param name="sendGridMessage"></param>
        /// <returns></returns>
        Task<Response> SendEmail(SendGridMessage sendGridMessage);
    }
}
