namespace nanoFramework.Telegram.Bot.Core.Models
{
    public abstract class TelegramResponseBase : ITelegramResponse
    {
        public bool ok { get; set; }
        public int error_code { get; set; }
        public string description { get; set; }
    }
}
