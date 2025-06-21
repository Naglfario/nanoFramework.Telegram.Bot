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
        /// Receive messages from Telegram that users send to the bot
        /// </summary>
        public bool TrackMessages { get; }

        /// <summary>
        /// Receive callback data from Telegram (for example, inline keyboard events)
        /// </summary>
        public bool TrackCallbackQuery { get; }
    }
}
