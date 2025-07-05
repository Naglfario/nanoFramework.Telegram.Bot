using nanoFramework.Networking;
using nanoFramework.Telegram.Bot.Core;
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

            //Enabling certificates is mandatory. If you don't want to use them,
            //or if you want to use a self-signed certificate, use this:
            //httpClient.SslVerification = System.Net.Security.SslVerification.NoVerification;
            //Also you can insert your certificate chain to string variable and not use resource manager
            var certificates = Resources.GetString(Resources.StringResources.CertificatesTree);
            httpClient.HttpsAuthentCert = new X509Certificate(certificates);
            httpClient.SslProtocols = System.Net.Security.SslProtocols.Tls12;

            var telegram = new TelegramBot(TelegramBotToken, httpClient);
            var messageReceiver = new MessagesReceiver(telegram, MyTelegramId);

            telegram.Events.OnError += (details) =>
            {
                Debug.WriteLine($"OnError: {details.Message}");
            };
            telegram.Events.OnMessageReceived += messageReceiver.Receive;

            var connectionTest = telegram.CheckConnection();

            // use callback data updates only if you want to work with inline buttons
            //var callbackReceiver = new CallbackReceiver(telegram, MyTelegramId);
            //telegram.Events.OnCallbackQuery += callbackReceiver.Receive;
            //telegram.ToggleCallbackDataUpdatesReceiving(true);

            if (!connectionTest.ok)
            {
                Debug.WriteLine($"Telegram connection error");
                Debug.WriteLine($"Error code: {connectionTest.error_code}");
                Debug.WriteLine($"Error description: {connectionTest.description}");

                WifiNetworkHelper.Disconnect();
                return;
            }

            telegram.StartReceiving();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
