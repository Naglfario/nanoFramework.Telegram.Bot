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
    }
}
