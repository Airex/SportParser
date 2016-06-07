namespace TelegramBot.Core
{
    public class UserData
    {
        public string Language { get; set; } = "en-US";
        public  CommandsHistory CommandsHistory { get; } = new CommandsHistory();
        public string CurrectHandlerContext { get; set; }
        public int[] Game15Data { get; set; }
    }
}