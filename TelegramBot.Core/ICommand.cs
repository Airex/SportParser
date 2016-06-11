using System;
using System.Collections.Generic;
using System.Linq;
using SportParser;

namespace TelegramBot.Core
{
    public interface ICommand
    {
        IDictionary<DateTime, League[]> Execute(Func<DateTime,bool, ResultHolder> getData);
    }

    public class TopOnlyLeaguesByDate : ICommand
    {
        private readonly DateTime _date;
        private readonly bool _forceNewData;

        public TopOnlyLeaguesByDate(DateTime date, bool forceNewData = false)
        {
            _date = date;
            _forceNewData = forceNewData;
        }

        public IDictionary<DateTime, League[]> Execute(Func<DateTime,bool, ResultHolder> getData)
        {
            var resultHolder = getData(_date, _forceNewData);

            var enumerable = from l in resultHolder.Leagues.Values
                where l.IsTop 
                select l;

            var pairs = enumerable.ToArray();
            return new Dictionary<DateTime, League[]>() {
                    { _date.Date, pairs}
                 };
        }
    }
}