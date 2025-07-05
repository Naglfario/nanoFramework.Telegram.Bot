namespace nanoFramework.Telegram.Bot.Core.Models.Update
{
    public class TelegramUpdate
    {
        public long update_id { get; set; }
        public TelegramMessage message { get; set; } = null;
        public CallbackQuery callback_query { get; set; } = null;
    }
}
