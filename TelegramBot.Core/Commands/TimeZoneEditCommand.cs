using System;
using System.Collections.Generic;
using System.Configuration;
using Google.TimeZoneApi;
using TelegramBot.Contracts;

namespace TelegramBot.Core.Commands
{
    public class TimeZoneEditCommand : IBotCommandHandler
    {
        public string ProcessCommand(IApiRequest apiRequest, IDataManager dataManager, UpdateObject updateObject, UserData userData)
        {
            var text = updateObject.message.text;
            if (string.Equals(text,"/timezone", StringComparison.InvariantCultureIgnoreCase))
            {
                apiRequest.ExecuteMethod(new sendMessage()
                {
                    chat_id = updateObject.message.chat.id,
                    text = string.Format(strings.Select_your_timezone,EmojiUtils.PinIcon,EmojiUtils.MyLocationIcon),
                    reply_markup = new ReplyKeyboardMarkup
                    {
                        keyboard = BuildTimezoneKeyboard(),
                        one_time_keyboard = true,
                        resize_keyboard = true
                    }
                });
            }
            else if (string.Equals(text, EmojiUtils.CancelIcon + strings.Cancel))
            {
                return "/settings";
            }
            else if (updateObject.message.location!=null)
            {
                var identifyTimeZone = IdentifyTimeZone(updateObject.message.location);
                if (!identifyTimeZone.Item2.HasValue)
                {
                    apiRequest.ExecuteMethod(new sendMessage()
                    {
                        chat_id = updateObject.message.chat.id,
                        text = strings.Timezone_was_not_identified__Continue_to_use_default_one
                    });
                }
                else
                {
                    userData.TimeZoneOffset = identifyTimeZone.Item2.Value;
                    apiRequest.ExecuteMethod(new sendMessage()
                    {
                        chat_id = updateObject.message.chat.id,
                        text = string.Format(strings.Timezone_identified_as, userData.TimeZoneOffset, identifyTimeZone.Item1)
                    });
                }
                
                return "/settings";
            }
            else
            {
                int value;
                if (int.TryParse(text, out value) && value >= -12 && value <= 12)
                {
                    userData.TimeZoneOffset = value;
                    apiRequest.ExecuteMethod(new sendMessage()
                    {
                        chat_id = updateObject.message.chat.id,
                        text = strings.Timezone_was_updated
                    });
                    return "/settings";
                }
                else
                {
                    apiRequest.ExecuteMethod(new sendMessage()
                    {
                        chat_id = updateObject.message.chat.id,
                        text = strings.Invalid_value_was_entered,

                    });
                    return "/settings";
                }
            }

            return null;
        }

        private Tuple<string, int?> IdentifyTimeZone(Location location)
        {
            try
            {
                var googleTimeZone = new GoogleTimeZone(ConfigurationManager.AppSettings["google.apiKey"]);
                var googleTimeZoneResult = googleTimeZone.ConvertDateTime(DateTime.Now,
                    new GeoLocation() {Latitude = location.latitude, Longitude = location.longitude});
                
                    return new Tuple<string, int?>(googleTimeZoneResult.TimeZoneName,(int) (googleTimeZoneResult.RawOffset / 3600));
            }
            catch
            {
                return new Tuple<string, int?>("",null);
            }
        }

        private static KeyboardButton[][] BuildTimezoneKeyboard()
        {
            var list = new List<KeyboardButton[]>()
            {
                new[] {new KeyboardButton {text = EmojiUtils.MyLocationIcon + strings.Send_my_location,request_location = true}},
                new[] {new KeyboardButton {text = EmojiUtils.CancelIcon + strings.Cancel}}
            };
            return list.ToArray();
        }

        public bool CanHandle(UpdateObject updateObject)
        {
            var text = updateObject.message.text;
            int value;
            return string.Equals(text,"/timezone") || string.Equals(text,
                EmojiUtils.CancelIcon + strings.Cancel) || int.TryParse(text, out value) || updateObject.message.location != null;
        }

        public int Priority { get; } = 10;
        public bool SupportContext { get; } = true;
    }
}