using BoilerMachine.Enums;

namespace BoilerMachine.Models
{
    /// <summary>
    /// Class which consists the systemstatus and switchstatus of the system
    /// </summary>
    public class Boiler
    {
        /// <summary>
        /// Gets or sets the status of the machine
        /// </summary>
        /// <value>Status of system</value>
        public SystemStatus Status { get; set; } = SystemStatus.Lockout;

        /// <summary>
        /// Gets or sets the switch status of the lock
        /// </summary>
        /// <value>Interlock switch status</value>
        public InterlockSwitchStatus SwitchStatus { get; set; } = InterlockSwitchStatus.Open;
    }
}
