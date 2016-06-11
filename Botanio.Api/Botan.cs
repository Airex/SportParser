using System;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Botanio.Api
{
    public class Botan
    {
        private static string token = "4KIgW1Iz-1EzDpDFWNWQqn6FLX_nDRLu";
        public static void Track(string userId, string message, string name)
        {
            var webRequest = WebRequest.Create("https://"+ $"api.botan.io/track?token={HttpUtility.UrlEncode(token)}&uid={HttpUtility.UrlEncode(userId)}&name={HttpUtility.UrlEncode(name)}");
            webRequest.Method = "POST";
            var requestStream = webRequest.GetRequestStream();
            
            var jsonTextWriter = new JsonTextWriter(new StreamWriter(requestStream));
            jsonTextWriter.WriteValue(message);
            jsonTextWriter.Close();
            var webResponse = webRequest.GetResponse();
            var responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            var readToEnd = reader.ReadToEnd();
            Console.WriteLine(readToEnd);
        }
    }
}