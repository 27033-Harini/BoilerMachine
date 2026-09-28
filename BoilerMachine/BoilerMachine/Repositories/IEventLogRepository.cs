using BoilerMachine.Models;

namespace BoilerMachine.Repositories
{
    public interface IEventLogRepository
    {
        void Log(EventLog log);
        List<EventLog> GetAll();
    }
}
