using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace BizTalkZombieManagement.Business.Configuration
{
    /// <summary>
    /// event generate when timer is over and the zombie service have a new status
    /// </summary>
    public class ServiceWindowsEvent : EventArgs
    {
        public ServiceControllerStatus NewStatus { get; private set; }

        public ServiceWindowsEvent(ServiceControllerStatus Status)
        {
            NewStatus = Status;
        }
    }
}
