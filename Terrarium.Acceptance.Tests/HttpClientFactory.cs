﻿using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using TerrariumServer;

namespace Terrarium.Acceptance.Tests
{
    internal class HttpClientFactory
    {
        internal static HttpClient Create()
        {
            var baseAddress = new Uri("http://localhost:12345");
            var config = new HttpSelfHostConfiguration(baseAddress);

            WebApiConfig.Register(config);
            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server);
            try {
                client.BaseAddress = baseAddress;
                return client;
            } catch {
                server.Dispose();
                client.Dispose();
                throw;
            } finally {
                config.Dispose();
            }
        }

    }
}
