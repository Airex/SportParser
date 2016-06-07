using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public interface IBotCommandFactory
    {
        IBotCommandHandler ResolveCommandHandler(UpdateObject updateObject);
    }
}