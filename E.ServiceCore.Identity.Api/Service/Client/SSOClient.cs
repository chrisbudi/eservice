using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace E.ServiceCore.Identity.Api.Service.Client
{
    public class SSOClient
    {
        public HttpClient Client { get; private set; }

        public SSOClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://e-procurement.gagas.co.id/eproc-sso/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            Client = httpClient;
        }

    }
}
