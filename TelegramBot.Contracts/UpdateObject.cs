namespace TelegramBot.Contracts
{
    public class UpdateObject
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }
}