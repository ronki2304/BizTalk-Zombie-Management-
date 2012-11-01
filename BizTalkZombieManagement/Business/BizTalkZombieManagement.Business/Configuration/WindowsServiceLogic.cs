using System;
using System.ServiceProcess;
using System.Timers;
using BizTalkZombieManagement.Dal.Configuration;
using BizTalkZombieManagement.Entities.ConstantName;


namespace BizTalkZombieManagement.Business.Configuration
{
    
    public sealed class WindowsServiceLogic:IDisposable
    {
        WindowsServiceAccess service;
        private Timer aTimer;
        public Boolean ServiceFound { get; private set; }
        public ServiceControllerStatus State { get; private set; }

        #region event generation
        public delegate void ServiceStatusChangedEventHandler(object sender, ServiceWindowsEventArgs e);
        public event ServiceStatusChangedEventHandler OnStateChange;
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
                State = service.ServiceStatus;
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
            if (ServiceFound && State != service.ServiceStatus)
            {
                //keep the lastest state
                State = service.ServiceStatus;
                //generate event for UI
                OnStateChange(this, new ServiceWindowsEventArgs(State));
            }
            
        }

       public void StartOrStopService()
        {
           switch (State)
           {
               case ServiceControllerStatus.Stopped:
                   service.Start();
                   break;
               case ServiceControllerStatus.Running:
                   service.Stop();
                   break;
               default: break;
           }
        }



       public void Dispose()
       {
           service.Dispose();
           aTimer.Dispose();
       }
    }
}
