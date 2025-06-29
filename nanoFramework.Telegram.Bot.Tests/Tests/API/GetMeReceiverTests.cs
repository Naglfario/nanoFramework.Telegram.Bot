using nanoFramework.Telegram.Bot.Core.API;
using nanoFramework.Telegram.Bot.Tests.Fakes;
using nanoFramework.TestFramework;
using System.Net;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Tests.Tests.API
{
    [TestClass]
    public class GetMeReceiverTests
    {
        [TestMethod]
        public void HttpClientReturnNull_ShouldReturnNullAndNotThrow()
        {
            var urlProvider = new FakeURLProvider();
            var httpClientProvider = new FakeHttpClientProvider(null);
            var target = new GetMeReceiver(urlProvider, httpClientProvider);

            var act = target.GetMe();

            Assert.IsNull(act);
        }

        [TestMethod]
        public void FailedTest_Example()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void HttpClientReturnNotSuccessStatusCode_ShouldReturnNullAndNotThrow()
        {
            var urlProvider = new FakeURLProvider();

            void RunCycle(int start, int end)
            {
                for (var i = start; i <= end; i++)
                {
                    var response = new HttpResponseMessage((HttpStatusCode)i);
                    var httpClientProvider = new FakeHttpClientProvider(response);
                    var target = new GetMeReceiver(urlProvider, httpClientProvider);

                    var act = target.GetMe();

                    Assert.IsNull(act);
                }
            }

            RunCycle(300, 307);
            RunCycle(400, 417);
            RunCycle(500, 505);
        }

        [TestMethod]
        public void HttpClientReturnEmptyJson_ShouldReturnNotOkAndNotThrow()
        {
            var urlProvider = new FakeURLProvider();
            var httpContent = new FakeHttpContent("{}");
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = httpContent };
            var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
            var target = new GetMeReceiver(urlProvider, httpClientProvider);

            var act = target.GetMe();

            Assert.IsFalse(act.ok);
        }

        [TestMethod]
        public void HttpClientReturnFormatted401_ShouldReturnNotNullAndNotOk()
        {
            var urlProvider = new FakeURLProvider();
            var httpResponseMessage = GetResponse(Res.StringResources.BaseResponse401, HttpStatusCode.Unauthorized);

            var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
            var target = new GetMeReceiver(urlProvider, httpClientProvider);

            var act = target.GetMe();

            Assert.IsNotNull(act);
            Assert.IsFalse(act.ok);
        }

        [TestMethod]
        public void HttpClientReturnBotInfo_ShouldNotReturnNotNullAndOk()
        {
            var urlProvider = new FakeURLProvider();
            var httpResponseMessage = GetResponse(Res.StringResources.GetMeResponse);
            var httpClientProvider = new FakeHttpClientProvider(httpResponseMessage);
            var target = new GetMeReceiver(urlProvider, httpClientProvider);

            var act = target.GetMe();

            Assert.IsNotNull(act);
            Assert.IsTrue(act.ok);
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
