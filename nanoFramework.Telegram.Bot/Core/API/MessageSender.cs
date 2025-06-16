using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Extensions;
using System;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core.API
{
    internal class MessageSender : IDisposable
    {
        private readonly TelegramBotEvents _events;
        private readonly IURLProvider _urlProvider;
        private readonly IHttpClientProvider _httpClient;

        public MessageSender(
            TelegramBotEvents events,
            IURLProvider urlProvider,
            IHttpClientProvider httpClient)
        {
            _events = events;
            _urlProvider = urlProvider;
            _httpClient = httpClient;
        }

        public void Send(SendTelegramMessageCommand command)
        {
            try
            {
                var url = _urlProvider.SendMessage(command);
                using var response = _httpClient.Get(url);
                HandleProblems(response);
            }
            catch (Exception ex)
            {
                _events.RaiseError(new(ex));
            }
        }

        internal void HandleProblems(HttpResponseMessage response)
        {
            var problemDetails = response.GetProblemDetails();
            if (problemDetails != null)
            {
                _events.RaiseError(problemDetails);

                return;
            }

            if (_events.IsErrorsTracked)
            {
                var telegramResponse = (TelegramSendMessageResponse)JsonConvert.DeserializeObject(
                    response.Content.ReadAsStream(), typeof(TelegramSendMessageResponse));

                _events.RaiseError(telegramResponse.GetProblemDetails());
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}