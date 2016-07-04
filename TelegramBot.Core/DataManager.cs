

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using SportParser;

namespace TelegramBot.Core
{
    public class DataManager : IDataManager
    {
        private readonly ConcurrentDictionary<CacheKey, DataContainer> _data = new ConcurrentDictionary<CacheKey, DataContainer>();
        private readonly IUpdateStrategy _updateStrategy;
        private readonly IFeedLoader _feedLoader;
        private static readonly object lockObject = new object();

        public DataManager(IUpdateStrategy updateStrategy, IFeedLoader feedLoader)
        {
            _updateStrategy = updateStrategy;
            _feedLoader = feedLoader;
        }

        private ResultHolder GetData(DateTime date, bool forceUpdate, string language, int timeOffset)
        {
            Console.WriteLine();
            var now = DateTime.Now.Date;
            var timeSpan = date.Date - now;
            if (Math.Abs(timeSpan.TotalDays) > 7)
                throw new InvalidOperationException("Date difference should be less then 7 days");
            var dataContainer = _data.GetOrAdd(new CacheKey(), key =>
            {
                var result = _feedLoader.LoadData(date.Date, language, timeOffset);
                return new DataContainer(result);
            });
            if ( forceUpdate || _updateStrategy.NeedUpdate(dataContainer.LastUpdate, DateTime.Now))
            {
                var result = _feedLoader.LoadData(date.Date, language, timeOffset);
                dataContainer.Update(result);
            }

            return dataContainer.Data;
        }

        public IDictionary<DateTime, League[]> ExecuteCommand(ICommand command, string language, int timeOffset)
        {
            return command.Execute((time, b) => GetData(time, b, language, timeOffset));
        }
    }

    public class CacheKey
    {
        public string Language { get; set; }
        public DateTime Date { get; set; }
        public int TimeOffset { get; set; }

        protected bool Equals(CacheKey other)
        {
            return string.Equals(Language, other.Language) && Date.Equals(other.Date) && TimeOffset == other.TimeOffset;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CacheKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Language?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ Date.GetHashCode();
                hashCode = (hashCode*397) ^ TimeOffset;
                return hashCode;
            }
        }
    }
}