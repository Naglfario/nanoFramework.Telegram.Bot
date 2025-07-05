using nanoFramework.Telegram.Bot.Core;
using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Commands;
using nanoFramework.Telegram.Bot.Core.Models.ReplyMarkup;

namespace nanoFramework.Telegram.Bot.Example
{
    internal class MessagesReceiver
    {
        private readonly TelegramBot _bot;
        private readonly long _adminId;
        private string _customMessage = "Custom message is not set";

        public MessagesReceiver(TelegramBot bot, long adminId)
        {
            _bot = bot;
            _adminId = adminId;
        }

        public void Receive(TelegramMessage message)
        {
            if (message.from.id != _adminId) return;

            if (!message.text.StartsWith("/"))
            {
                _bot.Send(new SendTelegramMessageCommand()
                {
                    chat_id = message.chat.id,
                    text = "Not a command!"
                });
            }
            else HandleCommand(message);
        }

        private void HandleCommand(TelegramMessage message)
        {
            var commandParts = message.text.Split(' ');

            var replyCommand = commandParts[0] switch
            {
                "/setCustom" => SetCustomMessageCommand(message, commandParts),
                "/custom" => CustomMessageCommand(message),
                "/quiet" => QuietCommand(message),
                "/protected" => ProtectedCommand(message),
                "/simpleReply" => SimpleReplyCommand(message),
                "/fullReply" => FullReplyCommand(message),
                "/forceAnswer" => MakeMeAnswerForcedCommand(message),
                "/keyboard" => KeyboardCommand(message),
                "/removeKeyboard" => RemoveKeyboardCommand(message),
                "/inlineKeyboard" => InlineKeyboardCommand(message),
                _ => CommandDoesNotRecognized(message, commandParts[0])
            };

            _bot.Send(replyCommand);
        }

        private SendTelegramMessageCommand CommandDoesNotRecognized(TelegramMessage message, string command)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"Command `{command}` does not recognized",
                parse_mode = "MarkdownV2"
            };

        private SendTelegramMessageCommand SetCustomMessageCommand(TelegramMessage message, string[] commandParts)
        {
            _customMessage = string.Empty;

            for (int i = 1; i < commandParts.Length; i++)
            {
                if (i > 1)
                    _customMessage += " ";

                _customMessage += commandParts[i];
            }

            return new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"OK"
            };
        }

        private SendTelegramMessageCommand CustomMessageCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = _customMessage
            };

        private SendTelegramMessageCommand QuietCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"This message came to you without sound.",
                disable_notification = true
            };

        private SendTelegramMessageCommand ProtectedCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"This message can't be forwarded and saved",
                protect_content = true
            };

        private SendTelegramMessageCommand SimpleReplyCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"Simple reply",
                reply_parameters = new ReplyParameters()
                {
                    message_id = message.message_id,
                    chat_id = message.chat.id // this parameter must be filled
                }
            };

        private SendTelegramMessageCommand FullReplyCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"Full reply",
                reply_parameters = new ReplyParameters()
                {
                    message_id = message.message_id,
                    chat_id = message.chat.id,
                    quote = "/full",
                    quote_position = 0,
                    allow_sending_without_reply = true,
                }
            };

        private SendTelegramMessageCommand MakeMeAnswerForcedCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = $"Now you MUST answer",
                reply_markup = new ForceReply()
                {
                    force_reply = true,
                    input_field_placeholder = "Now you must answer..."
                }
            };

        private SendTelegramMessageCommand KeyboardCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = "I sent you a keyboard",
                reply_markup = new ReplyKeyboardMarkup()
                {
                    keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Press this button and this text will be sent to chat"),
                            new KeyboardButton("/removeKeyboard")
                        },
                    }
                }
            };

        private SendTelegramMessageCommand RemoveKeyboardCommand(TelegramMessage message)
            => new SendTelegramMessageCommand()
            {
                chat_id = message.chat.id,
                text = "Keyboard removed",
                reply_markup = new ReplyKeyboardRemove()
            };

        private SendTelegramMessageCommand InlineKeyboardCommand(TelegramMessage message)
        {
            return new SendTelegramMessageCommand()
            {
                chat_id = message.from.id,
                text = "These are very heavy buttons. Are you sure you need them?",
                reply_markup = new InlineKeyboardMarkup()
                {
                    inline_keyboard = new InlineKeyboardButton[][]
                    {
                        new InlineKeyboardButton[]
                        {
                            new InlineKeyboardButton()
                            {
                                text = "First",
                                callback_data = "this_data_hidden_but_you_receive_it_when_user_press_the_button"
                            },
                            new InlineKeyboardButton()
                            {
                                text = "Second",
                                callback_data = "you_can_store_this_some_variables"
                            },
                            new InlineKeyboardButton()
                            {
                                text = "Third",
                                callback_data = "for_example_from_previous_user_activity"
                            }
                        },
                        new InlineKeyboardButton[]
                        {
                            new InlineKeyboardButton()
                            {
                                text = "Copy",
                                copy_text = new CopyTextButton("This text will be copied to clipboard when user clicks the button")
                            },
                            new InlineKeyboardButton()
                            {
                                text = "Open google",
                                url  = "https://google.com",
                            }
                        }
                    }
                }
            };
        }
    }
}
