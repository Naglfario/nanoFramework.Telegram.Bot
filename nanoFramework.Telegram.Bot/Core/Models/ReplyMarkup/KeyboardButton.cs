namespace nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup
{
    /// <summary>
    /// This object represents one button of the reply keyboard.
    /// At most one of the optional fields must be used to specify type of the button.
    /// For simple text buttons, String can be used instead of this object to specify the button text.
    /// </summary>
    public class KeyboardButton
    {
        /// <summary>
        /// Text of the button. If none of the optional fields are used,
        /// it will be sentas a message when the button is pressed
        /// </summary>
        public string text { get; set; } = "Can't be empty!";
    }
}
