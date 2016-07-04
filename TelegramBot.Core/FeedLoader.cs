using System;
using System.Collections.Generic;
using System.IO.Compression;
using SportParser;

namespace TelegramBot.Core
{
    public class FeedLoader : IFeedLoader
    {
        private readonly IDictionary<string, FeedSourceSettings> _settings;

        public FeedLoader()
        {
            _settings = new Dictionary<string, FeedSourceSettings>()
            {
                {"ru", new FeedSourceSettings()
                {
                    Host = "http://"+"d.myscore.com.ua/x/feed/",
                    Referer = "http://d.myscore.com.ua/x/feed/proxy",
                    Sign = "SW9D1eZo",
                    ShortLanguageName = "ru"
                } },
                {"en", new FeedSourceSettings()
                {
                    Host = "http://"+"d.flashscore.com/x/feed/",
                    Referer = "http://d.flashscore.com/x/feed/proxy",
                    Sign = "SW9D1eZo",
                    ShortLanguageName = "ru"
                } }
            };
        }

        public ResultHolder LoadData(DateTime date, string language, int timeOffset)
        {
            language = language.Substring(0, 2);
            FeedSourceSettings settings;
            if (!_settings.TryGetValue(language,out settings))
                if (!_settings.TryGetValue(language, out settings))
                    throw new InvalidOperationException("No feed sourse settings found");

            var timeSpan = date.Date - DateTime.UtcNow.Date;
            var totalDays = timeSpan.TotalDays;

            var requestUriString = $"{settings.Host}f_1_{totalDays}_{timeOffset}_{settings.ShortLanguageName}_1";
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Accept = "*";
            request.Headers.Add("Accept-Language", "*");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.94 Safari/537.36";
            request.Headers.Add("X-GeoIP", "1");
            request.Headers.Add("X-Fsign", settings.Sign);
            request.Referer = settings.Referer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Method = "GET";
            var resp = request.GetResponse();
            var stream = resp.GetResponseStream();

            var reader = new System.IO.StreamReader(new GZipStream(stream, CompressionMode.Decompress), System.Text.Encoding.UTF8);

            var data = reader.ReadToEnd();

            var parser = new Parser();
            return parser.Parse(data, timeOffset);
        }
    }

    public class FeedSourceSettings
    {
        public string Host { get; set; }
        public string Sign { get; set; }
        public string Referer { get; set; }
        public string ShortLanguageName { get; set; }
    }
}