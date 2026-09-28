namespace BoilerMachine.Models
{
    /// <summary>
    /// Class to handle error logging and event logging to file.
    /// </summary>
    public class EventLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLog"/> class.
        /// </summary>
        /// <param name="time">Timestamp of any event</param>
        /// <param name="eventDescription">Description of event</param>
        /// <param name="eventData">Data or value the event holds</param>
        public EventLog(DateTime time, string eventDescription, string eventData)
        {
            this.TimeStamp = time;
            this.Event = eventDescription;
            this.EventData = eventData;
        }

        /// <summary>
        /// Gets or sets the timestamp of the event occurence
        /// </summary>
        /// <value>Event occurence timestamp</value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the event name or description
        /// </summary>
        /// <value>Description of event</value>
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the data of the event occured
        /// </summary>
        /// <value>Data of the event</value>
        public string EventData { get; set; }

    }
}
