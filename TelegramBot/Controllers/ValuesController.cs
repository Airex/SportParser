using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using TelegramBot.Contracts;
using TelegramBot.Core;

namespace TelegramBot.Controllers
{

    public class DoController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post(UpdateObject input)
        {

            BotHandler botHandler = new BotHandler();
            botHandler.Process(input);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
          
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

}