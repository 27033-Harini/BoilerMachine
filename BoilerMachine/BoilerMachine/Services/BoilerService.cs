using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;
using System.Runtime.CompilerServices;

namespace BoilerMachine.Services
{
    public class BoilerService
    {
        private readonly Boiler _boiler;
        private bool IsRunning = false;
        private CancellationTokenSource _cts;
        public event InterruptStart? OnStartInterrupted;
        public event ResetRequest? OnResetRequest;
        public event StartCompletion? OnStartCompletion;
        public event Action? OnAlreadyRunning;
        public BoilerService(Boiler boiler, CancellationTokenSource cts)
        {
            this._boiler = boiler;
            this._cts = cts;
        }

        public async Task StartSequence()
        {
            if (this.IsRunning)
            {
                OnAlreadyRunning?.Invoke();
                return;
            }
            this.IsRunning = true;
            try
            {
                await RunSequence(_cts);
            }
            catch (Exception)
            {
                _cts.TryReset();
                this.IsRunning = false;
                OnStartInterrupted?.Invoke();
            }
        }

        public async Task RunSequence(CancellationTokenSource cts)
        {
            SystemStatus status = this.GetSystemStatus();
            if (status != SystemStatus.Ready)
            {
                OnResetRequest?.Invoke();
                return;
            }
            try
            {
                await Task.Delay(10 * 1000);
                this.UpdateSystemStatus(SystemStatus.PrePurge);
                await Task.Delay(10 * 1000);
                this.UpdateSystemStatus(SystemStatus.Ignition);
                this.UpdateSystemStatus(SystemStatus.Operable);
                OnStartCompletion?.Invoke();
            }
            catch
            {
                cts.TryReset();
                this.IsRunning = false;
                OnStartInterrupted?.Invoke();
            }
            
        }

        public bool StopSequence()
        {
            if (!this.IsRunning)
            {
                return false;
            }
            else
            {
                this.UpdateSystemStatus(SystemStatus.Ready);
                _cts.Cancel();
                this.IsRunning = false;
                OnStartInterrupted?.Invoke();
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
        }

        public bool ResetMachine()
        {
            if (this.GetCurrentToggleStatus() == InterlockSwitchStatus.Closed)
            {
                this._boiler.Status = SystemStatus.Ready;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
