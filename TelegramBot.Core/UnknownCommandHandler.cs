using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class UnknownCommandHandler : IBotCommandHandler
    {

        public void ProcessCommand(UpdateObject updateObject)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                text = strings.unknownCommand
            });
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            throw new System.NotImplementedException();
        }
    }
}