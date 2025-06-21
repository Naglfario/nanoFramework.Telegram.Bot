using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup;
using nanoFramework.Telegram.Bot.Core.Providers;
using nanoFramework.TestFramework;

namespace nanoFramework.Telegram.Bot.Tests
{
    [TestClass]
    public class URLProviderTests
    {
        public class FakeSettingsProvider : ISettingsProvider
        {
            public string Token => "TOKEN";

            public int PollDelayMilliseconds => 500;

            public int UpdatesLimitPerRequest => 1;

            public bool TrackMessages => true;

            public bool TrackCallbackQuery => false;
        }

        [TestMethod]
        public void MinimalFilledCommand_GetUrl_ShouldBeCorrect()
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
        public void MaximumFilledCommand_GetUrl_ShouldBeCorrect()
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
            Assert.Contains($"\"message_id\":{command.reply_parameters.message_id}", act);
            Assert.Contains($"\"chat_id\":{command.reply_parameters.chat_id}", act);
            Assert.Contains($"\"allow_sending_without_reply\":{command.reply_parameters.allow_sending_without_reply.ToString().ToLower()}", act);
            Assert.Contains($"\"quote\":\"{command.reply_parameters.quote}\"", act);
            Assert.Contains($"\"quote_parse_mode\":\"{command.reply_parameters.quote_parse_mode}\"", act);
            Assert.Contains($"\"quote_position\":{command.reply_parameters.quote_position}", act);
            Assert.Contains($"\"force_reply\":{forceReply.force_reply.ToString().ToLower()}", act);
            Assert.Contains($"\"input_field_placeholder\":\"{forceReply.input_field_placeholder}\"", act);
            Assert.Contains($"\"selective\":{forceReply.selective.ToString().ToLower()}", act);
        }
    }
}
