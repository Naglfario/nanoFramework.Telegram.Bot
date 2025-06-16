namespace nanoFramework.Telegram.Bot.Core.Models
{
    internal interface ITelegramResponse
    {
        public bool ok { get; set; }

        public int error_code { get; set; }
        public string description { get; set; }
    }
}
