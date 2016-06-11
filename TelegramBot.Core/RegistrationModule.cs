using System.ComponentModel.Design;
using Autofac;
using TelegramBot.Core.Commands;

namespace TelegramBot.Core
{
    public class RegistrationModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BotHandler>().As<BotHandler>().InstancePerLifetimeScope();

            builder.RegisterType<ApiRequest>().As<IApiRequest>().SingleInstance();
            builder.RegisterType<BotCommandFactory>().As<IBotCommandFactory>().SingleInstance();


            builder.RegisterType<DataManager>().As<IDataManager>().SingleInstance();
            builder.RegisterType<UpdateStrategy>().As<IUpdateStrategy>().SingleInstance();
            builder.RegisterType<FeedLoader>().As<IFeedLoader>();


            builder.RegisterType<ScheduleCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MainMenuCommand>().As<IBotCommandHandler>().SingleInstance();
            builder.RegisterType<OtherLeaguesCommand>().As<IBotCommandHandler>().SingleInstance();
            builder.RegisterType<NowCommand>().As<IBotCommandHandler>().SingleInstance();
            builder.RegisterType<ResultsCommand>().As<IBotCommandHandler>().SingleInstance();
        }
    }
}