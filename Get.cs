using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace logs
{
    public static class Get
    {
        [FunctionName("Get")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("LogEntities")] CloudTable table,
            ILogger log)
        {
            var logs = new List<LogEntity>();
            TableContinuationToken token = default;
            do {
                var query = new TableQuery<LogEntity>();
                var results = await table.ExecuteQuerySegmentedAsync(query, token);
                logs.AddRange(results);
                token = results.ContinuationToken;
            } while (token != default);

            return new OkObjectResult(
                logs.Select(result => new
                    {
                        pathname = result.PartitionKey.Replace("|", "/"),
                        referrer = result.Referrer,
                        datetime = result.DateTime
                    }));
        }
    }
}
