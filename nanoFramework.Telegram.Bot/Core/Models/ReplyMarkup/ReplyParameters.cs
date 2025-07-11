﻿namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    public class ReplyParameters
    {
        /// <summary>
        /// Identifier of the message that will be replied to in the current chat,
        /// or in the chat chat_id if it is specified
        /// </summary>
        public long message_id { get; set; }

        /// <summary>
        /// Optional. If the message to be replied to is from a different chat,
        /// unique identifier for the chat. Not supported for messages
        /// sent on behalf of a business account.
        /// </summary>
        public long chat_id { get; set; }

        /// <summary>
        /// Optional. Pass True if the message should be sent even if the specified message
        /// to be replied to is not found. Always False for replies in another chat or
        /// forum topic. Always True for messages sent on behalf of a business account.
        /// </summary>
        public bool allow_sending_without_reply { get; set; } = false;

        /// <summary>
        /// Optional. Quoted part of the message to be replied to;
        /// 0-1024 characters after entities parsing. The quote must be an exact substring
        /// of the message to be replied to, including bold, italic, underline,
        /// strikethrough, spoiler, and custom_emoji entities. The message will fail
        /// to send if the quote isn't found in the original message.
        /// </summary>
        public string quote { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Mode for parsing entities in the quote.
        /// Possible values: Markdown, MarkdownV2, Html
        /// </summary>
        public string quote_parse_mode { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Position of the quote in the original message in UTF-16 code units
        /// </summary>
        public int quote_position { get; set; }
    }
}
