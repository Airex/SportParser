using System;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class OtherLeaguesCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return updateObject.message.text.StartsWith("/other_leagues");
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; set; } = true;
    }
}