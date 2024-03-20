using AteaScraper.Blobing;
using AteaScraper.IServices;
using AteaScraper.Logging;
using AteaScraper.Mediator;
using AteaScraper.Services;
using AteaScraper.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AteaScraper.Startup))]

namespace AteaScraper
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register HttpClient
            builder.Services.AddHttpClient();

            // Register AzureStorageService
            builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();

            // Register HttpClientService
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();

            builder.Services.AddSingleton<LogQueryHandler>();
            builder.Services.AddSingleton<BlobQueryHandler>();
            builder.Services.AddSingleton<IValidator<LogQueryRequest>, LogQueryValidator>();
            builder.Services.AddSingleton<IMediator, Mediator.Mediator>();
        }

    }
}
