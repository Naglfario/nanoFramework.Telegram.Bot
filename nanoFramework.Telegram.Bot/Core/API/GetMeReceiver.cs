using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Providers;
using System;

namespace nanoFramework.Telegram.Bot.Core.API
{
    internal class GetMeReceiver
    {
        private readonly IURLProvider _urlProvider;
        private readonly IHttpClientProvider _httpClient;

        public GetMeReceiver(
            IURLProvider urlProvider,
            IHttpClientProvider httpClient)
        {
            _urlProvider = urlProvider;
            _httpClient = httpClient;
        }

        public GetMeResponse GetMe()
        {
            try
            {
                var url = _urlProvider.GetMe();
                using var response = _httpClient.Get(url);
                if (response == null) return null;
                else if (!response.IsSuccessStatusCode) return null;

                var deserializedResponseObject = JsonConvert.DeserializeObject(
                    response.Content.ReadAsStream(), typeof(GetMeResponse));

                return deserializedResponseObject is GetMeResponse deserializedResponse
                    ? deserializedResponse
                    : null;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
