using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Business.Configuration
{
    /// <summary>
    /// event generate when timer is over and the zombie service have a new status
    /// </summary>
    public class ServiceWindowsEvent : EventArgs
    {
        public String NewStatus {get; private set;}

        public ServiceWindowsEvent(String Status)
        {
            NewStatus = Status;
        }
    }
}
