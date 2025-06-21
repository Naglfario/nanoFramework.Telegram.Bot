using nanoFramework.Telegram.Bot.Core.API;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Core.Updates;
using System;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBot : IDisposable
    {
        private HttpUpdatesReceiver _updatesReceiver;
        private MessageSender _sender;
        private readonly TelegramBotEvents _events;
        private readonly SettingsProvider _settings;
        private readonly URLProvider _urlProvider;
        private readonly HttpClientProvider _httpClient;

        public TelegramBot(string token)
        {
            _events = new();
            _settings = new SettingsProvider(token, _events);
            _urlProvider = new URLProvider(_settings);
            _httpClient = new();
        }

        /// <summary>
        /// Start receiving messages
        /// </summary>
        public void StartReceiving(TelegramBotEvents.MessageDelegate messageDelegate, int pollDelayMs = 500)
        {
            if (_updatesReceiver != null && _updatesReceiver.IsEnabled) return;

            _settings.SetPollDelay(pollDelayMs);
            _events.OnMessageReceived += messageDelegate;

            if (_updatesReceiver == null)
                _updatesReceiver = new HttpUpdatesReceiver(
                    _events, _settings, _urlProvider, _httpClient);
            _updatesReceiver.Start();
        }

        /// <summary>
        /// Stop receiving messages
        /// </summary>
        public void StopReceiving()
        {
            if (_updatesReceiver == null || !_updatesReceiver.IsEnabled) return;

            _updatesReceiver.Stop();
        }

        /// <summary>
        /// Change poll delay
        /// </summary>
        public void UpdatePollDelay(int newValue)
        {
            _settings.SetPollDelay(newValue);
            _updatesReceiver.UpdateDelay();
        }

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
            => _settings.SetTrackMessagesValue(newState);

        /// <summary>
        /// Send message
        /// </summary>
        public void Send(SendTelegramMessageCommand command)
        {
            _sender ??= new MessageSender(_events, _urlProvider, _httpClient);

            _sender.Send(command);
        }

        public void Dispose()
        {
            _updatesReceiver?.Dispose();
            _sender?.Dispose();
        }
    }
}
