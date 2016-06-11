using System;
using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Core
{
    public class KeyboardBuilder
    {
        public static KeyboardButton[][] BuildDateKeyboard(DateTime date, int offset)
        {
            IList<KeyboardButton> list = new List<KeyboardButton>();
            DateTime now = DateTime.Now.Date;
            DateTime offsetDate = now.AddDays(offset);
            DateTime startDate = offsetDate > now ? now : offsetDate;
            DateTime endDate = offsetDate < now ? now : offsetDate;

            for (var i = startDate; i <= endDate; i=i.AddDays(1))
            {
                
                var text = date.Date == i ? "Today" : i.ToString("dd.MM");
                list.Add(new KeyboardButton { text = text });
            }
            return SplitIntoCoulumns(list, 4);
        }

        public static T[][] SplitIntoCoulumns<T>(IList<T> list, int columns)
        {
            IList<T[]> rows = new List<T[]>();
            IList<T> row = new List<T>();
            for (var i = 0; i < list.Count; i++)
            {
                row.Add(list[i]);
                if ((i+1) % columns != 0) continue;
                rows.Add(row.ToArray());
                row.Clear();
            }
            if (row.Count > 0) rows.Add(row.ToArray());
            return rows.ToArray();
        }
    }
}