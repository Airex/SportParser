using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SportParser
{
    public class ResultHolder
    {
        public IDictionary<string, League> Leagues { get; set; } = new ConcurrentDictionary<string, League>();
        public IDictionary<string, Participant> Participants { get; set; } = new ConcurrentDictionary<string, Participant>();
       

    }
}