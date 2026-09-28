namespace BoilerMachine.Models
{
    public class EventLog
    {
        public EventLog(DateTime time, string eventDescription, string eventData)
        {
            this.TimeStamp = time;
            this.Event = eventDescription;
            this.EventData = eventData;
        }
        public DateTime TimeStamp { get; set; }
        public string Event { get; set; }
        public string EventData { get; set; }

    }
}
