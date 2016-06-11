using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public abstract class DatesCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject)
        {
            var userData = UserSettings.GetUserData(updateObject.message.@from.id);
            var text = updateObject.message.text;

            if (text == EmojiUtils.HomeIcon + "Home")
                return "/mainmenu";
            if (text == "Today")
                userData.OperationDate = DateTime.Now.Date;
            else
            {
                DateTime date;
                if (DateTime.TryParseExact(text, "dd.MM", null, DateTimeStyles.None, out date))
                {
                    userData.OperationDate = date;
                }
            }

            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                reply_to_message_id = updateObject.message.message_id,
                text = GetTitle()+" " + userData.OperationDate.ToString("dd.MM"),
                reply_markup = new ReplyKeyboardMarkup()
                {
                    one_time_keyboard = false,
                    keyboard = GetDatesKeyboard(DateTime.Now)
                }
            });
            return ProcessCommandInternal(apiRequest,dataManager,updateObject,userData.OperationDate);
        }

        private KeyboardButton[][] GetDatesKeyboard(DateTime date)
        {
            KeyboardButton[][] k = GetDatesKeyboardInternal(date);
            IList<KeyboardButton[]> list = new List<KeyboardButton[]>(k);
            list.Add(new[] {new KeyboardButton() {text = EmojiUtils.HomeIcon+"Home"} });
            return list.ToArray();
        }

        protected abstract KeyboardButton[][] GetDatesKeyboardInternal(DateTime date);

        protected abstract string GetTitle();

        protected abstract string ProcessCommandInternal(IApiRequest apiRequest, IDataManager dataManager,
            UpdateObject updateObject, DateTime operationDate);

        public bool CanHandle(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            DateTime date;
            return text =="Today" || text ==EmojiUtils.HomeIcon+"Home" || DateTime.TryParseExact(text, "dd.MM", null, DateTimeStyles.None, out date)||  CanHandleInternal(updateObject);
        }

        protected abstract bool CanHandleInternal(UpdateObject updateObject);

        public abstract int Priority { get; }
        public  bool SupportContext { get; } = true;
    }
}