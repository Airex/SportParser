using System;
using SportParser;

namespace TelegramBot.Core
{
    public interface ICommand
    {
        object Execute(Func<DateTime, ResultHolder> getData);
    }
}