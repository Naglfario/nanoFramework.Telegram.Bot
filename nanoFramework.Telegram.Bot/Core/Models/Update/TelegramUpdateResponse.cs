namespace nanoFramework.Telegram.Bot.Core.Models.Update
{
    public class TelegramUpdateResponse : TelegramResponseBase
    {
        public TelegramUpdate[] result { get; set; }
    }
}
