using System;
using System.Globalization;
using System.Threading;
using Botanio.Api;
using SportParser;
using TelegramBot.Contracts;

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
            var stamp = DateTimeManager.FromTimeStamp(input.message.date);
            if ((DateTime.Now - stamp).TotalMinutes > 10) return;
            
            var userData = UserSettings.GetUserData(input.message.@from.id);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(userData.Language);
            string commandResult = null;
            do
            {
                var resolveCommandHandler = _botCommandFactory.ResolveCommandHandler(input);

                try
                {
                    if (resolveCommandHandler != null)
                    {
                        Botan.Track(input.message.from.id.ToString(),input.message.text,resolveCommandHandler.GetType().Name);
                        commandResult = resolveCommandHandler.ProcessCommand(_apiRequest, _dataManager, input);
                        if (commandResult != null)
                        {
                            userData.CurrectHandlerContext = null;
                            input.message.text = commandResult;
                        }
                        userData.CommandsHistory.Add(input);
                        if (resolveCommandHandler.SupportContext)
                            userData.CurrectHandlerContext = resolveCommandHandler.GetType().Name;

                    }
                }
                catch (NotImplementedException)
                {
                    _apiRequest.ExecuteMethod(new sendMessage()
                    {
                        text = "This feature is under development",
                        chat_id = input.message.chat.id,
                        reply_to_message_id = input.message.message_id
                    });
                }
            } while (commandResult != null);
        }
    }
}