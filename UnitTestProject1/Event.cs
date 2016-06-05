using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Threading;

namespace SportParser
{
    public class Event : DataItem
    {
        public Event(IDictionary<string, string> data) : base(data)
        {
        }

        public string LeagueId => this["labl_id"];
        public string SportId => this["sport_id"];
        public string SportName => this["sport"];
        public string StageType => this["AB"];
        public bool IsTotallyFinished => true;
        public string Stage => this["AC"];
        public bool IsRoundFinish => Stage == "3";
        public bool IsScheduled => StageType == "1";
        public bool IsLive => StageType == "2";
        public bool IsFinished => StageType == "3";
        public bool IsDelayed => Stage == "43";

        public string GetHomeScorePart(int part)
        {
            switch (part)
            {
                case 1:
                    return this["BA"];
                case 2:
                    return this["BC"];
                case 3:
                    return this["BE"];
                case 4:
                    return this["BG"];
                case 5:
                    return this["BI"];
            }
            throw new ArgumentOutOfRangeException(nameof(GetHomeScorePart));
        }

        public string StartUTime => this["AD"];
        public string EndUTime => this["AP"];
        public string HomeScore => this["AG"];
        public string AwayScore => this["AH"];
        public string HomeGameScore => this["WA"];
        public string AwayGamescore => this["WB"];
        public string OddsWinner => this["AZ"];
        public int Service => Convert.ToInt32(this["WC"]);
        public string HomeName => this["AE"];
        public string AwayName => this["AF"];
        public string HomeRedCardCount => this["AJ"];
        public string AwayRedCardCount => this["AK"];
        public string Winner => this["AS"];
        public bool IsHomeWinner => Winner == "1";
        public bool IsAwayWinner => Winner == "2";
        public bool IsGoallessDraw => Winner == "0";
        public string Rank => this["WS"];
        public string ParticipantStatus => this["WT"];
        public string ParticipantStatusSubType => this["VX"];
        public string HomeParticipantCountryId => this["CA"];
        public string AwayParticipantCountryId => this["CB"];
        public string GameTime => this["BX"];
        public string Round => this["ER", ""];
        public string CountryName => this["CC"];
        public string OriginalId => this["original_id"];

        public DateTime StartUDateTime => DateTimeUtils.FromTimeStamp(Convert.ToDouble(StartUTime));

        public override string ToString()
        {
            return HomeName + " - " + AwayName;
        }
    }
}