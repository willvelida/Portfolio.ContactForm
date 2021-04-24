using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.ContactForm;
using Portfolio.ContactForm.Mappers;
using Portfolio.ContactForm.Services;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Portfolio.ContactForm
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddLogging();

            builder.Services.AddSingleton<ISendGridService, SendGridService>();
            builder.Services.AddTransient<ISendGridMessageMapper, SendGridMessageMapper>();
        }
    }
}
