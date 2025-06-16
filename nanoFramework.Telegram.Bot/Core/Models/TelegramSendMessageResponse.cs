namespace nanoFramework.Telegram.Bot.Core.Models
{
    internal class TelegramSendMessageResponse : ITelegramResponse
    {
        public bool ok { get; set; }
        public int error_code { get; set; }
        public string description { get; set; }
    }
}
