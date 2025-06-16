namespace nanoFramework.Telegram.Bot.Core.Models
{
    public class TelegramUser
    {
        public ulong id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
    }
}
