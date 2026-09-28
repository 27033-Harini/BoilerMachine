using BoilerMachine.Models;

namespace BoilerMachine.FileHandlers
{
    public class FileHandler
    {
        private readonly string _filePath;
        private object _lockObject = new object();
        public FileHandler(string filePath)
        {
            this._filePath = filePath;
        }
        public string ConvertToCSV(EventLog log)
        {
            string csvLine = $"{log.TimeStamp: yy-MM-dd HH:mm:ss},{log.Event},{log.EventData}";
            return csvLine;
        }

        public EventLog FromCSV(string line)
        {
            string[] values = line.Split(',');
            return new EventLog
                        (
                        DateTime.Parse(values[0]),
                        values[1],
                        values[2]
                        );
        }

        public List<EventLog> GetAll()
        {
            List<EventLog> log;
            lock (_lockObject)
            {
                if (!File.Exists(_filePath))
                {
                    return new List<EventLog>();
                }
                string[] content = File.ReadAllLines(_filePath);
                log = new List<EventLog>();
                foreach (string line in content)
                {
                    log.Add(FromCSV(line));
                }
            }
            return log;
        }

        public void Log(EventLog log)
        {
            string csvLine = ConvertToCSV(log);
            lock (_lockObject)
            {
                if (!File.Exists(this._filePath))
                {
                    File.WriteAllText(this._filePath, csvLine + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText(this._filePath, csvLine + Environment.NewLine);
                }
            }
        }
    }
}
