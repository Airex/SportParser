using System.Linq;
using TelegramBot.Contracts;
using static System.Int32;

namespace TelegramBot.Core.Commands
{
    public class MainMenuCommand:IBotCommandHandler
    {
       
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject)
        {
            string[] commands = {EmojiUtils.ScheduleIcon+"Schedule", EmojiUtils.ResultsIcon+"Results", EmojiUtils.NowIcon+"Now"};
            apiRequest.ExecuteMethod(new sendMessage()
            {
                text = "Available commands:",
                chat_id = updateObject.message.chat.id,
                reply_to_message_id = updateObject.message.message_id,
                reply_markup = new ReplyKeyboardMarkup()
                {
                    one_time_keyboard = false,
                    keyboard = commands.Select(s => new [] { new KeyboardButton() {text = s}}).ToArray()
                }
            });
            return null;
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            return true;
        }

        public int Priority { get; } = MaxValue;
        public bool SupportContext { get; } = false;
    }
}