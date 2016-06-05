using System;

namespace SportParser
{
    public class DateTimeUtils
    {
        public static DateTime FromTimeStamp(double timeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}