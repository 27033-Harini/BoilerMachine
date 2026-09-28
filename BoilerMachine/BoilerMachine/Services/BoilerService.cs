using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;
using BoilerMachine.Repositories;

namespace BoilerMachine.Services
{
    /// <summary>
    /// Class to handle all the boiler service management
    /// </summary>
    public class BoilerService : IBoilerService
    {
        private readonly Boiler _boiler;
        private readonly IEventLogRepository _logRepository;
        private bool IsRunning = false;
        private CancellationTokenSource? _cts;
        public event InterruptStart? OnStartInterrupted;
        public event ResetRequest? OnResetRequest;
        public event StartCompletion? OnStartCompletion;
        public event Action? OnAlreadyRunning;
        public event Started? OnStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerService"/> class.
        /// </summary>
        /// <param name="boiler">Boiler model object</param>
        /// <param name="logRepository">LogRepository</param>
        public BoilerService(Boiler boiler, IEventLogRepository logRepository)
        {
            this._boiler = boiler;
            this._logRepository = logRepository;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool StopSequence()
        {
            try
            {
                if (this.IsRunning == false)
                {
                    return false;
                }
                else
                {
                    this.UpdateSystemStatus(SystemStatus.Ready);
                    if (_cts != null)
                    {
                        if (_cts.Token.CanBeCanceled)
                        {
                            _cts?.Cancel();
                        }
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public InterlockSwitchStatus GetCurrentToggleStatus()
        {
            return this._boiler.SwitchStatus;
        }

        /// <inheritdoc/>
        public SystemStatus GetSystemStatus()
        {
            return this._boiler.Status;
        }

        /// <inheritdoc/>
        public void UpdateSystemStatus(SystemStatus status)
        {
            this._boiler.Status = status;
            _logRepository.Log(new EventLog(DateTime.Now, "System status updated", this._boiler.Status.ToString()));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IReadOnlyList<EventLog> GetAll()
        {
            return this._logRepository.GetAll();
        }

        /// <inheritdoc/>
        public void InitializeBoiler()
        {
            this._logRepository.Log(new EventLog(DateTime.Now, "Boiler initialized", this._boiler.Status.ToString()));
        }
    }
}
