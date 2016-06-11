using System;
using System.Linq;
using System.Text;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class NowCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject)
        {
            var result = dataManager.ExecuteCommand(new TopOnlyLeaguesByDate(DateTime.Now, true));

            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id = updateObject.message.chat.id,
                reply_to_message_id = updateObject.message.message_id,
                text = "Matches schedule on " + DateTime.Now.ToString("dd.MM")
            });
            var leagues = result.SingleOrDefault().Value;
            foreach (var league in leagues)
            {
                var builder = new StringBuilder();

                var list = league.Events.Values.Where(e => e.IsLive && e.StartUDateTime.Date == DateTime.Now.Date).ToList();
                if (list.Count > 0)
                {
                    builder.AppendLine($"*{league.Title}*");
                    foreach (var ev in list.OrderBy(e => e.StartUTime))
                    {
                        builder.AppendLine($"{ev.StartUDateTime.ToString("HH:mm dd.MM")} | {ev.HomeName} *{ev.HomeScore}* - *{ev.AwayScore}* {ev.AwayName}");
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

        public bool CanHandle(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            return (text.StartsWith(EmojiUtils.NowIcon, StringComparison.InvariantCultureIgnoreCase)) ||
                   text.StartsWith("/now", StringComparison.InvariantCultureIgnoreCase);
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; set; } = true;
    }
}