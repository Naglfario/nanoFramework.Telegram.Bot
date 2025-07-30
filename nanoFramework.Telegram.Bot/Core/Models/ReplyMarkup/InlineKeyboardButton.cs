namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    /// <summary>
    /// This object represents one button of an inline keyboard. 
    /// Exactly one of the optional fields must be used to specify type of the button.
    /// </summary>
    public class InlineKeyboardButton
    {
        /// <summary>
        /// Label text on the button
        /// </summary>
        public string text { get; set; } = "Can't be empty!";

        /// <summary>
        /// Optional. HTTP or tg:// URL to be opened when the button is pressed. 
        /// Links tg://user?id=user_id can be used to mention a user by their 
        /// identifier without using a username, if this is allowed by their privacy settings.
        /// </summary>
        public string url { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Data to be sent in a callback query to the bot when 
        /// the button is pressed, 1-64 bytes
        /// </summary>
        public string callback_data { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Description of the button that copies the specified text to the clipboard.
        /// </summary>
        public CopyTextButton copy_text { get; set; } = null;
    }
}
