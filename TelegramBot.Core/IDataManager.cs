using System;
using System.Collections.Generic;
using SportParser;

namespace TelegramBot.Core
{
    public interface IDataManager
    {
        IDictionary<DateTime, League[]> ExecuteCommand(ICommand command);
    }
}