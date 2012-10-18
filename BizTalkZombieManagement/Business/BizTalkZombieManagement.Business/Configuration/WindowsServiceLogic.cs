using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal.Configuration;
using System.Timers;
using BizTalkZombieManagement.Entities.ConstantName;


namespace BizTalkZombieManagement.Business.Configuration
{
    
    public class WindowsServiceLogic
    {
        WindowsServiceAccess service;
        private Timer aTimer;
        public Boolean ServiceFound { get; private set; }
        public String state { get; private set; }

        #region event generation
        public delegate void ServiceStatusChangedDelegate(object o, ServiceWindowsEvent e);
        public event ServiceStatusChangedDelegate OnStateChange;
        #endregion

        public WindowsServiceLogic()
        {

            service = new WindowsServiceAccess();
            if (service.ServiceFound)
            {
                aTimer = new Timer(WindowsServiceKey.Time);
                aTimer.Interval = WindowsServiceKey.TimeInterval;
                aTimer.Elapsed += TimeElapsed;
                aTimer.Enabled = true;
                ServiceFound = true;
                state = service.GetStatusService();
            }
            else
            {
                ServiceFound = false;
            }
        }
        /// <summary>
        /// When the elapsed time is over we check again the service status
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TimeElapsed(object o, ElapsedEventArgs e)
        {
          
            //first check the service status
            if (ServiceFound && state != service.GetStatusService())
            {
                //keep the lastest state
                state = service.GetStatusService();
                //generate event for UI
                OnStateChange(this, new ServiceWindowsEvent(state));
            }
            
        }

       


    }
}
