using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal class HttpClientProvider : IHttpClientProvider
    {
        private static readonly object _lockObject = new object();
        private readonly HttpClient _httpClient;
        public HttpClientProvider(HttpClient client) => _httpClient = client;
        public void Dispose() => _httpClient.Dispose();
        public HttpResponseMessage Get(string url)
        {
            lock(_lockObject)
            {
                return _httpClient.Get(url);
            }
        }
    }
}
