using BoilerMachine.Enums;
using BoilerMachine.Models;

namespace BoilerMachine.Services
{
    public interface IBoilerService
    {
        Task StartSequence();
        Task RunSequence(CancellationTokenSource cts);
        bool StopSequence();
        OperationResult Toggle();
        bool SimulateError();
        void UpdateSystemStatus(SystemStatus status);
        bool ResetMachine();
        List<EventLog> GetAll();
    }
}
