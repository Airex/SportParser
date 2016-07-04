namespace TelegramBot.Core
{
    public class sendMessage:IMethod
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; } = "";
        public bool? disable_web_page_preview { get; set; } = false;
        public bool? disable_notification { get; set; } = false;
        public int? reply_to_message_id { get; set; }
        public ReplyMarkup reply_markup { get; set; }
    }
}