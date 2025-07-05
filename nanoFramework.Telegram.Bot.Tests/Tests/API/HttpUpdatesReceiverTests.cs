using nanoFramework.Telegram.Bot.Core;
using nanoFramework.Telegram.Bot.Core.Updates;
using nanoFramework.Telegram.Bot.Tests.Fakes;
using nanoFramework.TestFramework;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace nanoFramework.Telegram.Bot.Tests.Tests.API
{
    [TestClass]
    public class HttpUpdatesReceiverTests
    {
        [TestMethod]
        public void Start_WithOneMessageResponse_ShouldFireOneMessage()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.GetUpdatesOneMessageResponse);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual("", errors, "Errors is not expected");
            Assert.AreEqual(1, messageReceived);
            Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
        }

        [TestMethod]
        public void Start_WithOneCallbackResponse_ShouldFireOneCallback()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.GetUpdatesOneCallbackResponse);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual("", errors, "Errors is not expected");
            Assert.AreEqual(0, messageReceived, "Messages is not expected");
            Assert.AreEqual(1, callbacksReceived);
        }

        [TestMethod]
        public void Start_WithCallbackAndMessageResponse_ShouldFireCallbackAndMessage()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.GetUpdatesOneMessageAndOneCallbackResponse);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual(1, messageReceived);
            Assert.AreEqual(1, callbacksReceived);
            Assert.AreEqual("", errors, "Errors is not expected");
        }

        [TestMethod]
        public void Start_WithEmptyResponse_ShouldNotFireMessagesAndCallbacksAndErrors()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.BaseResponseOk);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual(0, messageReceived, "Messages is not expected");
            Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
            Assert.AreEqual("", errors, "Errors is not expected");
        }

        [TestMethod]
        public void Start_WithFormatted401Response_ShouldFireError()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.BaseResponse401, HttpStatusCode.Unauthorized);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual(0, messageReceived, "Messages is not expected");
            Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
            Assert.AreNotEqual("", errors, "Expected one error");
        }

        [TestMethod]
        public void Start_WithNullResponse_ShouldFireError()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var httpClientProvider = new FakeHttpClientProvider(null);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual(0, messageReceived, "Messages is not expected");
            Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
            Assert.AreNotEqual("", errors, "Expected one error");
        }

        [TestMethod]
        public void Start_WithEmptyJson_ShouldFireError()
        {
            var events = new TelegramBotEvents();
            int messageReceived = 0;
            int callbacksReceived = 0;
            string errors = "";

            events.OnError += (problemDetails) =>
            { errors += $"{problemDetails.Message}; "; };
            events.OnCallbackQuery += (callback) =>
            { callbacksReceived++; };
            events.OnMessageReceived += (test) =>
            { messageReceived++; };

            var settings = new FakeSettingsProvider();
            var urlProvider = new FakeURLProvider();
            var httpContent = new FakeHttpContent("{}");
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = httpContent };
            var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
            var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

            target.StartPolling();
            Thread.Sleep(50);

            Assert.AreEqual(0, messageReceived, "Messages is not expected");
            Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
            Assert.AreNotEqual("", errors, "Expected one error");
        }

        [TestMethod]
        public void Start_WithDifferentErrorCodesAndNoContent_ShouldFireError()
        {
            void RunCycle(int start, int end)
            {
                for (var i = start; i <= end; i++)
                {
                    var events = new TelegramBotEvents();
                    int messageReceived = 0;
                    int callbacksReceived = 0;
                    string errors = "";

                    events.OnError += (problemDetails) =>
                    { errors += $"{problemDetails.Message}; "; };
                    events.OnCallbackQuery += (callback) =>
                    { callbacksReceived++; };
                    events.OnMessageReceived += (test) =>
                    { messageReceived++; };

                    var settings = new FakeSettingsProvider();
                    var urlProvider = new FakeURLProvider();
                    var httpResponseMessage = new HttpResponseMessage((HttpStatusCode)i);
                    var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
                    var target = new HttpUpdatesReceiver(events, settings, urlProvider, httpClientProvider);

                    target.StartPolling();
                    Thread.Sleep(50);

                    Assert.AreEqual(0, messageReceived, "Messages is not expected");
                    Assert.AreEqual(0, callbacksReceived, "Callbacks is not expected");
                    Assert.AreNotEqual("", errors, "Expected one error");
                }
            }

            RunCycle(300, 307);
            RunCycle(400, 417);
            RunCycle(500, 505);
        }

        private HttpResponseMessage GetResponse(
            Res.StringResources resource,
            HttpStatusCode status = HttpStatusCode.OK)
        {
            var content = Res.GetString(resource);
            var httpContent = new FakeHttpContent(content);
            var httpResponseMessage = new HttpResponseMessage(status)
            { Content = httpContent };

            return httpResponseMessage;
        }
    }
}
