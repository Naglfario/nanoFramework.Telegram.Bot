using nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup;

namespace nanoFramework.Telegram.Bot.Core.Models.Commands
{
    public class SendTelegramMessageCommand
    {
        /// <summary>
        /// Unique identifier for the target chat
        /// </summary>
        public long chat_id { get; set; }

        /// <summary>
        /// Text of the message to be sent, 1-4096 characters after entities parsing
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Mode for parsing entities in the message text.
        /// Possible values: Markdown, MarkdownV2, Html
        /// </summary>
        public string parse_mode { get; set; } = null;

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        public bool disable_notification { get; set; } = false;

        /// <summary>
        /// Protects the contents of the sent message from forwarding and saving
        /// </summary>
        public bool protect_content { get; set; } = false;

        /// <summary>
        /// Description of the message to reply to
        /// </summary>
        public ReplyParameters reply_parameters { get; set; } = null;

        /// <summary>
        /// Additional interface options.
        /// Can be <see cref="ForceReply"/> or <see cref="ReplyKeyboardMarkup"/> or null.
        /// </summary>
        public IReplyMarkup reply_markup { get; set; } = null;
    }
}
