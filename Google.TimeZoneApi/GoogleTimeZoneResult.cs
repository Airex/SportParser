using System;

namespace Google.TimeZoneApi
{
    public class GoogleTimeZoneResult
    {
       public double RawOffset { get; set; }
        public double DstOffset { get; set; }
        /// <summary>
        /// Gets or sets the time zone identifier.
        /// </summary>
        /// <value>
        /// The time zone identifier.
        /// </value>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets the name of the time zone.
        /// </summary>
        /// <value>
        /// The name of the time zone.
        /// </value>
        public string TimeZoneName { get; set; }
    }
}
