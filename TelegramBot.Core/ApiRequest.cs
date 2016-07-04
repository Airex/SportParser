using System;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace TelegramBot.Core
{
    public interface IApiRequest
    {
        void ExecuteMethod(IMethod method);
    }

    public class ApiRequest : IApiRequest
    {
        private readonly string _botToken = ConfigurationManager.AppSettings["telegram.bot.token"];
        private readonly string _apiUrl= ConfigurationManager.AppSettings["telegram.bot.url"];

        public void ExecuteMethod(IMethod method)
        {
            var name = method.GetType().Name;
            var body = JsonConvert.SerializeObject(method, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            SendRequest(name,body);
        }

        private void SendRequest(string method, string body)
        {
            Console.WriteLine(method);
            Console.WriteLine(body);
            var requestUriString = _apiUrl+_botToken+"/"+method;
            Console.WriteLine(requestUriString);
            var webRequest = WebRequest.Create(requestUriString);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            using (var requestStream = webRequest.GetRequestStream())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(body);
                requestStream.Write(bytes,0,bytes.Length);
            }
            var webResponse = webRequest.GetResponse();

            using (var responseStream = webResponse.GetResponseStream())
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {
                        var response = reader.ReadToEnd();
                        Console.WriteLine(response);
                    }
        }
    }
}