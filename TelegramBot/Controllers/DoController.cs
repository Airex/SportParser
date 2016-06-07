using System.Net;
using System.Net.Http;
using System.Web.Http;
using TelegramBot.Contracts;
using TelegramBot.Core;

namespace TelegramBot.Controllers
{

    public class DoController : ApiController
    {
      // POST api/values
        public HttpResponseMessage Post(UpdateObject input)
        {

            BotHandler botHandler = new BotHandler();
            botHandler.Process(input);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }

}