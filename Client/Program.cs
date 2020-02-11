using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // call api
            using (var client = new HttpClient()) {
                var accessToken = await GetAccessToken();
                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("Access error");
                    Console.ReadLine();
                    return;
                }

                client.SetBearerToken(accessToken);
                Console.WriteLine(accessToken);
                //var response = await client.GetAsync("http://localhost:5001/api/identity");
                //if (!response.IsSuccessStatusCode)
                //{
                //    Console.WriteLine(response.StatusCode);
                //}
                //else
                //{
                //    var content = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine(JArray.Parse(content));
                //}
                Console.ReadLine();
            };
                
        }

        private static async Task<string> GetAccessToken() {
            Console.WriteLine("Get-AccessToken!");
            using (var client = new HttpClient()) {
                var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return string.Empty;
                }

                // request token
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1"
                });
                Console.WriteLine("End-AccessToken");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return string.Empty;
                }
                Console.WriteLine(tokenResponse.Json);
                return tokenResponse.AccessToken;
            }
                
     
        }
    }
}
