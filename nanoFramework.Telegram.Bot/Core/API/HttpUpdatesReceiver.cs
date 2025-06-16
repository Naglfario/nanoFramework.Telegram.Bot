using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models.Problem;
using nanoFramework.Telegram.Bot.Core.Models.Update;
using nanoFramework.Telegram.Bot.Extensions;
using System;
using System.Net.Http;
using System.Threading;

namespace nanoFramework.Telegram.Bot.Core.Updates
{
    internal class HttpUpdatesReceiver : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramBotEvents _events;
        private readonly TelegramBotSettings _settings;
        private long _lastSeenUpdateId = 0;
        private Timer _timer;

        public bool IsEnabled { get; private set; } = false;

        public HttpUpdatesReceiver(TelegramBotEvents events, TelegramBotSettings settings)
        {
            _httpClient = new HttpClient();
            _events = events;
            _settings = settings;
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
                var url = Constants.TelegramBaseUrl
                    + _settings.Token
                    + Constants.GetUpdatesRoute;
                if (_lastSeenUpdateId > 0) url += $"{Constants.OffsetParam}{_lastSeenUpdateId}";
                var response = _httpClient.Get(url);

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
                    _events.RaiseMessageReceived(update.message);
                }
            }
            catch (Exception ex)
            {
                _events.RaiseError(new ProblemDetails(ex));
            }
        }
    }
}