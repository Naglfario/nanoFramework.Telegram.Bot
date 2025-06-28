using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Providers;

namespace nanoFramework.Telegram.Bot.Tests.Fakes
{
    public class FakeURLProvider : IURLProvider
    {
        private readonly string _getMe;
        private readonly string _getUpdates;
        private readonly string _sendMessages;

        public FakeURLProvider(
            string getMe = "",
            string getUpdates = "",
            string sendMessage = "")
        {
            _getMe = getMe;
            _getUpdates = getUpdates;
            _sendMessages = sendMessage;
        }

        public string GetMe() => _getMe;
        public string GetUpdates(long lastSeenUpdateId) => _getUpdates;
        public string SendMessage(SendTelegramMessageCommand command) => _sendMessages;
    }
}
