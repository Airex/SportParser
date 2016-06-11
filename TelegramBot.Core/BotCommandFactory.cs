using System.Collections.Generic;
using System.Linq;
using TelegramBot.Contracts;
using TelegramBot.Core.Commands;

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
            var userData = UserSettings.GetUserData(updateObject.message.@from.id);
            var currectHandlerContext = userData.CurrectHandlerContext;
            if (currectHandlerContext != null)
            {
                var resolveCommandHandler =
                    _handlers.SingleOrDefault(handler => handler.GetType().Name == currectHandlerContext);
                if (resolveCommandHandler != null)
                {
                    if (resolveCommandHandler.CanHandle(updateObject))
                        return resolveCommandHandler;
                }
            }
            return
                _handlers.OrderBy(handler => handler.Priority)
                    .FirstOrDefault(botCommandHandler => botCommandHandler.CanHandle(updateObject));
        }
    }
}