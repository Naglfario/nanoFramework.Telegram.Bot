namespace nanoFramework.Telegram.Bot.Core.Models.Update
{
    public class CallbackQuery
    {
        public string id { get; set; }
        public TelegramUser from { get; set; }
        public TelegramMessage message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
    }
}
