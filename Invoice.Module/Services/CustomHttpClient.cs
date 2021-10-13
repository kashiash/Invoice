using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Invoice.Module.Services
{
    public class CustomHttpClient
    {
        private readonly HttpClient client;

        public CustomHttpClient()
        {
            client = new HttpClient();
        }

        public async Task<string> GetStringAsync(string uri, Dictionary<string, string> queryParams = null)
        {
            if (queryParams != null)
            {
                uri = SetQueryParams(uri, queryParams);
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }

        private string SetQueryParams(string uri, Dictionary<string, string> queryParams)
        {
            var builder = new UriBuilder(uri);

            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }

            builder.Query = query.ToString();

            return builder.ToString();
        }
    }
}
