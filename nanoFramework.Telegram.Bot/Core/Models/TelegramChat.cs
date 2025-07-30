namespace nanoFramework.Telegram.Bot.Core.Models
{
    public class TelegramChat
    {
        public long id { get; set; }
        public string type { get; set; } = string.Empty;
        public bool is_bot { get; set; }
        //public string first_name { get; set; } = string.Empty;
        //public string last_name { get; set; } = string.Empty;
        //public string username { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
    }
}
