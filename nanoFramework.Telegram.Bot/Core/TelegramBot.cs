using nanoFramework.Telegram.Bot.Core.API;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Updates;
using System;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBot : IDisposable
    {
        private HttpUpdatesReceiver _updatesReceiver;
        private MessageSender _sender;
        public TelegramBotEvents Events { get; }
        public TelegramBotSettings Settings { get; }

        public TelegramBot(string token)
        {
            Events = new TelegramBotEvents();
            Settings = new TelegramBotSettings(token, Events);
        }

        /// <summary>
        /// Start receiving messages
        /// </summary>
        /// <param name="pollDelayMs"></param>
        public void StartReceiving(TelegramBotEvents.MessageDelegate messageDelegate, int pollDelayMs = 500)
        {
            if (_updatesReceiver != null && _updatesReceiver.IsEnabled) return;

            Settings.SetPollDelay(pollDelayMs);
            Events.OnMessageReceived += messageDelegate;

            if (_updatesReceiver == null)
                _updatesReceiver = new HttpUpdatesReceiver(Events, Settings);
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
        /// Change <see cref="PollDelayMilliseconds"/>
        /// </summary>
        public void UpdatePollDelay(int newValue)
        {
            Settings.SetPollDelay(newValue);
            _updatesReceiver.UpdateDelay();
        }

        /// <summary>
        /// Send message
        /// </summary>
        public void Send(SendTelegramMessageCommand command)
        {
            _sender ??= new MessageSender(Events, Settings);

            _sender.Send(command);
        }

        public void Dispose()
        {
            _updatesReceiver?.Dispose();
            _sender?.Dispose();
        }
    }
}
