using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;

namespace BoilerMachine.Services
{
    /// <summary>
    /// Interface to handler boiler service operations
    /// </summary>
    public interface IBoilerService
    {
        public event InterruptStart? OnStartInterrupted;
        public event ResetRequest? OnResetRequest;
        public event StartCompletion? OnStartCompletion;
        public event Action? OnAlreadyRunning;
        public event Started? OnStart;
        /// <summary>
        /// Task to start the boiler sequence
        /// </summary>
        /// <returns>Returns the completed task</returns>
        Task StartSequence();

        /// <summary>
        /// Task to change states of system and run the boiler
        /// </summary>
        /// <param name="cts">CancellationTokenSource to indicate cancellation</param>
        /// <returns>Returns a completed task</returns>
        Task RunSequence(CancellationTokenSource cts);
        
        /// <summary>
        /// Stops the sequence if ongoing else informs the user it is not on goind
        /// </summary>
        /// <returns>Returns true if stopped and false if not</returns>
        bool StopSequence();

        /// <summary>
        /// Toggles the switch between "On" and "Off"
        /// </summary>
        /// <returns></returns>
        OperationResult Toggle();
        
        /// <summary>
        /// Method returning a bool indicating error simulation or not
        /// </summary>
        /// <returns>Returns true if operable state else false</returns>
        bool SimulateError();

        /// <summary>
        /// Updates the system status
        /// </summary>
        /// <param name="status">Status to be updated with</param>
        void UpdateSystemStatus(SystemStatus status);

        /// <summary>
        /// Machine could be reset based on specific criteria like switch state being closed.
        /// </summary>
        /// <returns>Returns true upon successful reset and false upon failure</returns>
        bool ResetMachine();

        /// <summary>
        /// Gets all the event log list from the file 'log.txt'
        /// </summary>
        /// <returns>Returns a read only lise of event logs</returns>
        IReadOnlyList<EventLog> GetAll();

        /// <summary>
        /// Gets the current toggle status of the switch
        /// </summary>
        /// <returns>Returns the switch status</returns>
        InterlockSwitchStatus GetCurrentToggleStatus();

        /// <summary>
        /// Gets the system status
        /// </summary>
        /// <returns>System status returned</returns>
        SystemStatus GetSystemStatus();
    }
}
