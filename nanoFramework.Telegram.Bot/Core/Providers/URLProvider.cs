using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using System.Text;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal class URLProvider : IURLProvider
    {
        private readonly ISettingsProvider _settings;

        internal const string TelegramBaseUrl = "https://api.telegram.org/bot";

        internal const string GetUpdatesRoute = "/getUpdates?";
        internal const string AllowUpdatesStart = "allowed_updates=[";
        internal const string TrackMessages = "%22message%22";
        internal const string TrackCallbackQuery = "%22callback_query%22";
        internal const string OffsetParam = "offset=";
        internal const string LimitParam = "&limit=";

        internal const string GetMeRoute = "/getMe";

        internal const string AnswerCallbackRoute = "/answerCallbackQuery?";
        internal const string CallbackQueryIdParam = "callback_query_id=";

        internal const string SendMessageRoute = "/sendMessage?";
        internal const string ChatIdParam = "chat_id=";
        internal const string TextParam = "&text=";
        internal const string DisableNotificationParam = "&disable_notification=";
        internal const string ProtectContentParam = "&protect_content=";
        internal const string ParseModeParam = "&parse_mode=";
        internal const string ReplyParametersParam = "&reply_parameters=";
        internal const string ReplyMarkupParam = "&reply_markup=";

        public URLProvider(ISettingsProvider settings)
        {
            _settings = settings;
        }

        public string GetUpdates(long lastUpdateId)
        {
            var sb = new StringBuilder(TelegramBaseUrl);
            sb.Append(_settings.Token);
            sb.Append(GetUpdatesRoute);
            if(_settings.TrackMessages || _settings.TrackCallbackQuery)
            {
                sb.Append(AllowUpdatesStart);
                if(_settings.TrackMessages && _settings.TrackCallbackQuery)
                {
                    sb.Append(TrackMessages);
                    sb.Append(',');
                    sb.Append(TrackCallbackQuery);
                }
                else if(_settings.TrackMessages) sb.Append(TrackMessages);
                else if(_settings.TrackCallbackQuery) sb.Append(TrackCallbackQuery);
                sb.Append(']');
                sb.Append('&');
            }
            if (lastUpdateId > 0)
            {
                sb.Append(OffsetParam);
                var offset = lastUpdateId + 1;
                sb.Append(offset);
            }

            sb.Append(LimitParam);
            sb.Append(_settings.UpdatesLimitPerRequest);

            return sb.ToString();
        }
        public string SendMessage(SendTelegramMessageCommand command)
        {
            var sb = new StringBuilder(TelegramBaseUrl);
            sb.Append(_settings.Token);
            sb.Append(SendMessageRoute);
            sb.Append(ChatIdParam); sb.Append(command.chat_id);
            sb.Append(TextParam); sb.Append(command.text);
            sb.Append(DisableNotificationParam); sb.Append(command.disable_notification);
            sb.Append(ProtectContentParam); sb.Append(command.protect_content);

            if (!string.IsNullOrEmpty(command.parse_mode))
            {
                sb.Append(ParseModeParam); sb.Append(command.parse_mode);
            }

            if (command.reply_parameters != null)
            {
                var replyParametersJson = JsonSerializer.SerializeObject(command.reply_parameters, false);
                sb.Append(ReplyParametersParam); sb.Append(replyParametersJson);
            }

            if (command.reply_markup != null)
            {
                var replyMarkupJson = JsonSerializer.SerializeObject(command.reply_markup, false);
                sb.Append(ReplyMarkupParam); sb.Append(replyMarkupJson);
            }

            return sb.ToString();
        }

        public string GetMe()
        {
            var sb = new StringBuilder(TelegramBaseUrl);

            sb.Append(_settings.Token);
            sb.Append(GetMeRoute);

            return sb.ToString();
        }

        public string AnswerCallbackQuery(string callbackId)
        {
            var sb = new StringBuilder(TelegramBaseUrl);

            sb.Append(_settings.Token);
            sb.Append(AnswerCallbackRoute);
            sb.Append(CallbackQueryIdParam);
            sb.Append(callbackId);

            return sb.ToString();
        }
    }
}
