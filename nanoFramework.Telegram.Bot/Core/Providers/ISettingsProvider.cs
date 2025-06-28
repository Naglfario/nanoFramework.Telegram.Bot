namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal interface ISettingsProvider
    {
        /// <summary>
        /// Telegram bot API token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Delay between calls to Telegram getUpdates endpoint
        /// </summary>
        public int PollDelayMilliseconds { get; }

        /// <summary>
        /// Maximum number of updates received per request to /getUpdates endpoint
        /// (in telegram API this paramter calls just "limit")
        /// </summary>
        public int UpdatesLimitPerRequest { get; }

        /// <summary>
        /// Receive messages from Telegram that users send to the bot
        /// </summary>
        public bool TrackMessages { get; }

        /// <summary>
        /// Receive callback data from Telegram (for example, inline keyboard events)
        /// </summary>
        public bool TrackCallbackQuery { get; }

        /// <summary>
        /// Notify about message sending errors via events
        /// </summary>
        public bool UseEventsForSendFailures { get; }
    }
}
