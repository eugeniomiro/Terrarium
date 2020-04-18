using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Terrarium.Acceptance.Tests
{
    using Web.UI;

    internal class HttpClientBuilder
    {
        public HttpClientBuilder()
        {
            _baseAddress    = new Uri("http://localhost:12345");
            _config         = new HttpSelfHostConfiguration(_baseAddress);
            _server         = new HttpSelfHostServer(_config);
            _buildStep      = c => { };
        }

        internal HttpClientBuilder WithBuildStep(Action<HttpConfiguration> buildStep)
        {
            _buildStep = buildStep;
            return this;
        }

        internal HttpClient Build()
        {
            _buildStep(_config);
            WebApiConfig.Register(_config);
            var client = new HttpClient(_server);
            try {
                client.BaseAddress = _baseAddress;
                return client;
            } catch {
                _server.Dispose();
                client.Dispose();
                throw;
            } finally {
                _config.Dispose();
            }
        }

        private Uri                         _baseAddress;
        private HttpSelfHostConfiguration   _config;
        private HttpSelfHostServer          _server;
        private Action<HttpConfiguration>   _buildStep;
    }
}
