using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.Contracts;

namespace TelegramBot.Core
{
    public static class UserSettings
    {
        static readonly ConcurrentDictionary<int, UserData> _users = new ConcurrentDictionary<int, UserData>();

        public static UserData GetUserData(int userId)
        {
            return _users.GetOrAdd(userId, new UserData());
        }
       
    }

    public class CommandsHistory
    {
        readonly IList<UpdateObject> _list = new List<UpdateObject>();

        public void Add(UpdateObject updateObject)
        {
            _list.Add(updateObject);
            if (_list.Count>10)
                _list.RemoveAt(0);
        }

        public UpdateObject PreviousCommand => _list.LastOrDefault();

   }
}