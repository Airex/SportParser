using System;

namespace TelegramBot.Core
{
    public interface IUpdateStrategy
    {
        bool NeedUpdate(DateTime? lastUpdate);
    }

    public class UpdateStrategy : IUpdateStrategy
    {
        public bool NeedUpdate(DateTime? lastUpdate)
        {
            var timeSpan = DateTime.Now-lastUpdate.GetValueOrDefault(DateTime.MinValue);
            return timeSpan.TotalMinutes>2;
        }
    }
}