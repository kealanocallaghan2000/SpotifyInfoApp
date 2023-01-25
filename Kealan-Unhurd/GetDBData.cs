using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Net.Http;

namespace Kealan_Unhurd
{
    // Returns the user a database of Artists and their Spotify ID
    public static class GetDBData
    {
        [FunctionName("GetDBData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                using (CosmosClient cosmosClient = new CosmosClient("https://unhurd-test-cosmosdb.documents.azure.com:443/", "egqVjppVDozl0TQuDuxlEIZ20AeCxu1I31O0DtYAMV6LHRBOwsSlzFuOb4y605G2qajALuUgTR1PACDb3DQ25w=="))
                {
                    Container container = cosmosClient.GetContainer("tech-test-db", "artists");
                   
                    var items = container.GetItemLinqQueryable<TodoItem>();
                    var iterator = items.ToFeedIterator();
                    var results = await iterator.ReadNextAsync();

                    return new OkObjectResult(results);
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"Exception {e.InnerException} message {e.Message}");
            }
            
        }
    }
}
