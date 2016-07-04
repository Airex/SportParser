using System;
using System.Linq;
using Botanio.Api;
using NUnit.Framework;
using TelegramBot.Core;
using SportParser = TelegramBot.DataAccess.SportParser;

namespace UnitTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
           FeedLoader feedLoader = new FeedLoader();
            var result = feedLoader.LoadData(DateTime.Now,"ru", 6);
           
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
                from e in l.Value.Events
                select e.Value;

            var r = a.FirstOrDefault();
//            var keyValuePairs = result.Leagues.Where(pair => string.Equals(pair.Value.CountryName ,"АРГЕНТИНА", StringComparison.InvariantCultureIgnoreCase)).Select(pair => pair.Value.Title).ToList();
        }

        [Test]
        public void SendMessageTest()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = 225813207,
                text   = "<a href=\"http://telegram.me/Euro_2016bot?text=Home\">text</a>",
                parse_mode = "HTML"
                
            });
        }

        [Test]
        public void BotanTest()
        {
            Botan.Track("1","1234","test");
        }

        [Test]
        public void DBTest()
        {
            TelegramBot.DataAccess.SportParser sportParser = new TelegramBot.DataAccess.SportParser();
            var user = sportParser.Users.SingleOrDefault(u => u.userIdRef == 1);
            Assert.That(user, Is.Not.Null);
            var setting = user.Settings.SingleOrDefault(s => s.name=="Language");
            Assert.That(setting, Is.Not.Null);
            Assert.That(setting.value, Is.EqualTo("en-US"));

        }

        [Test]
        public void TimeZonesTest()
        {
            FeedLoader feedLoader = new FeedLoader();
            var result1 = feedLoader.LoadData(DateTime.Now, "ru", 3);
            var result2 = feedLoader.LoadData(DateTime.Now, "ru", 0);

           var a = from l in result1.Leagues

                    where
                        l.Value.IsTop
                    from e in l.Value.Events
                    select e.Value;

            var b = from l in result2.Leagues

                    where
                        l.Value.IsTop
                    from e in l.Value.Events
                    select e.Value;

            var r1 = a.FirstOrDefault();
            var r2 = b.FirstOrDefault();

            Console.WriteLine(r1);
            Console.WriteLine(r2);
            //            var keyValuePairs = result.Leagues.Where(pair => string.Equals(pair.Value.CountryName ,"АРГЕНТИНА", StringComparison.InvariantCultureIgnoreCase)).Select(pair => pair.Value.Title).ToList();
        }
    }
}
