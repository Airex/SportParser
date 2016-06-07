using System.Globalization;
using System.Threading;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class BotHandler
    {
        public void Process(UpdateObject input)
        {
            var userData = UserSettings.GetUserData(input.message.@from.id);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(userData.Language);
            IBotCommandFactory botCommandFactory = new BotCommandFactory(new IBotCommandHandler[] {new HelpCommandHandler(), new LanguageCommandHandler()});
            var resolveCommandHandler = botCommandFactory.ResolveCommandHandler(input);
            resolveCommandHandler?.ProcessCommand(input);
            userData.CommandsHistory.Add(input);
        }
    }

    public interface IBotCommandHandler
    {
        void ProcessCommand(UpdateObject updateObject);
        bool CanHandle(UpdateObject updateObject);
    }
}