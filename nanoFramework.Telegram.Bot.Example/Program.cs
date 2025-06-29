using nanoFramework.Networking;
using nanoFramework.Telegram.Bot.Core;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace nanoFramework.Telegram.Bot.Example
{
    public class Program
    {
        private static string WifiSSID = "<INSERT_WIFI_SSID_HERE>";
        private static string WifiPass = "<INSERT_WIFI_PASSWORD_HERE>";
        private static string TelegramBotToken = "<INSERT_TELEGRAM_TOKEN_HERE>";

        private static long MyTelegramId = 123456789;
        public static void Main()
        {
            var isWifiConnected = WifiNetworkHelper.ConnectDhcp(WifiSSID, WifiPass, WifiReconnectionKind.Automatic, true, 0, CancellationToken.None);

            if(!isWifiConnected)
            {
                Debug.WriteLine("Wifi is not connected!");

                return;
            }

            var httpClient = new HttpClient();
            httpClient.SslProtocols = System.Net.Security.SslProtocols.Tls12;

            //Enabling certificates is mandatory. If you don't want to use them,
            //or if you want to use a self-signed certificate, use this:
            //httpClient.SslVerification = System.Net.Security.SslVerification.NoVerification;
            //Also you can insert your certificate chain to string variable and not use resource manager
            var certificates = Resources.GetString(Resources.StringResources.CertificatesTree);
            httpClient.HttpsAuthentCert = new X509Certificate(certificates);

            var telegram = new TelegramBot(TelegramBotToken, httpClient);

            telegram.Events.OnError += (details) =>
            {
                Debug.WriteLine($"OnError: {details.Message}");
            };
            telegram.Events.OnMessageReceived += (message) =>
            {
                Debug.WriteLine($"OnMessageReceived: {message.text}");
                if (message.from.id == MyTelegramId)
                {
                    var replyCommand = new SendTelegramMessageCommand()
                    {
                        chat_id = message.chat.id,
                        text = "Hello, master!",
                        reply_parameters = new Core.Models.ReplyMarkup.ReplyParameters
                        {
                            chat_id = message.chat.id,
                            message_id = message.message_id,
                            quote = message.text,
                            quote_parse_mode = "Markdown"
                        }
                    };
                    telegram.Send(replyCommand);
                }
            };

            var connectionTest = telegram.CheckConnection();
            telegram.StartReceiving();

            if (!connectionTest.ok)
            {
                Debug.WriteLine($"Telegram connection error");
                Debug.WriteLine($"Error code: {connectionTest.error_code}");
                Debug.WriteLine($"Error description: {connectionTest.description}");

                WifiNetworkHelper.Disconnect();
                return;
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
