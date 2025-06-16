namespace nanoFramework.Telegram.Bot.Core.Models.Update
{
    internal class TelegramUpdateResponse : ITelegramResponse
    {
        public bool ok { get; set; }
        public TelegramUpdate[] result { get; set; }

        public int error_code { get; set; }
        public string description { get; set; }
    }
}
