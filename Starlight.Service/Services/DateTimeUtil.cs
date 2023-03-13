using Starlight.Domain.Models;
using System.Globalization;

namespace Starlight.Service.Services
{
    public class DateTimeUtil
    {
        public static DateTime? SetDatetimeEntities(string value, int index, Utterance u)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            DateTime parsedDate = DateTime.Parse(value);

            // If the time has already passed today, the alarm will be set for tomorrow.
            if (parsedDate.CompareTo(DateTime.Now) <= 0)
                parsedDate = parsedDate.AddDays(1);

            u.Entity.StartIndex = (byte)index;
            u.Entity.EndIndex = (byte)(index + 4);

            return parsedDate;
        }
    }
}