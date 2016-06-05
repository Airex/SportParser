using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SportParser
{
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

        public IDictionary<string, Event> Events = new ConcurrentDictionary<string, Event>();

        public League(IDictionary<string, string> data) : base(data)
        {
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}