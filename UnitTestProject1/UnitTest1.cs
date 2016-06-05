using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://d.myscore.com.ua/x/feed/f_1_0_3_ru_1");
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
            System.IO.Compression.GZipStream stram = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);

            System.IO.StreamReader reader = new System.IO.StreamReader(stram, System.Text.Encoding.UTF8);
            
            var data = reader.ReadToEnd();
            Console.Write(data);

            Parser parser = new Parser();
            var result = parser.Parse(data);
            Assert.IsTrue(result.Leagues.Count > 0, "Leagues are not empty");
            Assert.IsTrue(result.Events.Count > 0, "Events are not empty");
           // Assert.IsTrue(result.Participants.Count > 0, "Participants are not empty");
            
        }
    }
}
