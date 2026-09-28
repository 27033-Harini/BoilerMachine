using BoilerMachine.Models;

namespace BoilerMachine.Repositories
{
    public interface IEventLogRepository
    {
        /// <summary>
        /// Logs the event log object content into the text file in csv format 
        /// </summary>
        /// <param name="log"></param>
        void Log(EventLog log);
        
        /// <summary>
        /// Gets all the event logs from the 'log.txt' file.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<EventLog> GetAll();
    }
}
