using BoilerMachine.Models;

namespace BoilerMachine.FileHandlers
{
    public interface IFileHandler
    {
        string ConvertToCSV(EventLog log);
        EventLog FromCSV(string line);
        List<EventLog> GetAll();
        void Log(EventLog log);
    }
}
