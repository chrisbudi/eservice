using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Client
{
    public class EprocClient
    {
        public HttpClient Client { get; private set; }

        public EprocClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://178.128.96.19:4005/api/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            Client = httpClient;
        }

    }
}
