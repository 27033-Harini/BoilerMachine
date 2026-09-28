using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;

namespace BoilerMachine.Services
{
    public class BoilerService
    {
        private readonly Boiler _boiler;
        private CancellationTokenSource _cts;
        private bool IsRunning = false;
        public event InterruptStart? OnStartInterrupted;
        public event ResetRequest? OnResetRequest;
        public event StartCompletion? OnStartCompletion;
        public BoilerService(Boiler boiler)
        {
            this._boiler = boiler;
        }

        public async Task StartSequence()
        {
            this.IsRunning = true;
            this._cts = new CancellationTokenSource();
            try
            {
                await RunSequence(this._cts);
            }
            catch (OperationCanceledException)
            {
                this._cts.Dispose();
                OnStartInterrupted?.Invoke();
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        public async Task RunSequence(CancellationTokenSource tokenSource)
        {
            SystemStatus status = this.GetSystemStatus();
            if (status != SystemStatus.Ready)
            {
                OnResetRequest?.Invoke();
                return;
            }

            await Task.Delay(10 * 1000);
            this.UpdateSystemStatus(SystemStatus.PrePurge);
            await Task.Delay(10 * 1000);
            this.UpdateSystemStatus(SystemStatus.Ignition);
            this.UpdateSystemStatus(SystemStatus.Operable);
            OnStartCompletion?.Invoke();
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
                this._cts.Cancel();
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
