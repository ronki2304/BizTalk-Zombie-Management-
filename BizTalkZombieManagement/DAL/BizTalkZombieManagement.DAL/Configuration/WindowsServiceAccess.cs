using System;
using System.ServiceProcess;
using BizTalkZombieManagement.Entities.ConstantName;

namespace BizTalkZombieManagement.Dal.Configuration
{
    /// <summary>
    /// Handle the service Zombie status
    /// </summary>
    public sealed class WindowsServiceAccess : IDisposable
    {
        private ServiceController sc;
        private Boolean _serviceFound = false;

        public Boolean ServiceFound
        {
            get
            {
                return _serviceFound;
            }
        }

        /// <summary>
        /// Initialize the windows serrvice object, if not exist ServiceFound equals false
        /// </summary>
        public WindowsServiceAccess()
        {
            foreach (ServiceController s in ServiceController.GetServices())
            {
                if (s.DisplayName == WindowsServiceKey.ServiceName)
                {
                    sc = new ServiceController(WindowsServiceKey.ServiceName);
                    _serviceFound = true;
                }
            }
        }

        /// <summary>
        /// return the current status of the service
        /// </summary>
        /// <returns></returns>
        public ServiceControllerStatus ServiceStatus
        {
            get
            {
                sc.Refresh();
                return sc.Status;
            }
        }


        /// <summary>
        /// start the service and wait until the real state
        /// </summary>
        public void Start()
        {
            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running);
        }

        /// <summary>
        /// Stop the service and wait it is really in stop state
        /// </summary>
        public void Stop()
        {
            sc.Stop();
            sc.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        public void Dispose()
        {
            sc.Dispose();
        }
    }
}
