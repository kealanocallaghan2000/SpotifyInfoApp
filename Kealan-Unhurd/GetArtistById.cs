using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Kealan_Unhurd
{
    // Schema for deserializing response
    public class Root
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GetArtistById
    {
        [FunctionName("GetArtistById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var connectionString = "https://unhurd-test-cosmosdb.documents.azure.com:443/";

            var client = new CosmosClientBuilder(connectionString, "egqVjppVDozl0TQuDuxlEIZ20AeCxu1I31O0DtYAMV6LHRBOwsSlzFuOb4y605G2qajALuUgTR1PACDb3DQ25w==")
                                .WithSerializerOptions(new CosmosSerializationOptions
                                {
                                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                                })
                                .Build();

            var container = client.GetContainer("tech-test-db", "artists");
            string myAccessToken = await getSpotifyToken();           
            HttpClient http = new HttpClient();          
            string id= req.Query["artist"]; //ID retrieved from the URL
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAccessToken);
            
            string responseMessage = "";            
            string url = "https://api.spotify.com/v1/artists/" + id;
        
            try
            {
                using (var response = http.GetAsync(url))
                {
                    string responseBody = response.Result.Content.ReadAsStringAsync().Result;
                    responseMessage = responseBody;
                    Root formattedData = JsonConvert.DeserializeObject<Root>(responseBody);
                    //Creates item in database
                    var cosmosAddArtist =  await container.CreateItemAsync(formattedData);                   
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"Exception {e.InnerException} message {e.Message}");
            }

            return new OkObjectResult(responseMessage);
        }

        // This method generates a new authorization token each time the application is run. 
        // This solves the issue of the token expiring after one hour
        public async static Task<String> getSpotifyToken()
        {
            // Client and secret from registered spotify
            var spotifyClient = "9ac258b04c64448ea73b2e232a725a06";
            var spotifySecret = "a0a371b281f2401b886c74426e53fb6d";

            var httpClient = new HttpClient();
        
            // Set the necessary headers
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(spotifyClient + ":" + spotifySecret)));

            // Create the request body
            var requestBody = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // Make the request to the token endpoint
            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", requestBody);
            var responseContent = await response.Content.ReadAsStringAsync();

            byte[] byteArray = Encoding.ASCII.GetBytes(responseContent);

            // Parse the response to get the token
            var textResponse = Encoding.UTF8.GetString(byteArray);

            int pFrom = textResponse.IndexOf("access_token") + "access_token".Length;
            int pTo = textResponse.LastIndexOf("token_type");
            string semiToken = textResponse.Substring(pFrom, pTo - pFrom);
            string myAccessToken = semiToken.Substring(3, semiToken.Length - 6);
            
            return myAccessToken;
        }
    }
}
