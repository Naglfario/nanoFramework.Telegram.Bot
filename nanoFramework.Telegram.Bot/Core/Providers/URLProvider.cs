using nanoFramework.Json;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using System.Text;

namespace nanoFramework.Telegram.Bot.Core.Providers
{
    internal class URLProvider : IURLProvider
    {
        private readonly ISettingsProvider _settings;

        internal const string TelegramBaseUrl = "https://api.telegram.org/bot";
        internal const string GetUpdatesRoute = "/getUpdates?allowed_updates=[%22message%22]";
        internal const string OffsetParam = "&offset=";
        internal const string SendMessageRoute = "/sendMessage?";

        public URLProvider(ISettingsProvider settings)
        {
            _settings = settings;
        }

        public string GetUpdates(long lastUpdateId)
        {
            var sb = new StringBuilder(TelegramBaseUrl);
            sb.Append(_settings.Token);
            sb.Append(GetUpdatesRoute);
            if (lastUpdateId > 0)
            {
                sb.Append(OffsetParam);
                sb.Append(lastUpdateId);
            }

            return sb.ToString();
        }
        public string SendMessage(SendTelegramMessageCommand command)
        {
            var sb = new StringBuilder(TelegramBaseUrl);
            sb.Append(_settings.Token);
            sb.Append(SendMessageRoute);
            sb.Append("chat_id="); sb.Append(command.chat_id);
            sb.Append("&text="); sb.Append(command.text);
            sb.Append("&disable_notification="); sb.Append(command.disable_notification);
            sb.Append("&protect_content="); sb.Append(command.protect_content);

            if (!string.IsNullOrEmpty(command.parse_mode))
            {
                sb.Append("&parse_mode="); sb.Append(command.parse_mode);
            }

            if (command.reply_parameters != null)
            {
                var replyParametersJson = JsonSerializer.SerializeObject(command.reply_parameters, false);
                sb.Append("&reply_parameters="); sb.Append(replyParametersJson);
            }

            if (command.reply_markup != null)
            {
                var replyMarkupJson = JsonSerializer.SerializeObject(command.reply_markup, false);
                sb.Append("&reply_markup="); sb.Append(replyMarkupJson);
            }

            return sb.ToString();
        }
    }
}
