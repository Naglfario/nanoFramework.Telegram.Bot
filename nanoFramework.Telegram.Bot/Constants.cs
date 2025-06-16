namespace nanoFramework.Telegram.Bot
{
    internal class Constants
    {
        public const string TelegramBaseUrl = "https://api.telegram.org/bot";
        public const string GetUpdatesRoute = "/getUpdates?allowed_updates=[%22message%22]";
        public const string OffsetParam = "&offset=";
        public const string SendMessage = "/sendMessage?";
    }
}
