using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage.Table;
using TableEntity = Azure.Data.Tables.TableEntity;
using AteaScraper.Mediator;
using AteaScraper.Logging;

namespace AteaScraper
{
    public static class Log
    {
        [FunctionName("Log")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            IMediator mediator)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = new LogQueryRequest
            {
                From = req.Query["from"],
                To = req.Query["to"]
            };

            return (IActionResult)await mediator.Send(request);
        }
    }
}
