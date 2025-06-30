namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    /// <summary>
    /// This object represents a custom keyboard with reply options
    /// (see Introduction to bots for details and examples).
    /// Not supported in channels and for messages sent on behalf of a Telegram Business account.
    /// </summary>
    public class ReplyKeyboardMarkup : IReplyMarkup
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of KeyboardButton objects
        /// </summary>
        public KeyboardButton[][] keyboard { get; set; }

        /// <summary>
        /// Optional. Requests clients to always show the keyboard when the regular keyboard
        /// is hidden. Defaults to false, in which case the custom keyboard can be
        /// hidden and opened with a keyboard icon.
        /// </summary>
        public bool is_persistent { get; set; } = false;

        /// <summary>
        /// Optional. Requests clients to resize the keyboard vertically for optimal fit
        /// (e.g., make the keyboard smaller if there are just two rows of buttons).
        /// Defaults to false, in which case the custom keyboard is always of the same height
        /// as the app's standard keyboard.
        /// </summary>
        public bool resize_keyboard { get; set; } = false;

        /// <summary>
        /// Optional. Requests clients to hide the keyboard as soon as it's been used.
        /// The keyboard will still be available, but clients will automatically display
        /// the usual letter-keyboard in the chat - the user can press a special button
        /// in the input field to see the custom keyboard again. Defaults to false.
        /// </summary>
        public bool one_time_keyboard { get; set; } = false;

        /// <summary>
        /// Optional. The placeholder to be shown in the input field when
        /// the keyboard is active; 1-64 characters
        /// </summary>
        public string input_field_placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Use this parameter if you want to force reply from specific users only.
        /// Targets: 1) users that are @mentioned in the text of the Message object;
        /// 2) if the bot's message is a reply to a message in the same chat and forum topic,
        /// sender of the original message.
        /// </summary>
        public bool selective { get; set; }
    }
}
