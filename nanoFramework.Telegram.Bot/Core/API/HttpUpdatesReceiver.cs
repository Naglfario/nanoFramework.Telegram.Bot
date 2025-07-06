using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models;
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
            StopPolling();
            _httpClient.Dispose();
            IsEnabled = false;
        }

        public void StartPolling()
        {
            _timer ??= new Timer(TimerCallback,
            this, 0, _settings.PollDelayMilliseconds);
            IsEnabled = true;
        }

        public void StopPolling()
        {
            _timer?.Dispose();
            _timer = null;
            IsEnabled = false;
        }

        public void UpdatePollDelay()
        {
            if (_timer == null) return;

            _timer.Change(_settings.PollDelayMilliseconds, _settings.PollDelayMilliseconds);
        }

        private static void TimerCallback(object state)
        {
            var receiver = (HttpUpdatesReceiver)state;
            var longRunningThread = new Thread(receiver.GetUpdatesPolling);
            longRunningThread.Start();
        }

        private void GetUpdatesPolling()
        {
            try
            {
                var getUpdatesResult = SendGetUpdatesRequest();

                if(getUpdatesResult.ProblemDetails != null)
                {
                    _events.RaiseError(getUpdatesResult.ProblemDetails);

                    return;
                }

                var telegramResponse = getUpdatesResult.RawUpdates;

                if (telegramResponse == null ||
                    telegramResponse.result == null ||
                    telegramResponse.result.Length == 0)
                    return;

                foreach (var update in telegramResponse.result)
                {
                    if (update.message != null)
                        _events.RaiseMessageReceived(update.message);
                    else if (update.callback_query != null)
                        _events.RaiseCallbackReceived(update.callback_query);
                }
            }
            catch (Exception ex)
            {
                _events.RaiseError(new(ex));
            }
        }

        internal GetUpdatesResult SendGetUpdatesRequest()
        {
            if (!_settings.TrackMessages && !_settings.TrackCallbackQuery)
            {
                return new GetUpdatesResult(new ProblemDetails(ErrorType.NothingToReceive));
            }

            try
            {
                var url = _urlProvider.GetUpdates(_lastSeenUpdateId);
                using var response = _httpClient.Get(url);

                var telegramResponse = (TelegramUpdateResponse)JsonConvert.DeserializeObject(
                       response.Content.ReadAsString(), typeof(TelegramUpdateResponse));

                var problemDetails = telegramResponse.GetProblemDetails();

                if(problemDetails != null) return new GetUpdatesResult(problemDetails);

                if (telegramResponse.result != null && telegramResponse.result.Length > 0)
                {
                    _lastSeenUpdateId = telegramResponse.result[telegramResponse.result.Length - 1].update_id;
                    if(_settings.AnswerCallbackQuery)
                    {
                        foreach(var update in telegramResponse.result)
                        {
                            if (update.callback_query != null)
                            {
                                SendAnswerCallbackQuery(update.callback_query.id);
                            }
                        }
                    }
                }

                return new GetUpdatesResult(telegramResponse);
            }
            catch(Exception ex)
            {
                return new GetUpdatesResult(new ProblemDetails(ex));
            }
        }

        private void SendAnswerCallbackQuery(string callbackId)
        {
            var url = _urlProvider.AnswerCallbackQuery(callbackId);
            using var response = _httpClient.Get(url);
        }

        internal GetUpdatesResult SendGetUpdatesRequest()
        {
            if (!_settings.TrackMessages && !_settings.TrackCallbackQuery)
            {
                return new GetUpdatesResult(new ProblemDetails(ErrorType.NothingToReceive));
            }

            try
            {
                var url = _urlProvider.GetUpdates(_lastSeenUpdateId);
                using var response = _httpClient.Get(url);

                var telegramResponse = (TelegramUpdateResponse)JsonConvert.DeserializeObject(
                       response.Content.ReadAsString(), typeof(TelegramUpdateResponse));

                var problemDetails = telegramResponse.GetProblemDetails();

                if(problemDetails != null) return new GetUpdatesResult(problemDetails);

                if (telegramResponse.result != null && telegramResponse.result.Length > 0)
                {
                    _lastSeenUpdateId = telegramResponse.result[telegramResponse.result.Length - 1].update_id;
                }

                return new GetUpdatesResult(telegramResponse);
            }
            catch(Exception ex)
            {
                return new GetUpdatesResult(new ProblemDetails(ex));
            }
        }
    }
}