using BoilerMachine.Models;

namespace BoilerMachine.FileHandlers
{
    /// <summary>
    /// Interface to handle file operations
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Method to convert a log data to csv format
        /// </summary>
        /// <param name="log">Log to convert to csv</param>
        /// <returns>Returns the converted string</returns>
        string ConvertToCSV(EventLog log);

        /// <summary>
        /// Method to convert a csv line to log object
        /// </summary>
        /// <param name="line">Line to tranform into object to</param>
        /// <returns>Returns the log object</returns>
        EventLog FromCSV(string line);

        /// <summary>
        /// Method to get all the event logs from the 'log.txt' filw 
        /// </summary>
        /// <returns>Read only list of event logs parsed from the file</returns>
        IReadOnlyList<EventLog> GetAll();

        /// <summary>
        /// Logs the log data into the file
        /// </summary>
        /// <param name="log">Log data to store as csv inside the text file</param>
        void Log(EventLog log);
    }
}
