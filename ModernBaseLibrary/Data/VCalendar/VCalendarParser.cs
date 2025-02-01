
namespace ModernBaseLibrary.VCalendar
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class VCalendarParser
    {
        private List<VCalendarEvent> contents;

        public List<VCalendarEvent> Contents
        {
            get { return contents; }
        }

        public VCalendarParser(VCalendarReader r)
        {
            this.contents = r.GetContents();
        }

        public void WriteToConsole()
        {
            this.contents.ForEach(a => Console.WriteLine(a));
        }

        public static DateTime ToDateTime(string dateString)
        {
            string format = "yyyyMMddTHHmmss";
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString.Replace("\r",string.Empty), format, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime) == false)
            {
                dateTime = new DateTime(1900,1,1,0,0,0);
            }

            return dateTime;
        }
    }
}
