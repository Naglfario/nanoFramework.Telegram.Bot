using nanoFramework.Telegram.Bot.Core;
using nanoFramework.Telegram.Bot.Core.API;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Tests.Fakes;
using nanoFramework.TestFramework;
using System.Net;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Tests.Tests.API
{
    [TestClass]
    public class MessageSenderTests
    {
        [TestMethod]
        public void Send_OkResponse_ShouldReturnOkResult()
        {
            var events = new TelegramBotEvents();
            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.SendMessageResponse);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var settings = new FakeSettingsProvider()
            {
                UseEventsForSendFailures = false
            };
            var target = new MessageSender(events, urlProvider, httpClientProvider, settings);
            var command = new SendTelegramMessageCommand();

            var act = target.Send(command);

            Assert.IsTrue(act.ok);
        }

        [TestMethod]
        public void Send_NullResponse_ShouldReturnNotOkResultAndFireEvent()
        {
            var events = new TelegramBotEvents();
            string error = "";

            events.OnError += (problemDetails) =>
            { error += $"{problemDetails.Message}; "; };

            var urlProvider = new FakeURLProvider();
            var httpClientProvider = new FakeHttpClientProvider(null);
            var settings = new FakeSettingsProvider()
            {
                UseEventsForSendFailures = true
            };
            var target = new MessageSender(events, urlProvider, httpClientProvider, settings);
            var command = new SendTelegramMessageCommand();

            var act = target.Send(command);

            Assert.IsFalse(act.ok);
            Assert.AreNotEqual("", error, "Error was expected");
        }

        [TestMethod]
        public void Send_NullResponseAndDisabledEvents_ShouldReturnNotOkResultAndDontFireEvent()
        {
            var events = new TelegramBotEvents();
            string error = "";

            events.OnError += (problemDetails) =>
            { error += $"{problemDetails.Message}; "; };

            var urlProvider = new FakeURLProvider();
            var httpClientProvider = new FakeHttpClientProvider(null);
            var settings = new FakeSettingsProvider()
            {
                UseEventsForSendFailures = false
            };
            var target = new MessageSender(events, urlProvider, httpClientProvider, settings);
            var command = new SendTelegramMessageCommand();

            var act = target.Send(command);

            Assert.IsFalse(act.ok);
            Assert.AreEqual("", error, "Error is not expected");
        }

        [TestMethod]
        public void Send_Base401Response_ShouldReturnNotOkResultAndFireEvent()
        {
            var events = new TelegramBotEvents();
            string error = "";

            events.OnError += (problemDetails) =>
            { error += $"{problemDetails.Message}; "; };

            var urlProvider = new FakeURLProvider();
            var response = GetResponse(Res.StringResources.BaseResponse401, HttpStatusCode.Unauthorized);
            var httpClientProvider = new FakeHttpClientProvider(response);
            var settings = new FakeSettingsProvider()
            {
                UseEventsForSendFailures = true
            };
            var target = new MessageSender(events, urlProvider, httpClientProvider, settings);
            var command = new SendTelegramMessageCommand();

            var act = target.Send(command);

            Assert.IsFalse(act.ok);
            Assert.AreNotEqual("", error, "Error was expected");
        }

        [TestMethod]
        public void Send_DifferentErrorCodesAndNoContent_ShouldFireError()
        {
            void RunCycle(int start, int end)
            {
                for (var i = start; i <= end; i++)
                {
                    var events = new TelegramBotEvents();
                    string error = "";

                    events.OnError += (problemDetails) =>
                    { error += $"{problemDetails.Message}; "; };

                    var urlProvider = new FakeURLProvider();
                    var httpResponseMessage = new HttpResponseMessage((HttpStatusCode)i);
                    var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
                    var settings = new FakeSettingsProvider()
                    {
                        UseEventsForSendFailures = true
                    };
                    var target = new MessageSender(events, urlProvider, httpClientProvider, settings);
                    var command = new SendTelegramMessageCommand();

                    var act = target.Send(command);

                    Assert.IsFalse(act.ok);
                    Assert.AreNotEqual("", error, "Error was expected");
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
