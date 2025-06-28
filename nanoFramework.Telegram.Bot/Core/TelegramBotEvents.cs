using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Problem;
using nanoFramework.Telegram.Bot.Core.Models.Update;

namespace nanoFramework.Telegram.Bot.Core
{
    public class TelegramBotEvents
    {
        public delegate void ErrorDelegate(ProblemDetails problem);
        public delegate void MessageDelegate(TelegramMessage message);
        public delegate void CallbackQueryDelegate(CallbackQuery callback);

        public event ErrorDelegate OnError;
        public event MessageDelegate OnMessageReceived;
        public event CallbackQueryDelegate OnCallbackQuery;

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

        internal void RaiseCallbackReceived(CallbackQuery callback)
        {
            if (callback == null) return;

            OnCallbackQuery?.Invoke(callback);
        }
    }
}
