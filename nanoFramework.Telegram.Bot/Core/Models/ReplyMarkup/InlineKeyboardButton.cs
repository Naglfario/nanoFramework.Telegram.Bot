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
        /// Links tg://user?id=<user_id> can be used to mention a user by their 
        /// identifier without using a username, if this is allowed by their privacy settings.
        /// </summary>
        public string url { get; set; } = null;

        /// <summary>
        /// Optional. Data to be sent in a callback query to the bot when 
        /// the button is pressed, 1-64 bytes
        /// </summary>
        public string callback_data { get; set; } = null;

        /// <summary>
        /// Optional. If set, pressing the button will prompt the user to select 
        /// one of their chats, open that chat and insert the bot's username and 
        /// the specified inline query in the input field. May be empty, 
        /// in which case just the bot's username will be inserted. 
        /// Not supported for messages sent on behalf of a Telegram Business account.
        /// </summary>
        public string switch_inline_query { get; set; } = null;

        /// <summary>
        /// Optional. If set, pressing the button will insert the bot's username 
        /// and the specified inline query in the current chat's input field. 
        /// May be empty, in which case only the bot's username will be inserted.
        /// This offers a quick way for the user to open your bot in inline mode 
        /// in the same chat - good for selecting something from multiple options. 
        /// Not supported in channels and for messages sent on behalf of a Business account.
        /// </summary>
        public string switch_inline_query_current_chat { get; set; } = null;

        /// <summary>
        /// Optional. Description of the button that copies the specified text to the clipboard.
        /// </summary>
        public CopyTextButton copy_text { get; set; } = null;
    }
}
