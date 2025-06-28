using nanoFramework.Telegram.Bot.Core.Providers;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Tests.Fakes
{
    public class FakeHttpClientProvider : IHttpClientProvider
    {
        private readonly HttpResponseMessage _response;
        public FakeHttpClientProvider(HttpResponseMessage response)
        {
            _response = response;
        }

        public void Dispose() { }
        public HttpResponseMessage Get(string url) => _response;
    }
}
