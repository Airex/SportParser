using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SportParser
{
    public class Parser
    {
        const char RowEnd = '~';
        const char CellEnd = '¬';
        const char IndexDelimiter = '÷';

        string LEAGUE_INDEX = "ZA";
        string SPORT_INDEX = "SA";
        string EVENT_INDEX = "AA";
        string MOVED_EVENTS_INDEX = "QA";
        string TOP_LEAGUES_INDEX = "SG";
        string U_304_INDEX = "A1";
        string REFRESH_UTIME_INDEX = "A2";
        string DOWNLOAD_UL_FEED_INDEX = "UL";
        string PAST_FUTURE_GAMES_INDEX = "FG";
        string PARTICIPANT_INDEX = "PR";
        string SPECIAL_INDEX = "ST";

        public ResultHolder Parse(string s, int timeOffset)
        {

            ResultHolder result = new ResultHolder();

            string sportId = null;
            string lableId = null;
            var rows = s.Split(RowEnd);
            foreach (var row in rows)
            {
                var cols = row.Split(CellEnd);
                var colsLength = cols.Length - 1;
                var index = cols[0].Split(IndexDelimiter);
                string indexName = null, indexValue = null;
                if (index.Length > 0)
                {
                    indexName = index[0];
                }
                if (index.Length > 1)
                {
                    indexValue = index[1];
                }
               
                if (indexName == SPORT_INDEX)
                {
                    sportId = indexValue;
                }
                else if (indexName == LEAGUE_INDEX)
                {
                    IDictionary<string, string> dic = new Dictionary<string, string>();
                    if (IsNoDuelSport(sportId))
                    {
                        dic.Add("AB", "");
                        dic.Add("AC", "");
                        dic.Add("AD", "");
                    }
                    for (int i = 0; i < colsLength; i++)
                    {
                        var rowParts = cols[i].Split(IndexDelimiter);
                        if (rowParts.Length == 2)
                            dic.Add(rowParts[0], rowParts[1]);

                    }
                    dic.Add("display", (dic["ZD"] != "c").ToString());
                    lableId = sportId + "_" + dic["ZC"];

                    result.Leagues.Add(lableId, new League(dic));
                }
                else if (indexName == PARTICIPANT_INDEX)
                {
                    for (int i = 0; i < colsLength; i++)
                    {
                        var strings = cols[i].Split(IndexDelimiter);
                        var key = strings[0];
                        var value = strings[1];
                        if (key == "PR")
                        {
                            var participantData = value.Split('|');
                            var participantId = participantData[0];
                            IDictionary<string,string> dic = participantData.Select((s1, i1) => new {s1, i1}).ToDictionary(arg => arg.i1.ToString(), arg => arg.s1);
                            result.Participants.Add(participantId,new Participant(dic));
                        }
                    }
                }
                else if (indexName == EVENT_INDEX)
                {
                    IDictionary<string,string> dic = new ConcurrentDictionary<string, string>();
                    var originalId = indexValue;
                    var id = "g_" + sportId + "_" + originalId;
                    dic.Add("labl_id", lableId);
                    dic.Add("original_id", originalId);
                    for (int i = 1; i < colsLength; i++)
                    {
                        var rowParts = cols[i].Split(IndexDelimiter);
                        if (rowParts.Length!=2) continue;

                        var key = rowParts[0];
                        var new_value_string = rowParts[1];

                        if (key == "EA" || key == "EB" || key == "EC" || key == "ED")
                            continue;
                        var new_value = new_value_string;
                        if (dic.ContainsKey(key))
                            dic[key] = new_value;
                        else dic.Add(key,new_value);

                    }
                    var league = result.Leagues[lableId];
                    league.Events.Add(id, new Event(dic, timeOffset));

                }
                else if (indexName == TOP_LEAGUES_INDEX)
                {
                    var tmp = indexValue;
                }
            }
            return result;
        }

        private bool IsNoDuelSport(string id)
        {
            var ids = new[] { 23, 31, 32, 33, 34, 35 };
            return ids.Any(i => i.ToString() == id);
        }
    }
}
