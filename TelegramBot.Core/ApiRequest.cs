using System;
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
        private string _botToken = "144111852:AAFt_JWdplHY4FxV1e2Rn1zjVqBPdWOirk8";
        private string _apiUrl= "https://api.telegram.org/bot";

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
            var requestStream = webRequest.GetRequestStream();
            var bytes = System.Text.Encoding.UTF8.GetBytes(body);
            requestStream.Write(bytes,0,bytes.Length);
            requestStream.Close();
            var webResponse = webRequest.GetResponse();
            var responseStream = webResponse.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var response = reader.ReadToEnd();
            Console.WriteLine(response);
        }
    }

    public interface IMethod
    {
        
    }

    public class sendMessage:IMethod
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; } = "";
        public bool? disable_web_page_preview { get; set; } = false;
        public bool? disable_notification { get; set; } = false;
        public int? reply_to_message_id { get; set; }
        public ReplyMarkup reply_markup { get; set; }
    }

    public class ReplyKeyboardMarkup:ReplyMarkup
    {
        public KeyboardButton[][] keyboard { get; set; }
        public bool? resize_keyboard { get; set; } = null;
        public bool? one_time_keyboard { get; set; }
        public bool? selective { get; set; } = null;

    }

    public class KeyboardButton
    {
        public string text { get; set; }
        public bool? request_contact { get; set; }
        public bool? request_location { get; set; }
    }

    public abstract class ReplyMarkup { }
   
}