using BoilerMachine.FileHandlers;
using BoilerMachine.Models;

namespace BoilerMachine.Repositories
{
    public class EventLogRepository
    {
        private readonly FileHandler _fileHandler;
        public EventLogRepository(FileHandler fileHandler)
        {
            this._fileHandler = fileHandler;
        }

        public void Log(EventLog log)
        {
            _fileHandler.Log(log);
        }

        public List<EventLog> GetAll()
        {
            return _fileHandler.GetAll();
        }
    }
}
