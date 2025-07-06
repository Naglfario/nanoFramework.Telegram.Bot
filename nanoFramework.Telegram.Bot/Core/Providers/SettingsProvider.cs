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
        public int PollDelayMilliseconds { get; private set; } = 5000;

        /// <inheritdoc/>
        public int UpdatesLimitPerRequest { get; private set; } = 1;

        /// <inheritdoc/>
        public bool TrackMessages { get; private set; } = true;

        /// <inheritdoc/>
        public bool TrackCallbackQuery { get; private set; } = false;

        /// <inheritdoc/>
        public bool UseEventsForSendFailures { get; private set; } = false;

        /// <inheritdoc/>
        public bool AnswerCallbackQuery { get; private set; } = true;

        internal void SetPollDelay(int delay)
        {
            if(delay < 0)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.IncorrectPollDelay));
                return;
            }

            PollDelayMilliseconds = delay;
        }

        internal void SetUpdatesLimit(int newValue)
        {
            if(newValue < 1 || newValue > 100)
            {
                _events.RaiseError(new ProblemDetails(ErrorType.IncorrectPollDelay));
                return;
            }

            UpdatesLimitPerRequest = newValue;
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

        internal void SetUseEventsForSendFailures(bool newState) => UseEventsForSendFailures = newState;

        internal void SetAnswerCallbackQuery(bool newState) => AnswerCallbackQuery = newState;
    }
}
