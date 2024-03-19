using AteaScraper.IServices;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AteaScraper
{
    public class Function1
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IAzureStorageService _azureStorageService;

        public Function1(IHttpClientService httpClientService, IAzureStorageService azureStorageService)
        {
            _httpClientService = httpClientService;
            _azureStorageService = azureStorageService;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            var response = await _httpClientService.GetAsync("https://api.publicapis.org/random?auth=null");

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var key = Guid.NewGuid().ToString();
                var requestKey = Guid.NewGuid(); // Generate a new GUID for the request

                await _azureStorageService.AddEntityToTableAsync("", key, true, requestKey);
                await _azureStorageService.UploadBlobAsync($"{key}.json", responseStream);
            }
            else
            {
                // Handle unsuccessful HTTP request
                log.LogError("HTTP request failed.");
            }

            // log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
