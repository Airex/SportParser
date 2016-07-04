namespace TelegramBot.Core
{
    public class ReplyKeyboardMarkup:ReplyMarkup
    {
        public KeyboardButton[][] keyboard { get; set; }
        public bool? resize_keyboard { get; set; } = null;
        public bool? one_time_keyboard { get; set; }
        public bool? selective { get; set; } = null;

    }

    public class ForceReplyMarkup : ReplyMarkup
    {
        public bool force_reply { get; } = true;
        public bool? selective { get; set; }
    }
}