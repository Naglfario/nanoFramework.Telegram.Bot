using nanoFramework.Telegram.Bot.Core.Models.Problem;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly TelegramBotEvents _events;

        internal SettingsProvider(string token, TelegramBotEvents events)
        {
            Token = token;
            _events = events;
        }

        /// <inheritdoc/>
        public string Token { get; }

        /// <inheritdoc/>
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
