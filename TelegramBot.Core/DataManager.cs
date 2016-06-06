

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using SportParser;

namespace TelegramBot.Core
{
    public class DataManager
    {
        private readonly IDictionary<DateTime, DataContainer> _data = new ConcurrentDictionary<DateTime, DataContainer>();
        private readonly IUpdateStrategy _updateStrategy;
        private readonly IFeedLoader _feedLoader;
        private static readonly object lockObject = new object();

        public DataManager(IUpdateStrategy updateStrategy, IFeedLoader feedLoader)
        {
            _updateStrategy = updateStrategy;
            _feedLoader = feedLoader;
        }

        private ResultHolder GetData(DateTime date)
        {
            var now = DateTime.Now.Date;
            var timeSpan = date.Date - now;
            if (Math.Abs(timeSpan.TotalDays) > 7)
                throw new InvalidOperationException("Date difference should be less then 7 days");
            lock (lockObject)
            {
                if (_data.ContainsKey(date.Date))
                {
                    var container = _data[date.Date];
                    if (container.Data == null || _updateStrategy.NeedUpdate(container.LastUpdate))
                    {
                        var result = _feedLoader.LoadData(date.Date);
                        _data[date.Date].Update(result);
                    }
                }
                else
                {
                    var result = _feedLoader.LoadData(date.Date);
                    _data.Add(date.Date, new DataContainer(result));
                }
                return _data[date.Date].Data;
            }
        }

        public object ExecuteCommand(ICommand command)
        {
            return command.Execute(GetData);
        }
    }
}