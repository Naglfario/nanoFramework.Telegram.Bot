using nanoFramework.Telegram.Bot.Core.Models.Problem;
using nanoFramework.Telegram.Bot.Core.Models.Update;

namespace nanoFramework.Telegram.Bot.Core.Models
{
    /// <summary>
    /// This class describes result of interaction with the Telegram API
    /// </summary>
    public class GetUpdatesResult
    {
        public GetUpdatesResult(TelegramUpdateResponse response)
        {
            RawUpdates = response;
            ProblemDetails = null;
        }

        public GetUpdatesResult(ProblemDetails problemDetails)
        {
            ProblemDetails = problemDetails;
            RawUpdates = null;
        }

        /// <summary>
        /// It can be null when updates request was not successful
        /// </summary>
        public TelegramUpdateResponse RawUpdates { get; set; } = null;

        /// <summary>
        /// Some requests will fail, so this class will explain why
        /// </summary>
        public ProblemDetails ProblemDetails { get; set; } = null;
    }
}
