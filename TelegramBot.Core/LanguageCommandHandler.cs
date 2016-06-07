using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class LanguageCommandHandler : IBotCommandHandler
    {
        readonly string[] commands = { "/language", "English", "Русский", "Українська" };
        public void ProcessCommand(UpdateObject updateObject)
        {
            if (string.Equals(updateObject.message.text.Trim(), commands[0], StringComparison.InvariantCultureIgnoreCase))
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.ExecuteMethod(new sendMessage()
                {
                    chat_id = updateObject.message.chat.id,
                    text = strings.LanguageCommandHandler_ProcessCommand_Select_language,
                    reply_markup = new ReplyKeyboardMarkup
                    {
                        keyboard = commands.Skip(1).Select(s => new[] {new KeyboardButton() {text = s},} ).ToArray(),
                        one_time_keyboard = true
                    }
                });
            }
            else
            {
                var userData = UserSettings.GetUserData(updateObject.message.@from.id);
                userData.Language = GetLanguage(updateObject.message.text.Trim());
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(userData.Language);
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.ExecuteMethod(new sendMessage()
                {
                    chat_id = updateObject.message.chat.id,
                    text = strings.languageChanged
                });
            }
        }

        private string GetLanguage(string v)
        {
            switch (v)
            {
                case "Русский":
                    return "ru-RU";
                case "Українська":
                    return "uk-UA";
                default:
                    return "en-US";
            }
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            if (commands[0].Equals(updateObject.message.text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                return true;
            var userData = UserSettings.GetUserData(updateObject.message.@from.id);
            var previousCommand = userData.CommandsHistory.PreviousCommand;
            if (previousCommand == null) return false;
            return commands[0].Equals(previousCommand.message.text.Trim()) && commands.Skip(1).Any(s => s.Equals(updateObject.message.text.Trim(), StringComparison.InvariantCultureIgnoreCase) );

        }
    }
}