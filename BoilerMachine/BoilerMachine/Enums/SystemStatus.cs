namespace BoilerMachine.Enums
{
    /// <summary>
    /// Indicates the status of the system
    /// </summary>
    public enum SystemStatus
    {
        Lockout = 1,
        Ready,
        PrePurge,
        Ignition,
        Operable,
    }
}
