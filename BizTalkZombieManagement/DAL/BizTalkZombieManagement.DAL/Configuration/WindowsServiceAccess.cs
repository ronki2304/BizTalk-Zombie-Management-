using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using BizTalkZombieManagement.Entities.ConstantName;

namespace BizTalkZombieManagement.Dal.Configuration
{
    /// <summary>
    /// Handle the service Zombie status
    /// </summary>
    public class WindowsServiceAccess
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
        public String GetStatusService()
        {
            sc.Refresh();
            return sc.Status.ToString();
        }


        /// <summary>
        /// start the service
        /// </summary>
        public void Start()
        {
            sc.Start();
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        public void Stop()
        {
            sc.Stop();
        }
    }
}
