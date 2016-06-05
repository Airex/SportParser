using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{

    public class ResultHolder
    {
        public IDictionary<string, League> Leagues { get; set; } = new ConcurrentDictionary<string, League>();
        public IDictionary<string, Participant> Participants { get; set; } = new ConcurrentDictionary<string, Participant>();
        public IDictionary<string, Event> Events { get; set; } = new ConcurrentDictionary<string, Event>();

    }

    public abstract class DataItem
    {
        private readonly IDictionary<string, string> _data;

        protected DataItem(IDictionary<string, string> data)
        {
            _data = data;
        }

        protected string this[string key, string defaultValue = null] => _data.ContainsKey(key) ? _data[key] : defaultValue;
    }

    public class League : DataItem
    {
        public string Id => this["Id"];
        public string OriginalId => Id.Split('_')[1];
        public string SortKey => this["ZX"];
        public bool HasTable => this["ZG"] == "1";
        public bool HasLiveTable => this["ZO"] == "1";
        public bool HasDraw => this["ZG"] == "2";
        public string Title => this["ZA"];
        public string CountryId => this["ZB"];
        public string CountryName => this["ZY"];
        public string TournamentId => this["ZE"];
        public string TournamentStageId => this["ZC"];
        public string TournamentType => this["ZD"];
        public string TournamentStageType => this["ZJ"];
        public string TournamentTemplateKey => this["ZH"];
        public string RaceType => this["ZM"];
        public string CategoryId => TournamentTemplateKey?.Split('_')[0];
        public bool IsRaceTypeRace => RaceType == "r";
        public bool IsRaceTypePractice => RaceType == "p";
        public string RaceInfoText => this["ZN"];
        public string PrizeMoney => this["ZP"];
        public string Par => this["ZQ"];
        public string EventId => this["ZZ"];
        public string MeetingId => this["QM"];
        public string StageTabs => this["ZV"];
        public string Url => this["ZL"];
        public string SportId => this["sport_id"];
        public string SportName => this["sport"];
        public string IsOpen => this["display"];
        public string EventCount => this["g_count"];
        public bool IsPrimary => TournamentType == "p";
        public bool IsSecondary => TournamentType == "s";
        public bool IsTop => TournamentType == "t";
        public bool IsClosed => TournamentType == "c";

        public League(IDictionary<string, string> data) : base(data)
        {
        }
    }

    public class Participant:DataItem
    {
        public Participant(IDictionary<string, string> data) : base(data)
        {
        }

    }

    public class Event : DataItem
    {
        public Event(IDictionary<string, string> data) : base(data)
        {
        }
    }
    class Parser
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

        public ResultHolder Parse(string s)
        {

            ResultHolder result = new ResultHolder();

            string sportId = null;
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
                    var lableId = sportId + "_" + dic["ZC"];

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
                    var originalId = indexValue;
                    var id = "g_" + sportId + "_" + originalId;

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
