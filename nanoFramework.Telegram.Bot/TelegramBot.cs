using nanoFramework.Telegram.Bot.Core.API;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Core.Updates;
using System;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBot : IDisposable
    {
        public readonly TelegramBotEvents Events = new();

        private HttpUpdatesReceiver _updatesReceiver;
        private MessageSender _sender;
        private GetMeReceiver _getMeReceiver;

        private readonly SettingsProvider _settings;
        private readonly URLProvider _urlProvider;
        private readonly HttpClientProvider _httpClient;

        public TelegramBot(string token, HttpClient client)
        {
            _settings = new SettingsProvider(token, Events);
            _urlProvider = new URLProvider(_settings);
            _httpClient = new HttpClientProvider(client);
        }

        /// <summary>
        /// Executes /getMe endpoint and returns a basic response, 
        /// without information about the bot, you already know it, don't you?
        /// So, this method needed only to check connection.
        /// </summary>
        public GetMeResponse CheckConnection()
        {
            if(_getMeReceiver == null)
            {
                _getMeReceiver = new GetMeReceiver(_urlProvider, _httpClient);
            }

            return _getMeReceiver.GetMe();
        }
        
        /// <summary>
        /// Send single "getUpdates" request to Telegram API
        /// </summary>
        public GetUpdatesResult GetUpdates()
        {
            if (_updatesReceiver == null)
            {
                _updatesReceiver = new HttpUpdatesReceiver(
                    Events, _settings, _urlProvider, _httpClient);
            }

            return _updatesReceiver.SendGetUpdatesRequest();
        }

        /// <summary>
        /// Start receiving messages
        /// </summary>
        public void StartReceiving(int pollDelayMs = 5000)
        {
            if (_updatesReceiver != null && _updatesReceiver.IsEnabled) return;

            _settings.SetPollDelay(pollDelayMs);

            if (_updatesReceiver == null)
            {
                _updatesReceiver = new HttpUpdatesReceiver(
                    Events, _settings, _urlProvider, _httpClient);
            }

            _updatesReceiver.StartPolling();
        }

        /// <summary>
        /// Stop receiving messages
        /// </summary>
        public void StopReceiving()
        {
            if (_updatesReceiver == null || !_updatesReceiver.IsEnabled) return;

            _updatesReceiver.StopPolling();
        }

        /// <summary>
        /// Change poll delay (default 1000 ms)
        /// </summary>
        public void UpdatePollDelay(int newValue)
        {
            _settings.SetPollDelay(newValue);
            _updatesReceiver.UpdatePollDelay();
        }

        /// <summary>
        /// Update limit on number of updates that program will request per one time
        /// (default 1)
        /// </summary>
        public void UpdateLimit(int newValue)
            => _settings.SetUpdatesLimit(newValue);

        /// <summary>
        /// Track messages that users send to the bot?
        /// It will works after call <see cref="StartReceiving"/> method
        /// </summary>
        public void ToggleMessageReceiving(bool newState)
            => _settings.SetTrackMessagesValue(newState);

        /// <summary>
        /// Track callback query updates(for example, inline keyboarad events)?
        /// It will works after call <see cref="StartReceiving"/> method
        /// </summary>
        public void ToggleCallbackDataUpdatesReceiving(bool newState)
            => _settings.SetTrackCallbackQueryValue(newState);

        /// <summary>
        /// Notify about message sending errors via events?
        /// Default: false
        /// </summary>
        public void ToggleUseEventsForSendFailures(bool newState)
            => _settings.SetUseEventsForSendFailures(newState);

        /// <summary>
        /// Send message
        /// </summary>
        public SendResult Send(SendTelegramMessageCommand command)
        {
            _sender ??= new MessageSender(Events, _urlProvider, _httpClient, _settings);

            return _sender.Send(command);
        }

        public void Dispose()
        {
            _updatesReceiver?.Dispose();
            _sender?.Dispose();
            _httpClient?.Dispose();
        }
    }
}
