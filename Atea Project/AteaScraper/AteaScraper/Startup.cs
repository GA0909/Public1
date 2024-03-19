using AteaScraper.IServices;
using AteaScraper.Services;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
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

            // Register TableServiceClient and BlobContainerClient
            builder.Services.AddSingleton(serviceProvider =>
            {
                var connectionString = "UseDevelopmentStorage=true"; // Update with your connection string
                return new TableServiceClient(connectionString);
            });

            builder.Services.AddSingleton(serviceProvider =>
            {
                var connectionString = "UseDevelopmentStorage=true"; // Update with your connection string
                var containerName = "atea";
                return new BlobContainerClient(connectionString, containerName);
            });

            // Register AzureStorageService
            builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();

            // Register HttpClientService
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();
        }
    }
}
