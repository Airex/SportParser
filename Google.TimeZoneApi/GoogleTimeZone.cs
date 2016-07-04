using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Xml.Linq;

namespace Google.TimeZoneApi
{
    public class GoogleTimeZone
    {
        private readonly string apiKey;
      
        public GoogleTimeZone(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Gets the converted date time based on address.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="address">The destination address, city or country.</param>
        /// <returns>
        /// A <see cref="GoogleTimeZoneResult" />.
        /// </returns>
        public GoogleTimeZoneResult ConvertDateTime(DateTime dateTime, GeoLocation location)
        {
            long timestamp = GetUnixTimeStampFromDateTime(TimeZoneInfo.ConvertTimeToUtc(dateTime));

           
            return GetConvertedDateTimeBasedOnAddress(timestamp, location);
        }

        private GoogleTimeZoneResult GetConvertedDateTimeBasedOnAddress(long timestamp, GeoLocation location)
        {
            var numberFormatInfo = new NumberFormatInfo() {CurrencyDecimalSeparator = "."};
            var latitude = location.Latitude.ToString(numberFormatInfo);
            var longitude = location.Longitude.ToString(numberFormatInfo);
            string requestUri =
                $"https://maps.googleapis.com/maps/api/timezone/xml?location={latitude},{longitude}&timestamp={timestamp}&key={this.apiKey}";

            XDocument xdoc = GetXmlResponse(requestUri);

            XElement result = xdoc.Element("TimeZoneResponse");
            XElement status = result.Element("status");

            if (status.Value == "OK")
            {
                var rawOffset = result.Element("raw_offset");
                var dstOfset = result.Element("dst_offset");
                XElement timeZoneId = result.Element("time_zone_id");
                XElement timeZoneName = result.Element("time_zone_name");

                return new GoogleTimeZoneResult()
                {
                    DstOffset = double.Parse(dstOfset.Value,numberFormatInfo),
                    RawOffset = double.Parse(rawOffset.Value, numberFormatInfo),
                    TimeZoneId = timeZoneId.Value,
                    TimeZoneName = timeZoneName.Value
                };
            }
            if (status.Value != "OVER_QUERY_LIMIT") return null;
            XElement errorMessage = result.Element("error_message");
            Debug.Write(errorMessage.Value);
            return null;
        }

        private XDocument GetXmlResponse(string requestUri)
        {
            try
            {
                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                return XDocument.Load(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long GetUnixTimeStampFromDateTime(DateTime dt)
        {
            DateTime epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = dt - epochDate;
            return (int)ts.TotalSeconds;
        }
    }
}
