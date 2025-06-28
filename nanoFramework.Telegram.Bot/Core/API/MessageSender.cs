using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Models.Problem;
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
        private readonly ISettingsProvider _settings;

        public MessageSender(
            TelegramBotEvents events,
            IURLProvider urlProvider,
            IHttpClientProvider httpClient,
            ISettingsProvider settings)
        {
            _events = events;
            _urlProvider = urlProvider;
            _httpClient = httpClient;
            _settings = settings;
        }

        public SendResult Send(SendTelegramMessageCommand command)
        {
            try
            {
                var url = _urlProvider.SendMessage(command);
                using var response = _httpClient.Get(url);
                return HandleProblems(response);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails(ex);
                if(_settings.UseEventsForSendFailures) _events.RaiseError(problemDetails);
                return new SendResult(problemDetails);
            }
        }

        internal SendResult HandleProblems(HttpResponseMessage response)
        {
            var problemDetails = response.GetProblemDetails();
            if (problemDetails != null)
            {
                if (_settings.UseEventsForSendFailures) _events.RaiseError(problemDetails);

                return new SendResult(problemDetails);
            }

            return new SendResult();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}