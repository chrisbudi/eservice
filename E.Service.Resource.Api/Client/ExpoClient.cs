using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Client
{
    public class ExpoClient
    {
        public HttpClient Client { get; private set; }

        public ExpoClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://expo.io/--/api/v2/push/send");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            Client = httpClient;
        }

    }
}
