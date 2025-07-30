using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Extensions;
using System;

namespace nanoFramework.Telegram.Bot.Core.API
{
    internal class GetMeReceiver
    {
        private readonly IURLProvider _urlProvider;
        private readonly IHttpClientProvider _httpClient;
        private readonly ISettingsProvider _settings;

        public GetMeReceiver(
            IURLProvider urlProvider,
            IHttpClientProvider httpClient,
            ISettingsProvider settings)
        {
            _urlProvider = urlProvider;
            _httpClient = httpClient;
            _settings = settings;
        }

        public GetMeResponse GetMe()
        {
            try
            {
                var url = _urlProvider.GetMe();
                using var response = _httpClient.Get(url);
                if (response == null) return null;
                var textResponse = response.Content.ReadAsString();
                if (_settings.DecodeUnicode) textResponse = textResponse.DecodeUnicode();

                var deserializedResponseObject = JsonConvert.DeserializeObject(
                    textResponse, typeof(GetMeResponse));

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
