using BoilerMachine.Delegates;
using BoilerMachine.Enums;
using BoilerMachine.Models;
using System.Diagnostics.Tracing;

namespace BoilerMachine.Services
{
    public class BoilerService
    {
        private readonly Boiler _boiler;
        public event ToggleError? OnToggleError;
        public BoilerService(Boiler boiler)
        {
            this._boiler = boiler;
        }
        public bool Toggle()
        {
            if (this._boiler.Status == SystemStatus.Lockout)
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
                return true;
            }
            else
            {
                OnToggleError?.Invoke();
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

        public bool UpdateSystemStatus()
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
