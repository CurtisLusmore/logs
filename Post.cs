using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace logs
{
    public static class Post
    {
        [FunctionName("Post")]
        [return: Table("LogEntities")]
        public static async Task<LogEntity> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string pathname = data?.pathname;
            string referrer = data?.referrer;

            return new LogEntity
            {
                PartitionKey = pathname.Replace("/", "|"),
                RowKey = Guid.NewGuid().ToString(),
                Referrer = referrer,
                DateTime = DateTime.UtcNow
            };
        }
    }
}
