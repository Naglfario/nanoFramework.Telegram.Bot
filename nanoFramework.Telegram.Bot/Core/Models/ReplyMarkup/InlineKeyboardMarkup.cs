namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    /// <summary>
    /// This object represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    public class InlineKeyboardMarkup : IReplyMarkup
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of InlineKeyboardButton objects
        /// </summary>
        public InlineKeyboardButton[][] inline_keyboard { get; set; }
    }
}
