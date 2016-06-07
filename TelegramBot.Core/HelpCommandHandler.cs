using System;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class HelpCommandHandler : IBotCommandHandler
    {
        public void ProcessCommand(UpdateObject updateObject)
        {
            string[] commands = new[] {"/help", "/language", "/15"};
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                text = strings.helpCommand+"\r\n"+string.Join("\r\n",commands)
            });
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return (updateObject.message.text.StartsWith("/help", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}