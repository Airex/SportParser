using System;
using System.Collections.Generic;
using System.Globalization;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public abstract class DatesCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData)
        {
            var text = updateObject.message.text;

            if (text == EmojiUtils.HomeIcon + strings.Home)
            {
                userData.OperationDate = DateTime.Now.Date;
                return "/mainmenu";
            }
            if (text == strings.Today)
                userData.OperationDate = DateTime.Now.Date;
            else
            {
                DateTime date;
                userData.OperationDate = DateTime.TryParseExact(text, "dd.MM", null, DateTimeStyles.None, out date) ? date : DateTime.Now.Date;
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
            var k = GetDatesKeyboardInternal(date);
            var list = new List<KeyboardButton[]>(k)
            {
                new[] {new KeyboardButton {text = EmojiUtils.HomeIcon + strings.Home}}
            };
            return list.ToArray();
        }

        protected abstract IEnumerable<KeyboardButton[]> GetDatesKeyboardInternal(DateTime date);

        protected abstract string GetTitle();

        protected abstract string ProcessCommandInternal(IApiRequest apiRequest, IDataManager dataManager,
            UpdateObject updateObject, DateTime operationDate);

        public bool CanHandle(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            DateTime date;
            return text ==strings.Today || text ==EmojiUtils.HomeIcon+strings.Home || DateTime.TryParseExact(text, "dd.MM", null, DateTimeStyles.None, out date)||  CanHandleInternal(updateObject);
        }

        protected abstract bool CanHandleInternal(UpdateObject updateObject);

        public abstract int Priority { get; }
        public  bool SupportContext { get; } = true;
    }
}