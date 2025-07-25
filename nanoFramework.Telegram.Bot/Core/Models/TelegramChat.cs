﻿namespace nanoFramework.Telegram.Bot.Core.Models
{
    public class TelegramChat
    {
        public long id { get; set; }
        public string type { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string title { get; set; }
    }
}
