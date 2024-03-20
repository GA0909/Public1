using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AteaScraper.Mediators;
using AteaScraper.Blobing;

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

            var request = new BlobQueryRequest
            {
                BlobName = req.Query["blob"]
            };

            var mediator = new Mediator();

            return (IActionResult)await mediator.Send(request);
        }
    }
}
