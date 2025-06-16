using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Extensions;
using System;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core.API
{
    internal class MessageSender : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramBotEvents _events;
        private readonly TelegramBotSettings _settings;

        public MessageSender(TelegramBotEvents events, TelegramBotSettings settings)
        {
            _httpClient = new HttpClient();
            _events = events;
            _settings = settings;
        }

        public void Send(SendTelegramMessageCommand command)
        {
            try
            {
                var url = GetUrl(command);
                var response = _httpClient.Get(url);
                HandleProblems(response);
            }
            catch (Exception ex)
            {
                _events.RaiseError(new(ex));
            }
        }

        internal string GetUrl(SendTelegramMessageCommand command)
        {
            var url = $"{Constants.TelegramBaseUrl}{_settings.Token}{Constants.SendMessage}" +
                $"chat_id={command.chat_id}" +
                $"&text={command.text}" +
                $"&disable_notification={command.disable_notification}" +
                $"&protect_content={command.protect_content}";

            if (!string.IsNullOrEmpty(command.parse_mode))
            {
                url += $"&parse_mode={command.parse_mode}";
            }

            if (command.reply_parameters != null)
            {
                var replyParametersJson = JsonSerializer.SerializeObject(command.reply_parameters, false);
                url += $"&reply_parameters={replyParametersJson}";
            }

            if (command.reply_markup != null)
            {
                var replyMarkupJson = JsonSerializer.SerializeObject(command.reply_markup, false);
                url += $"&reply_markup={replyMarkupJson}";
            }

            return url;
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