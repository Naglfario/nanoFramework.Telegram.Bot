using System;

namespace nanoFramework.Telegram.Bot.Core.Models
{
    public class TelegramMessage
    {
        public ulong message_id { get; set; }
        public TelegramUser from { get; set; }
        public TelegramChat chat { get; set; }
        public long date { get; set; }
        public string text { get; set; }
        public DateTime GetDate() => DateTime.FromUnixTimeSeconds(date);
    }
}
