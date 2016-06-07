using System.Collections.Generic;
using System.Linq;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class BotCommandFactory : IBotCommandFactory
    {
        private readonly IEnumerable<IBotCommandHandler> _handlers;

        public BotCommandFactory(IEnumerable<IBotCommandHandler> handlers)
        {
            _handlers = handlers;
        }

        public IBotCommandHandler ResolveCommandHandler(UpdateObject updateObject)
        {

            return _handlers?.FirstOrDefault(botCommandHandler => botCommandHandler.CanHandle(updateObject)) ??
                   new UnknownCommandHandler();
        }
    }
}