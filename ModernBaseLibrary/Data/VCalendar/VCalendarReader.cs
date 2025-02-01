
namespace ModernBaseLibrary.VCalendar
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class VCalendarReader
    {
        private string path;
        private List<VCalendarEvent> calendarEvents;

        public string File
        {
            get { return path; }
        }

        public VCalendarReader(string fp)
        {
            path = fp;
            this.calendarEvents = new List<VCalendarEvent>();
        }

        public List<VCalendarEvent> GetContents()
        {
            return this.prepareContents(new FileStream(this.File, FileMode.Open, FileAccess.Read));
        }

        public List<VCalendarEvent> prepareContents(FileStream fs)
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string ical = sr.ReadToEnd();
                char[] delim = { '\n' };
                string[] lines = ical.Split(delim);
                List<VCalendarEvent> newEvent = new List<VCalendarEvent>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("BEGIN:VEVENT"))
                    {
                       Dictionary<string, string> dictionary = this.grabEvent(i, lines);
                       this.setEvent(dictionary);
                    }
                }

                return this.calendarEvents;
            }
        }

        private List<VCalendarEvent> setEvent(Dictionary<string, string> dictionary)
        {
            VCalendarEvent a = new VCalendarEvent();
            try
            {
                if (dictionary.ContainsKey("CREATED;VALUE=DATE") == true)
                {
                    a.Created = dictionary["CREATED;VALUE=DATE"];
                }

                if (dictionary.ContainsKey("DESCRIPTION") == true)
                {
                    a.Description = dictionary["DESCRIPTION"];
                }

                if (dictionary.ContainsKey("LOCATION") == true)
                {
                    a.Location = dictionary["LOCATION"];
                }

                if (dictionary.ContainsKey("SUMMARY") == true)
                {
                    a.Summary = dictionary["SUMMARY"];
                }

                if (dictionary.ContainsKey("DTEND") == true)
                {
                    a.DTEnd = dictionary["DTEND"];
                }

                if (dictionary.ContainsKey("DTSTAMP") == true)
                {
                    a.DTStamp = dictionary["DTSTAMP"];
                }

                if (dictionary.ContainsKey("DTSTART") == true)
                {
                    a.DTStart = dictionary["DTSTART"];
                }

                if (dictionary.ContainsKey("LAST-MODIFIED;VALUE=DATE") == true)
                {
                    a.DTStart = dictionary["LAST-MODIFIED;VALUE=DATE"];
                }

                if (dictionary.ContainsKey("SEQUENCE") == true)
                {
                    a.Sequence = dictionary["SEQUENCE"];
                }

                if (dictionary.ContainsKey("TRANSP") == true)
                {
                    a.Transp = dictionary["TRANSP"];
                }

                if (dictionary.ContainsKey("UID") == true)
                {
                    a.UId = dictionary["UID"];
                }

                this.calendarEvents.Add(a);
            }
            catch (KeyNotFoundException)
            {
                return new List<VCalendarEvent>();
            }

            return this.calendarEvents;
        }

        private Dictionary<string, string> grabEvent(int i, string[] lines)
        {
            var events = new Dictionary<string, string>();
            for(int j = 0; j < 12; j++)
            {
                string current = lines[i + j];
                if (string.IsNullOrEmpty(current) == true)
                {
                    continue;
                }

                if (!current.Contains("END:VEVENT"))
                {
                    string key = string.Empty;
                    key = current.Substring(0, current.IndexOf(':'));
                    if (current.StartsWith("DTSTART;") == true)
                    {
                        key = "DTSTART";
                    }
                    else if (current.StartsWith("DTSTART:") == true)
                    {
                        key = "DTSTART";
                    }

                    if (current.StartsWith("DTEND;") == true)
                    {
                        key = "DTEND";
                    }
                    else if (current.StartsWith("DTEND:") == true)
                    {
                        key = "DTEND";
                    }

                    int start = current.IndexOf(':') + 1;
                    string value = current.Substring(start);
                    events.Add(key, value);
                }
            }

            return events;
        }
    }
}
