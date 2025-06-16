using nanoFramework.Telegram.Bot.Core.Models.Commands;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal interface IURLProvider
    {
        public string GetUpdates(long lastSeenUpdateId);
        public string SendMessage(SendTelegramMessageCommand command);
    }
}
