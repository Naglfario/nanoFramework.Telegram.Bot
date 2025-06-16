using System;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal interface IHttpClientProvider : IDisposable
    {
        public HttpResponseMessage Get(string url);
    }
}
