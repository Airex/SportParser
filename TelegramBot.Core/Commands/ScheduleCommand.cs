using System;
using System.Linq;
using System.Text;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class ScheduleCommand : DatesCommand
    {
        protected override KeyboardButton[][] GetDatesKeyboardInternal(DateTime date)
        {
            return KeyboardBuilder.BuildDateKeyboard(date, 7);
        }

        protected override string GetTitle()
        {
            return  "Matches schedule on";
        }

        protected override string ProcessCommandInternal(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject,
            DateTime operationDate)
        {
            var result = dataManager.ExecuteCommand(new TopOnlyLeaguesByDate(operationDate));


            var leagues = result.SingleOrDefault().Value;
            foreach (var league in leagues)
            {
                var builder = new StringBuilder();

                var list = league.Events.Values.Where(e => e.IsScheduled && e.StartUDateTime.Date == operationDate.Date).ToList();
                if (list.Count > 0)
                {
                    builder.AppendLine($"*{league.Title}*");
                    foreach (var ev in list.OrderBy(e => e.StartUTime))
                    {
                        builder.AppendLine(
                            $"{ev.StartUDateTime.ToString("HH:mm dd.MM")} | {ev.HomeName} - {ev.AwayName}");
                    }
                    apiRequest.ExecuteMethod(new sendMessage()
                    {
                        chat_id = updateObject.message.chat.id,
                        text = builder.ToString(),
                        parse_mode = "Markdown"
                    });
                }
            }

            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                text = "/other_leagues",


            });
            return null;
        }

       protected override bool CanHandleInternal(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            return text.StartsWith(EmojiUtils.ScheduleIcon, StringComparison.InvariantCultureIgnoreCase) ||
                   text.StartsWith("/schedule", StringComparison.InvariantCultureIgnoreCase);
        }

        public override int Priority { get; } = 10;
       
    }
}