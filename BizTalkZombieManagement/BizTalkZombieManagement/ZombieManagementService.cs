using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Management;

namespace BizTalkZombieManagement
{
    partial class ZombieManagementService : ServiceBase
    {
        public ZombieManagementService()
        {
            InitializeComponent();
        }
        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user service may run in the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ZombieManagementService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }
        protected override void OnStart(string[] args)
        {
            WqlEventQuery query = new WqlEventQuery("select * from MSBTS_ServiceInstanceSuspendedEvent");
            ManagementScope Listener = new ManagementScope("root\\MicrosoftBizTalkServer");

            watcher = new ManagementEventWatcher(Listener, query);

            // Set up a listener for events
            watcher.EventArrived +=
                new EventArrivedEventHandler(HandleEvent);

            // Start listening
            watcher.Start();
        }

        protected override void OnStop()
        {
            watcher.Stop();
        }
         

        #region memeber
        private ManagementEventWatcher watcher;
        #endregion
    }
}
