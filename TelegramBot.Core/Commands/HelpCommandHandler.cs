using System;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class HelpCommandHandler : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject)
        {
            string[] commands = {"/help", "/language", "/15"};
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                text = strings.helpCommand+"\r\n"+string.Join("\r\n",commands)
            });
            return null;
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return updateObject.message.text.StartsWith("/help", StringComparison.InvariantCultureIgnoreCase) || updateObject.message.text.StartsWith("/start", StringComparison.InvariantCultureIgnoreCase);
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; set; } = false;
    }
}