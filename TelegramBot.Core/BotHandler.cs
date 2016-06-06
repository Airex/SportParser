using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public class BotHandler
    {
        public void Process(UpdateObject input)
        {
            var id = input.message.chat.id;
            var text = input.message.text;
            ApiRequest apiRequest = new ApiRequest();

            apiRequest.ExecuteMethod(new sendMessage()
            {
                chat_id                = id,
                text = "Got text: "+text
            });
        }
    }
}