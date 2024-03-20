using AteaScraper.Validators;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableEntity = Azure.Data.Tables.TableEntity;

namespace AteaScraper.Logging
{
    public class LogQueryHandler
    {
        private readonly TableServiceClient _tableServiceClient;

        public LogQueryHandler(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task<IActionResult> Handle(LogQueryRequest request)
        {
            // Validate query parameters
            var validator = new LogQueryValidator();
            if (!validator.Validate(request))
            {
                return new BadRequestResult();
            }

            // Get Azure Table reference
            var table = _tableServiceClient.GetTableClient("atea");
            await table.CreateIfNotExistsAsync();

            // Generate query filter
            string filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, DateTime.Parse(request.From)),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, DateTime.Parse(request.To)));

            // Execute query
            var result = table.Query<TableEntity>(filter);

            return new OkObjectResult(result);
        }
    }
}
