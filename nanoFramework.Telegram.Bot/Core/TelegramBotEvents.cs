using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Problem;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBotEvents
    {
        public delegate void ErrorDelegate(ProblemDetails problem);
        public delegate void MessageDelegate(TelegramMessage message);

        public event ErrorDelegate OnError;
        public event MessageDelegate OnMessageReceived;

        internal bool IsErrorsTracked => OnError != null;

        internal void RaiseError(ProblemDetails problem)
        {
            if (problem == null) return;

            OnError?.Invoke(problem);
        }

        internal void RaiseMessageReceived(TelegramMessage message)
        {
            if(message == null) return;

            OnMessageReceived?.Invoke(message);
        }
    }
}
