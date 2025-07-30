using nanoFramework.Telegram.Bot.Core.Providers;

namespace nanoFramework.Telegram.Bot.Tests.Fakes
{
    public class FakeSettingsProvider : ISettingsProvider
    {
        public string Token { get; set; } = "TOKEN";

        public int PollDelayMilliseconds { get; set; } = 500;

        public int UpdatesLimitPerRequest { get; set; } = 1;

        public bool TrackMessages { get; set; } = true;

        public bool TrackCallbackQuery { get; set; } = false;

        public bool UseEventsForSendFailures { get; set; } = false;

        public bool AnswerCallbackQuery { get; set; } = true;

        public bool DecodeUnicode { get; set; } = false;
    }
}
