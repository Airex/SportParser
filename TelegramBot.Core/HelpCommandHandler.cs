using System;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class HelpCommandHandler : IBotCommandHandler
    {
        public void ProcessCommand(UpdateObject updateObject)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                text = strings.helpCommand
            });
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return (updateObject.message.text.StartsWith("/help", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}