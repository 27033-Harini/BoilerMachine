using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;
using BoilerMachine.Repositories;

namespace BoilerMachine.Services
{
    public class BoilerService : IBoilerService
    {
        private readonly Boiler _boiler;
        private readonly EventLogRepository _logRepository;
        private bool IsRunning = false;
        private CancellationTokenSource? _cts;
        public event InterruptStart? OnStartInterrupted;
        public event ResetRequest? OnResetRequest;
        public event StartCompletion? OnStartCompletion;
        public event Action? OnAlreadyRunning;
        public event Started? OnStart;
        public BoilerService(Boiler boiler, EventLogRepository logRepository)
        {
            this._boiler = boiler;
            this._logRepository = logRepository;
        }

        public async Task StartSequence()
        {
            SystemStatus status = this.GetSystemStatus();
            if (status != SystemStatus.Ready)
            {
                OnResetRequest?.Invoke();
                return;
            }
            if (this.IsRunning)
            {
                OnAlreadyRunning?.Invoke();
                return;
            }
            this.IsRunning = true;
            _cts = new CancellationTokenSource();
            try
            {
                OnStart?.Invoke();
                await RunSequence(_cts);
            }
            catch (OperationCanceledException)
            {
                _logRepository.Log(new EventLog(DateTime.Now, "Start sequence cancelled.", _cts.ToString() ?? string.Empty));
                this.IsRunning = false;
                OnStartInterrupted?.Invoke();
            }
        }

        public async Task RunSequence(CancellationTokenSource cts)
        {
            try
            {
                await Task.Delay(10 * 1000, cts.Token);
                this.UpdateSystemStatus(SystemStatus.PrePurge);
                await Task.Delay(10 * 1000, cts.Token);
                this.UpdateSystemStatus(SystemStatus.Ignition);
                this.UpdateSystemStatus(SystemStatus.Operable);
                OnStartCompletion?.Invoke();
            }
            catch(OperationCanceledException)
            {
                _logRepository.Log(new EventLog(DateTime.Now, "Start sequence cancelled.", string.Empty));
                this.IsRunning = false;
                OnStartInterrupted?.Invoke();
            }
            finally
            {
                cts?.Dispose();
                // cts = null;
            }
        }

        public bool StopSequence()
        {
            if (this.IsRunning == false)
            {
                return false;
            }
            else
            {
                this.UpdateSystemStatus(SystemStatus.Ready);
                _cts?.Cancel();
                return true;
            }
        }

        public OperationResult Toggle()
        {
            if (this.IsRunning)
            {
                return new OperationResult(false, "Start sequence is happening. Stop the sequence to toggle.");
            }
            if (this._boiler.Status == SystemStatus.Lockout || this._boiler.Status == SystemStatus.Ready)
            {
                InterlockSwitchStatus currentState = this._boiler.SwitchStatus;
                if (currentState == InterlockSwitchStatus.Open)
                {
                    this._boiler.SwitchStatus = InterlockSwitchStatus.Closed;
                }
                else
                {
                    this._boiler.SwitchStatus = InterlockSwitchStatus.Open;
                }
                _logRepository.Log(new EventLog(DateTime.Now, "Toggle switched", this._boiler.SwitchStatus.ToString()));
                return new OperationResult(true);
            }
            else
            {
                return new OperationResult(false, "The system status is not ready (or) lockout state in order to toggle");
            }
        }

        public bool SimulateError()
        {
            if (this._boiler.Status == SystemStatus.Operable)
            {
                this._boiler.Status = SystemStatus.Lockout;
                _logRepository.Log(new EventLog(DateTime.Now, "Error simulated", this._boiler.Status.ToString()));
                return true;
            }
            else
            {
                return false;
            }
        }

        public InterlockSwitchStatus GetCurrentToggleStatus()
        {
            return this._boiler.SwitchStatus;
        }

        public SystemStatus GetSystemStatus()
        {
            return this._boiler.Status;
        }

        public void UpdateSystemStatus(SystemStatus status)
        {
            this._boiler.Status = status;
            _logRepository.Log(new EventLog(DateTime.Now, "System status updated", this._boiler.Status.ToString()));
        }

        public bool ResetMachine()
        {
            if (this.GetCurrentToggleStatus() == InterlockSwitchStatus.Closed)
            {
                this._boiler.Status = SystemStatus.Ready;
                _logRepository.Log(new EventLog(DateTime.Now, "System reset done", this._boiler.Status.ToString()));
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EventLog> GetAll()
        {
            return this._logRepository.GetAll();
        }
    }
}
