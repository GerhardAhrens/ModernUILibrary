namespace ModernBaseLibrary.VCalendar
{
    public class VCalendarEvent
    {
        public string Created { get; set; }

        public string Description { get; set; }

        public string DTEnd { get; set; }

        public string DTStamp { get; set; }

        public string DTStart { get; set; }

        public string LastModified { get; set; }

        public string Location { get; set; }

        public string Sequence { get; set; }

        public string Summary { get; set; }


        public string Transp { get; set; }

        public string UId { get; set; }

        public override string ToString()
        {
            return
            "CREATED: " + this.Created + "\n" +
            "DESCRIPTION: " + this.Description + "\n" +
            "DTEND: " + this.DTEnd + "\n" +
            "DTSTAMP: " + this.DTStamp + "\n" +
            "DTSTART: " + this.DTStart + "\n" +
            "LAST_MODIFIED: " + this.LastModified + "\n" +
            "LOCATION: " + this.Location + "\n" +
            "SEQUENCE: " + this.Sequence + "\n" +
            "SUMMARY: " + this.Summary + "\n" +
            "TRANSP: " + this.Transp + "\n" +
            "UID: " + this.UId + "\n";
        }
    }

} 
