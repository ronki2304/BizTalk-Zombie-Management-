using System;
using System.ServiceProcess;

namespace BizTalkZombieManagement.Business.Configuration
{
    /// <summary>
    /// event generate when timer is over and the zombie service have a new status
    /// </summary>
    public class ServiceWindowsEventArgs : EventArgs
    {
        public ServiceControllerStatus NewStatus { get; private set; }

        public ServiceWindowsEventArgs(ServiceControllerStatus status)
        {
            NewStatus = status;
        }
    }
}
