using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebApiResourceOwnerFlowSpecs.IntergrationTests
{
    public static class HttpClientExtensions
    {
        public static async Task AuthenticateWith(this HttpClient httpClient, string username, string password)
        {
            var tokenDetails = new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            };

            var tokenPostData = new FormUrlEncodedContent(tokenDetails);
            var tokenResult = await httpClient.PostAsync("/token", tokenPostData);

            if (tokenResult.IsSuccessStatusCode)
            {
                var body = JObject.Parse(await tokenResult.Content.ReadAsStringAsync());
                var token = (string) body["access_token"];

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}