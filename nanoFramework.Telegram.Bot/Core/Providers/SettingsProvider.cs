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

        /// <inheritdoc/>
        public bool TrackMessages { get; private set; } = true;

        /// <inheritdoc/>
        public bool TrackCallbackQuery { get; private set; } = false;

        internal void SetPollDelay(int delay)
        {
            if(delay < 0)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.IncorrectPollDelay));
                return;
            }

            PollDelayMilliseconds = delay;
        }

        internal void SetTrackMessagesValue(bool newState)
        {
            TrackMessages = newState;

            if(!TrackMessages && !TrackCallbackQuery)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.NothingToReceive));
            }
        }

        internal void SetTrackCallbackQueryValue(bool newState)
        {
            TrackCallbackQuery = newState;

            if (!TrackMessages && !TrackCallbackQuery)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.NothingToReceive));
            }
        }
    }
}
