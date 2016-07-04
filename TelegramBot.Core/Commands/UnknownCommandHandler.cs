using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class UnknownCommandHandler : IBotCommandHandler
    {

        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData)
        {
            apiRequest.ExecuteMethod(new sendMessage
            {
                chat_id = updateObject.message.chat.id,
                text = strings.unknownCommand
            });
            return null;
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return true;
        }

        public int Priority { get; } = int.MaxValue;
        public bool SupportContext { get; } = false;
    }
}