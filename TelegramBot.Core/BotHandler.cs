using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Botanio.Api;
using SportParser;
using TelegramBot.Contracts;
using TelegramBot.DataAccess;

namespace TelegramBot.Core
{
    public class BotHandler
    {
        private readonly IApiRequest _apiRequest;
        private readonly IBotCommandFactory _botCommandFactory;
        private readonly IDataManager _dataManager;

        public BotHandler(IApiRequest apiRequest, IBotCommandFactory botCommandFactory, IDataManager dataManager)
        {
            _apiRequest = apiRequest;
            _botCommandFactory = botCommandFactory;
            _dataManager = dataManager;
        }

        public void Process(UpdateObject input)
        {
            var stamp = DateTimeManager.FromTimeStamp(input.message.date).ToLocalTime();
           if ((DateTime.Now - stamp).TotalMinutes > 10) return;
            
            var userData = UserSettings.GetUserData(input.message.from.id);

            using (var context = new DataAccess.SportParser())
            {
                var singleOrDefault = context.Users.SingleOrDefault(user => user.userIdRef == input.message.@from.id);
                userData.Language = singleOrDefault?.Settings.SingleOrDefault(setting => setting.name == "Language")?.value ?? "en-US";
                int timeZoneOffset;
                userData.TimeZoneOffset = int.TryParse(
                    singleOrDefault?.Settings.SingleOrDefault(setting => setting.name == "TimeZoneOffset")?.value,
                    out timeZoneOffset) ? timeZoneOffset : 3;

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(userData.Language);
                string commandResult = null;
                do
                {
                    var resolveCommandHandler = _botCommandFactory.ResolveCommandHandler(input);

                    try
                    {
                        if (resolveCommandHandler != null)
                        {
                            Botan.Track(input.message.@from.id.ToString(),input.message.text,resolveCommandHandler.GetType().Name);
                            if (resolveCommandHandler.SupportContext)
                                userData.CurrectHandlerContext = resolveCommandHandler.GetType().Name;
                            commandResult = resolveCommandHandler.ProcessCommand(_apiRequest, _dataManager, input, userData);
                            if (commandResult != null)
                            {
                                userData.CurrectHandlerContext = null;
                                input.message.text = commandResult;
                                input.message.location = null;
                            }
                            userData.CommandsHistory.Add(input);
                        

                        }
                    }
                    catch (NotImplementedException)
                    {
                        _apiRequest.ExecuteMethod(new sendMessage()
                        {
                            text = strings.This_feature_is_under_development,
                            chat_id = input.message.chat.id,
                            reply_to_message_id = input.message.message_id
                        });
                    }
                } while (commandResult != null);

                if (singleOrDefault == null)
                {
                    singleOrDefault = new DataAccess.User() {userIdRef = input.message.@from.id } ;
                    context.Users.Add(singleOrDefault);
                }
                var languageSettings = singleOrDefault?.Settings.SingleOrDefault(setting => setting.name == "Language");
                if (languageSettings == null)
                {
                    languageSettings = new Setting() {name = "Language", User = singleOrDefault};
                    singleOrDefault.Settings.Add(languageSettings);
                }
                languageSettings.value = userData.Language;

                var timeZoneSettings = singleOrDefault?.Settings.SingleOrDefault(setting => setting.name == "TimeZoneOffset");
                if (timeZoneSettings == null)
                {
                    timeZoneSettings = new Setting() { name = "TimeZoneOffset", User = singleOrDefault };
                    singleOrDefault.Settings.Add(timeZoneSettings);
                }
                timeZoneSettings.value = userData.TimeZoneOffset.ToString();

                context.SaveChanges();
            }
        }
    }
}