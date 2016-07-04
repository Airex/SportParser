using System.Data.Common;

namespace TelegramBot.Contracts
{
    public class Message
    {
        public int message_id { get; set; }
        public User from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public MessageEntity[] entities { get; set; }
        public Location location { get; set; }

    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class MessageEntity
    {
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string url { get; set; }
        public User user { get; set; }
    }
}