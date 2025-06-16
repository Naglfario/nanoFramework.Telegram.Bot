using nanoFramework.Telegram.Bot.Core.Models.Problem;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBotSettings
    {
        private readonly TelegramBotEvents _events;

        internal TelegramBotSettings(string token, TelegramBotEvents events)
        {
            Token = token;
            _events = events;
        }

        /// <summary>
        /// Telegram bot API token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Delay between calls to Telegram getUpdates endpoint
        /// </summary>
        public int PollDelayMilliseconds { get; private set; } = 500;

        internal void SetPollDelay(int delay)
        {
            if(delay < 0)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.IncorrectPollDelay));
                return;
            }

            PollDelayMilliseconds = delay;
        }
    }
}
