using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignalRMVCSolution.Models
{
    public class IEXService
    {

        public HttpClient Client { get; }

        //public string HttpRequest(string requestString)
        //{

        //    return null;
        //}


        public IEXService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://cloud.iexapis.com/");
            // GitHub API versioning
            //client.DefaultRequestHeaders.Add("Accept",
             //   "application/json");
            // GitHub requires a user-agent
            //client.DefaultRequestHeaders.Add("User-Agent",
             //   "HttpClientFactory-Sample");

            Client = client;
        }//https://cloud.iexapis.com/beta/stock/aapl/quote?token=pk_1047da25bc614c2796a0e43f19688724

        public async Task<string> HttpRequest()
        {
            var response = await Client.GetAsync(
                "/beta/stock/aapl/quote?token=pk_1047da25bc614c2796a0e43f19688724");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
