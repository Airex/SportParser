using System;

namespace TelegramBot.Core
{
    public interface IUpdateStrategy
    {
        bool NeedUpdate(DateTime? lastUpdate, DateTime date);
    }

    public class UpdateStrategy : IUpdateStrategy
    {
        public bool NeedUpdate(DateTime? lastUpdate, DateTime date)
        {
            if (lastUpdate.GetValueOrDefault(DateTime.MinValue).Date != date.Date) return true;
            var timeSpan = DateTime.Now-lastUpdate.GetValueOrDefault(DateTime.MinValue);
            if (date < DateTime.Now) return false;
            return timeSpan.TotalMinutes>1;
        }
    }
}