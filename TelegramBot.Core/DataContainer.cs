using System;
using SportParser;

namespace TelegramBot.Core
{
    public class DataContainer
    {
        public DataContainer(ResultHolder data)
        {
            Data = data;
            LastUpdate = DateTime.Now;
        }

        public DateTime? LastUpdate { get; private set; }
        public ResultHolder Data { get; private set; }

        public void Update(ResultHolder result)
        {
            LastUpdate = DateTime.Now;
            Data = result;
        }
    }
}