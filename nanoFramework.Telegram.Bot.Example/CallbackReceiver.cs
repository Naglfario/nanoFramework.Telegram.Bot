using nanoFramework.Telegram.Bot.Core;
using nanoFramework.Telegram.Bot.Core.Models.Update;

namespace nanoFramework.Telegram.Bot.Example
{
    internal class CallbackReceiver
    {
        private readonly TelegramBot _bot;
        private readonly long _adminId;

        public CallbackReceiver(TelegramBot bot, long adminId)
        {
            _bot = bot;
            _adminId = adminId;
        }

        public void Receive(CallbackQuery callback)
        {
            if(callback.from.id != _adminId) return;

            _bot.Send(new Core.Models.Commands.SendTelegramMessageCommand()
            {
                chat_id = callback.from.id,
                text = $"Received callback [{callback.data}]"
            });
        }
    }
}
