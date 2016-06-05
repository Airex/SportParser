using System.Collections.Generic;

namespace SportParser
{
    public abstract class DataItem
    {
        private readonly IDictionary<string, string> _data;

        protected DataItem(IDictionary<string, string> data)
        {
            _data = data;
        }

        protected string this[string key, string defaultValue = null] => _data.ContainsKey(key) ? _data[key] : defaultValue;
    }
}