using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient;
        public HttpClientProvider() => _httpClient = new HttpClient();
        public void Dispose() => _httpClient.Dispose();
        public HttpResponseMessage Get(string url) => _httpClient.Get(url);
    }
}
