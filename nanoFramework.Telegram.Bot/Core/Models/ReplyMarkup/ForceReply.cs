namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will display
    /// a reply interface to the user (act as if the user has selected the bot's message
    /// and tapped 'Reply'). This can be extremely useful if you want to create
    /// user-friendly step-by-step interfaces without having to sacrifice privacy mode.
    /// Not supported in channels and for messages sent on behalf of a Telegram Business account.
    /// </summary>
    public class ForceReply : IReplyMarkup
    {
        /// <summary>
        /// Shows reply interface to the user, as if they manually selected
        /// the bot's message and tapped 'Reply'
        /// </summary>
        public bool force_reply { get; set; }

        /// <summary>
        /// Optional. The placeholder to be shown in the input field when
        /// the reply is active; 1-64 characters
        /// </summary>
        public string input_field_placeholder { get; set; } = null;

        /// <summary>
        /// Optional. Use this parameter if you want to force reply from specific users only.
        /// Targets: 1) users that are @mentioned in the text of the Message object;
        /// 2) if the bot's message is a reply to a message in the same chat and forum topic,
        /// sender of the original message.
        /// </summary>
        public bool selective { get; set; }
    }
}
