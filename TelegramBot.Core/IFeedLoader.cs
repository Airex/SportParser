using System;
using SportParser;

namespace TelegramBot.Core
{
    public interface IFeedLoader
    {
        ResultHolder LoadData(DateTime date);
    }
}