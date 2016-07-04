using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public interface IBotCommandHandler
    {
        string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData);
        bool CanHandle(UpdateObject updateObject);
        int Priority { get; }
        bool SupportContext { get;}
    }
}