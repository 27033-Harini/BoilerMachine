using BoilerMachine.FileHandlers;
using BoilerMachine.Models;

namespace BoilerMachine.Repositories
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly IFileHandler _fileHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogRepository"/> class.
        /// </summary>
        /// <param name="fileHandler">File handler object</param>
        public EventLogRepository(IFileHandler fileHandler)
        {
            this._fileHandler = fileHandler;
        }

        /// <inheritdoc/>
        public void Log(EventLog log)
        {
            _fileHandler.Log(log);
        }

        /// <inheritdoc/>
        public IReadOnlyList<EventLog> GetAll()
        {
            return _fileHandler.GetAll();
        }
    }
}
