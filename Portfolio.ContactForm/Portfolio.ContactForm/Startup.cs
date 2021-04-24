using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.ContactForm;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Models.Settings;
using Portfolio.ContactForm.Services;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Portfolio.ContactForm
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<FunctionOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("FunctionOptions").Bind(settings);
                });
            builder.Services.AddLogging();

            builder.Services.AddSingleton<ISendGridService, SendGridService>();
            builder.Services.AddTransient<ISendGridMessageMapper, SendGridMessageMapper>();
        }
    }
}
