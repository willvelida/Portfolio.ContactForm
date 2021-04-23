using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Portfolio.ContactForm.Functions
{
    public class SendEmail
    {
        private readonly ILogger<SendEmail> _logger;
        private readonly ISendGridMessageMapper _sendGridMessageMapper;
        private readonly ISendGridService _sendGridService;

        public SendEmail(
            ILogger<SendEmail> logger,
            ISendGridMessageMapper sendGridMessageMapper,
            ISendGridService sendGridService)
        {
            _logger = logger;
            _sendGridMessageMapper = sendGridMessageMapper;
            _sendGridService = sendGridService;
        }

        [FunctionName(nameof(SendEmail))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SendEmail")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                string messageRequest = await new StreamReader(req.Body).ReadToEndAsync();

                var message = _sendGridMessageMapper.MapRequestToMessage(messageRequest);

                var response = await _sendGridService.SendEmail(message);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"{nameof(SendEmail)} received a bad request");
                    result = new BadRequestResult();
                }
                else
                {
                    result = new OkResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(SendEmail)}: {ex}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}

