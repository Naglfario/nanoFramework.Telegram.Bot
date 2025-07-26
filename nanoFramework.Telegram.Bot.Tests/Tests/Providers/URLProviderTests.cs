using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.Telegram.Bot.Tests.Fakes;
using nanoFramework.TestFramework;

namespace nanoFramework.Telegram.Bot.Tests.Tests.Providers
{
    [TestClass]
    public class URLProviderTests
    {
        [TestMethod]
        public void SendMessage_MinimalFilledCommand_ShouldBeCorrect()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);
            var command = new SendTelegramMessageCommand()
            {
                chat_id = 1,
                text = "hello"
            };

            var act = target.SendMessage(command);

            Assert.AreEqual(
                $"https://api.telegram.org/bot{settings.Token}/sendMessage?" +
                $"chat_id={command.chat_id}&text={command.text}" +
                $"&disable_notification={command.disable_notification}" +
                $"&protect_content={command.protect_content}",
                act);
        }

        [TestMethod]
        public void SendMessage_MaximumFilledCommand_ShouldBeCorrect()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);
            var forceReply = new ForceReply()
            {
                force_reply = true,
                input_field_placeholder = "Placeholder",
                selective = true
            };
            var command = new SendTelegramMessageCommand()
            {
                chat_id = 1,
                text = "hello",
                parse_mode = "MarkdownV2",
                disable_notification = true,
                protect_content = true,
                reply_parameters = new ReplyParameters()
                {
                    message_id = 123,
                    chat_id = 322,
                    allow_sending_without_reply = true,
                    quote = "test",
                    quote_parse_mode = "Html",
                    quote_position = 999
                },
                reply_markup = forceReply
            };
            var act = target.SendMessage(command);

            Assert.Contains($"chat_id={command.chat_id}", act);
            Assert.Contains($"&text={command.text}", act);
            Assert.Contains($"&parse_mode={command.parse_mode}", act);
            Assert.Contains($"&disable_notification={command.disable_notification}", act);
            Assert.Contains($"&protect_content={command.protect_content}", act);
            Assert.Contains($"&protect_content={command.protect_content}", act);
            Assert.DoesNotContains("\n", act);
            Assert.Contains($"message_id%22%3A{command.reply_parameters.message_id}", act);
            Assert.Contains($"chat_id%22%3A{command.reply_parameters.chat_id}", act);
            Assert.Contains($"allow_sending_without_reply%22%3A{command.reply_parameters.allow_sending_without_reply.ToString().ToLower()}", act);
            Assert.Contains($"quote%22%3A%22{command.reply_parameters.quote}", act);
            Assert.Contains($"quote_parse_mode%22%3A%22{command.reply_parameters.quote_parse_mode}", act);
            Assert.Contains($"quote_position%22%3A{command.reply_parameters.quote_position}", act);
            Assert.Contains($"force_reply%22%3A{forceReply.force_reply.ToString().ToLower()}", act);
            Assert.Contains($"input_field_placeholder%22%3A%22{forceReply.input_field_placeholder}", act);
            Assert.Contains($"selective%22%3A{forceReply.selective.ToString().ToLower()}", act);
        }

        [TestMethod]
        public void SendMessage_WithLineBreakChar_ShouldBeEncoded()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);
            var command = new SendTelegramMessageCommand()
            {
                chat_id = 1,
                text = "hello \n world"
            };

            var act = target.SendMessage(command);

            Assert.DoesNotContains("hello \n world", act);
            Assert.Contains("hello+%0A+world", act);
        }

        [TestMethod]
        public void SendMessage_SecondTimeUse_ShouldNotBeEncodedTwice()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);
            var command = new SendTelegramMessageCommand()
            {
                chat_id = 1,
                text = "hello \n world",
                reply_parameters = new ReplyParameters()
                {
                    message_id = 123,
                    chat_id = 322,
                    quote = "space test",
                },
            };

            var firstAct = target.SendMessage(command);
            var secondAct = target.SendMessage(command);

            Assert.DoesNotContains("hello \n world", secondAct);
            Assert.Contains("hello+%0A+world", secondAct);
            Assert.DoesNotContains("space test", secondAct);
            Assert.Contains("space+test", secondAct);
        }

        [TestMethod]
        public void GetMe_ShouldBeCorrect()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);

            var act = target.GetMe();

            Assert.AreEqual(
                $"https://api.telegram.org/bot{settings.Token}/getMe",
                act);
        }

        [TestMethod]
        public void GetUpdates_DefaultRequest_ShouldStartWithBaseUrl()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.StartsWith($"https://api.telegram.org/bot{settings.Token}/getUpdates?", act);
        }

        [TestMethod]
        public void GetUpdates_LastUpdateIdIsZero_ShouldNotContainsOffset()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.DoesNotContains("offset=", act);
        }

        [TestMethod]
        public void GetUpdates_LastUpdateIdIsNotZero_ShouldContainsOffset()
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);
            var offset = 616;

            var act = target.GetUpdates(offset);

            Assert.Contains($"offset={offset+1}", act);
        }

        [TestMethod]
        public void GetUpdates_OnlyTrackMessages_ShouldContainsOnlyTrackMessages()
        {
            var settings = new FakeSettingsProvider()
            {
                TrackMessages = true,
                TrackCallbackQuery = false,
            };
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.DoesNotContains("%22callback_query%22", act);
            Assert.Contains("allowed_updates=[%22message%22]", act);
        }

        [TestMethod]
        public void GetUpdates_OnlyCallback_ShouldContainsOnlyCallback()
        {
            var settings = new FakeSettingsProvider()
            {
                TrackMessages = false,
                TrackCallbackQuery = true,
            };
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.DoesNotContains("%22message%22", act);
            Assert.Contains("allowed_updates=[%22callback_query%22]", act);
        }

        [TestMethod]
        public void GetUpdates_AllUpdateTypes_ShouldContainsMessagesAndCallbacks()
        {
            var settings = new FakeSettingsProvider()
            {
                TrackMessages = true,
                TrackCallbackQuery = true,
            };
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.Contains("allowed_updates=[", act);
            Assert.Contains("%22message%22", act);
            Assert.Contains("%22callback_query%22", act);
        }

        [TestMethod]
        public void GetUpdates_Limit100_ShouldContainsLimit()
        {
            var settings = new FakeSettingsProvider()
            {
                UpdatesLimitPerRequest = 100
            };
            var target = new URLProvider(settings);

            var act = target.GetUpdates(0);

            Assert.Contains($"&limit={settings.UpdatesLimitPerRequest}", act);
        }

        [TestMethod]
        [DataRow("890000000000000000")]
        [DataRow("test")]
        public void AnswerCallbackQuery_ShouldBeCorrect(string callbackId)
        {
            var settings = new FakeSettingsProvider();
            var target = new URLProvider(settings);

            var act = target.AnswerCallbackQuery(callbackId);

            Assert.AreEqual(
                $"https://api.telegram.org/bot{settings.Token}/answerCallbackQuery?callback_query_id={callbackId}",
                act);
        }
    }
}
