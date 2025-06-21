namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    /// <summary>
    /// This object represents an inline keyboard button that copies specified text to the clipboard.
    /// </summary>
    public class CopyTextButton
    {
        public CopyTextButton(string textToClipboard) => text = textToClipboard;

        /// <summary>
        /// The text to be copied to the clipboard; 1-256 characters
        /// </summary>
        public string text { get; set; } = "Can't be empty!";
    }
}
