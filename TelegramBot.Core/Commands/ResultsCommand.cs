﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{

    public class ResultsCommand : DatesCommand
    {
        protected override IEnumerable<KeyboardButton[]> GetDatesKeyboardInternal(DateTime date)
        {
            return KeyboardBuilder.BuildDateKeyboard(date, -7);
        }

        protected override string GetTitle()
        {
            return strings.Results_on;
        }

        protected override string ProcessCommandInternal(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject,
            DateTime operationDate)
        {
            var userData = UserSettings.GetUserData(updateObject.message.@from.id);
            var result = dataManager.ExecuteCommand(new TopOnlyLeaguesByDate(operationDate),userData.Language, userData.TimeZoneOffset);

           
            var leagues = result.SingleOrDefault().Value;
            foreach (var league in leagues)
            {
                var builder = new StringBuilder();

                var list = league.Events.Values.Where(e => e.IsFinished && e.StartUDateTime.Date == operationDate).ToList();
                if (list.Count > 0)
                {
                    builder.AppendLine($"*{league.Title}*");
                    foreach (var ev in list.OrderBy(e => e.StartUTime))
                    {
                        builder.AppendLine(
                            $"{ev.StartUDateTime.ToString("HH:mm dd.MM")} | {ev.HomeName} *{ev.HomeScore}* - *{ev.AwayScore}* {ev.AwayName}");
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
                text = "/other_leagues"
                

            });
            return null;
        }


       

        protected override bool CanHandleInternal(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            return (text.StartsWith(EmojiUtils.ResultsIcon+strings.Results, StringComparison.InvariantCultureIgnoreCase)) ||
                   text.StartsWith("/results", StringComparison.InvariantCultureIgnoreCase)
                   ;
        }

        public override int Priority { get; } = 10;
       
    }
}