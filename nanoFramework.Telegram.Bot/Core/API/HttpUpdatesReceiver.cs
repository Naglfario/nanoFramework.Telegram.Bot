using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models.Problem;
using nanoFramework.Telegram.Bot.Core.Models.Update;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Extensions;
using System;
using System.Threading;

namespace nanoFramework.Telegram.Bot.Core.Updates
{
    internal class HttpUpdatesReceiver : IDisposable
    {
        private readonly TelegramBotEvents _events;
        private readonly ISettingsProvider _settings;
        private readonly IURLProvider _urlProvider;
        private readonly IHttpClientProvider _httpClient;
        private long _lastSeenUpdateId = 0;
        private Timer _timer;

        public bool IsEnabled { get; private set; } = false;

        public HttpUpdatesReceiver(
            TelegramBotEvents events,
            ISettingsProvider settings,
            IURLProvider urlProvider,
            IHttpClientProvider httpClient)
        {
            _events = events;
            _settings = settings;
            _urlProvider = urlProvider;
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            Stop();
            _httpClient.Dispose();
            IsEnabled = false;
        }

        public void Start()
        {
            _timer ??= new Timer(TimerCallback,
            this, 0, _settings.PollDelayMilliseconds);
            IsEnabled = true;
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
            IsEnabled = false;
        }

        public void UpdateDelay()
        {
            if (_timer == null) return;

            _timer.Change(0, _settings.PollDelayMilliseconds);
        }

        private static void TimerCallback(object state)
        {
            var myDevice = (HttpUpdatesReceiver)state;
            var longRunningThread = new Thread(myDevice.GetUpdates);
            longRunningThread.Start();
        }

        private void GetUpdates()
        {
            try
            {
                if (!_settings.TrackMessages && !_settings.TrackCallbackQuery) return;

                var url = _urlProvider.GetUpdates(_lastSeenUpdateId);
                using var response = _httpClient.Get(url);

                if (!response.IsSuccessStatusCode)
                {
                    _events.RaiseError(response.GetProblemDetails());

                    return;
                }

                var telegramResponse = (TelegramUpdateResponse)JsonConvert.DeserializeObject(
                        response.Content.ReadAsStream(), typeof(TelegramUpdateResponse));

                var problemDetails = telegramResponse.GetProblemDetails();

                if (problemDetails != null)
                {
                    _events.RaiseError(problemDetails);

                    return;
                }

                if (telegramResponse.result == null || telegramResponse.result.Length == 0) return;

                _lastSeenUpdateId = telegramResponse.result[telegramResponse.result.Length - 1].update_id;

                foreach (var update in telegramResponse.result)
                {
                    if (update.message != null)
                        _events.RaiseMessageReceived(update.message);
                    else if(update.callback_query != null)
                        _events.RaiseCallbackReceived(update.callback_query);
                }
            }
            catch (Exception ex)
            {
                _events.RaiseError(new ProblemDetails(ex));
            }
        }
    }
}