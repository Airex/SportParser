using System;
using System.IO.Compression;
using SportParser;

namespace TelegramBot.Core
{
    public class FeedLoader : IFeedLoader
    {
        public ResultHolder LoadData(DateTime date)
        {
            var timeSpan = date.Date - DateTime.Now.Date;
            var totalDays = timeSpan.TotalDays;

            var requestUriString = "http://"+$"d.myscore.com.ua/x/feed/f_1_{totalDays}_3_ru_1";
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Accept = "*";
            request.Headers.Add("Accept-Language", "*");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.94 Safari/537.36";
            request.Headers.Add("X-GeoIP", "1");
            request.Headers.Add("X-Fsign", "SW9D1eZo");
            request.Referer = "http://d.myscore.com.ua/x/feed/proxy";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Method = "GET";
            var resp = request.GetResponse();
            var stream = resp.GetResponseStream();

            var reader = new System.IO.StreamReader(new GZipStream(stream, CompressionMode.Decompress), System.Text.Encoding.UTF8);

            var data = reader.ReadToEnd();

            var parser = new Parser();
            return parser.Parse(data);
        }
    }
}