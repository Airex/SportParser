using System;
using System.Linq;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class Game15CommandHandler : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject)
        {
            var userData = UserSettings.GetUserData(updateObject.message.@from.id);
            var game15Data = userData.Game15Data;
            bool hasChanges = false;
            if (game15Data == null || updateObject.message.text == "/15")
            {
                game15Data = new int[16];
                for (var i = 0; i < 16; i++)
                {
                    game15Data[i] = i;
                }
                Random r = new Random(Environment.TickCount);
                game15Data = game15Data.Select(i => new {i, ran = r.Next(1000)})
                    .OrderBy(arg => arg.ran)
                    .Select(arg => arg.i)
                    .ToArray();
                hasChanges = true;
            }
            else
            {
                var i = int.Parse(updateObject.message.text);
                if (i > 0)
                {
                    var indexOf = Array.IndexOf(game15Data, i);
                    var offset = new[] {-1, 1, -4, 4};
                    foreach (int t in offset)
                    {
                        var index = indexOf +t;
                        if (index >= 0 && index < 16 && game15Data[index] == 0)
                        {
                            game15Data[index] = i;
                            game15Data[indexOf] = 0;
                            hasChanges = true;
                        }
                    }
                }
            }
            if (hasChanges)
            {
                userData.Game15Data = game15Data;
                var d = game15Data.Select(i => i == 0 ? " " : i.ToString()).ToArray();
                KeyboardButton[][] b =
                {
                    new[]
                    {
                        new KeyboardButton() {text = d[0]}, new KeyboardButton() {text = d[1]},
                        new KeyboardButton() {text = d[2]}, new KeyboardButton() {text = d[3]},
                    },
                    new[]
                    {
                        new KeyboardButton() {text = d[4]}, new KeyboardButton() {text = d[5]},
                        new KeyboardButton() {text = d[6]}, new KeyboardButton() {text = d[7]},
                    },
                    new[]
                    {
                        new KeyboardButton() {text = d[8]}, new KeyboardButton() {text = d[9]},
                        new KeyboardButton() {text = d[10]}, new KeyboardButton() {text = d[11]},
                    },
                    new[]
                    {
                        new KeyboardButton() {text = d[12]}, new KeyboardButton() {text = d[13]},
                        new KeyboardButton() {text = d[14]}, new KeyboardButton() {text = d[15]},
                    },

                };
                bool isWin = true;
                for (int i = 0; i < 16; i++)
                {
                    if (game15Data[i] != i + 1 && (i != 15 || game15Data[i] != 0))
                    {
                        isWin = false;
                        break;
                        ;
                    }
                }

               apiRequest.ExecuteMethod(new sendMessage()
                {
                    chat_id = updateObject.message.chat.id,
                    text = isWin ? strings.Game15CommandHandler_ProcessCommand_you_won___15_to_restart : strings.Game15CommandHandler_ProcessCommand_your_move___15_to_restart,
                    reply_markup = new ReplyKeyboardMarkup
                    {
                        keyboard = b,
                        one_time_keyboard = false
                    },
                    disable_notification = true,
                    reply_to_message_id = updateObject.message.message_id
                });
            }
            return null;    
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            if (updateObject.message.text.Equals("/15")) return true;
            int value;
            return int.TryParse(updateObject.message.text, out value) && value >= 1 && value <= 15;
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; set; } = true;
    }
}