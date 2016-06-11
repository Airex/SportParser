using System;
using System.Linq;
using NUnit.Framework;
using TelegramBot.Core;

namespace UnitTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
           FeedLoader feedLoader = new FeedLoader();
            var result = feedLoader.LoadData(DateTime.Now);
           
            Assert.IsTrue(result.Leagues.Count > 0, "Leagues are not empty");
           // Assert.IsTrue(result.Participants.Count > 0, "Participants are not empty");
            string query = "Словения";

            //            var a = from l in result.Leagues
            //                from e in l.Value.Events
            //                where
            //                    string.Equals(e.Value.HomeName, query, StringComparison.CurrentCultureIgnoreCase) ||
            //                    string.Equals(e.Value.AwayName, query, StringComparison.CurrentCultureIgnoreCase)
            //                select new {League = l.Value, Event = e.Value};

            var a = from l in result.Leagues

                    where
                       l.Value.IsTop
                    select l.Value;

            var r = a.ToList();
//            var keyValuePairs = result.Leagues.Where(pair => string.Equals(pair.Value.CountryName ,"АРГЕНТИНА", StringComparison.InvariantCultureIgnoreCase)).Select(pair => pair.Value.Title).ToList();
        }

        [Test]
        public void SendMessageTest()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = 128756198,
                text   = "🇺🇦",
                
            });
        }
    }
}
