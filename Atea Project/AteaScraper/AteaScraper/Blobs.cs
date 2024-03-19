
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

namespace AteaScraper
{
    public static class Blobs
    {
        [FunctionName("Blobs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            // Query for blob
            var blobName = req.Query["blob"];
            // The Blob Container info
            string connection = "UseDevelopmentStorage=true";
            string containerName = "atea";
            var blobClient = new BlobContainerClient(connection, containerName);
            await blobClient.CreateIfNotExistsAsync();
            var blob = blobClient.GetBlobClient($"{blobName}.json");
            //check if blob exists
            if(!await blob.ExistsAsync())
            {                return new NotFoundResult();
            }
            //Downloading the result of search
            var blobResponse = await blob.DownloadContentAsync();
            //Return the results of search
            return new OkObjectResult(blobResponse.Value.Content.ToString());
        }
    }
}
