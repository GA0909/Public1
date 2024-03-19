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

namespace AteaScraper
{
    public static class Log
    {
        [FunctionName("Log")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get query parameters for time period (from/to)
            string fromParam = req.Query["from"];
            string toParam = req.Query["to"];

            // Parse query parameters
            DateTime from, to;
            if (!DateTime.TryParse(fromParam, out from) || !DateTime.TryParse(toParam, out to))
            {
                return new BadRequestResult();
            }
            // Getting the Azure Table
            var serviceClient = new TableServiceClient("UseDevelopmentStorage=true");
            var table = serviceClient.GetTableClient("atea");
            await table.CreateIfNotExistsAsync();
            // The From-To Query
            string filter = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, from),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, to));
            //Gotten results
            var result = table.Query<TableEntity>(filter);
            //Return results
            return new OkObjectResult(result); 
        }
    }
}
