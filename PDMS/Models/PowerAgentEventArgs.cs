using System;

namespace MAXIMUS.Models
{
    /// <summary>
    /// Event arguments for Power Agent related events
    /// </summary>
    public class PowerAgentEventArgs : EventArgs
    {
        public string OhId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool HasAccessManagement { get; set; }
    }

    public class PowerAgentEnableEventArgs : EventArgs
    {
        public bool IsEnabled { get; private set; }

        public PowerAgentEnableEventArgs(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}