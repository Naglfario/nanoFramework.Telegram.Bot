namespace nanoFramework.Telegram.Bot.Core.Models.Update
{
    internal class TelegramUpdateResponse : TelegramResponseBase
    {
        public TelegramUpdate[] result { get; set; }
    }
}
