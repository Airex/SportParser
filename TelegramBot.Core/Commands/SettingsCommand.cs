using System;
using System.Linq;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class SettingsCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData)
        {
            var text = updateObject.message.text;
            string[] _commands = GetCommands();
            if (text.Equals(EmojiUtils.SettingsIcon+strings.Settings, StringComparison.InvariantCultureIgnoreCase) || text.Equals("/settings",StringComparison.InvariantCultureIgnoreCase))
            {
                apiRequest.ExecuteMethod(new sendMessage()
                {
                    chat_id = updateObject.message.chat.id,
                    text = strings.Available_settings,
                    reply_markup = new ReplyKeyboardMarkup
                    {
                        keyboard = _commands.Select(s => new[] { new KeyboardButton() { text = s }, }).ToArray(),
                        one_time_keyboard = false
                    }
                });
            }
            else if (text.Equals(_commands.Last(),StringComparison.InvariantCultureIgnoreCase))
            {
                return "/mainmenu";
            }
            else if (text.Equals(_commands[0], StringComparison.InvariantCultureIgnoreCase))
            {
                return "/language";
            }

            else if (text.Equals(_commands[1], StringComparison.InvariantCultureIgnoreCase))
            {
                return "/timezone";
            }
            else
            {
                throw new NotImplementedException();
            }
            return null;
        }

        private string[] GetCommands()
        {
            return  new []{ EmojiUtils.LanguageIcon + strings.Language,EmojiUtils.TimezoneIcon + strings.TimeZone,EmojiUtils.BackIcon + strings.Back};
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            string[] _commands = GetCommands();
            var text = updateObject.message.text;
            return string.Equals(text,EmojiUtils.SettingsIcon + strings.Settings, StringComparison.InvariantCultureIgnoreCase) || string.Equals(text,"/settings", StringComparison.InvariantCultureIgnoreCase) || _commands.Any(s => s.Equals(text, StringComparison.InvariantCultureIgnoreCase));
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; } = true;
    }
}