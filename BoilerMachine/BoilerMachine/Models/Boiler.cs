using BoilerMachine.Enums;

namespace BoilerMachine.Models
{
    public class Boiler
    {
        public SystemStatus Status { get; set; } = SystemStatus.Lockout;
        public InterlockSwitchStatus SwitchStatus { get; set; } = InterlockSwitchStatus.Open;
    }
}
