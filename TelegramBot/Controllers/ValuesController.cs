using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TelegramBot.Contracts;
using TelegramBot.Core;

namespace TelegramBot.Controllers
{
    
    public class DoController : ApiController
    {
        private readonly BotHandler _botHandler;

        public DoController(BotHandler botHandler)
        {
            _botHandler = botHandler;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post(UpdateObject input)
        {
            try
            {
                _botHandler.Process(input);
            }
            catch (Exception)
            {
                
            }
           
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