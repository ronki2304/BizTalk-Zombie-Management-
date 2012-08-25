using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Management;
using BizTalkZombieManagement.Business;
using System.IO;

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

            try
            {
                //check configuration before starting
                if (IsConfigurationOK())
                {
#if DEBUG
                    LogHelper.WriteInfo("Debug mode enable");
                    InitializeWatcher();
                    Console.ReadLine();
#else
                    System.ServiceProcess.ServiceBase[] ServicesToRun;

                    // More than one user service may run in the same process. To add
                    // another service to this process, change the following line to
                    // create a second service object. For example,
                    //
                    //   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
                    //
                    ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ZombieManagementService() };


                    System.ServiceProcess.ServiceBase.Run(ServicesToRun);
#endif

                }
                else
                {
                    LogHelper.WriteError("Service won't start due to validation error");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("Service encounters a problem and need to be stopped");
                LogHelper.WriteError(ex);
            }
            
        }
        protected override void OnStart(string[] args)
        {

            InitializeWatcher();
        }

        /// <summary>
        /// Initialize the searcher on suspendedEvent
        /// </summary>
        private static void InitializeWatcher()
        {
            WqlEventQuery query = new WqlEventQuery("select * from MSBTS_ServiceInstanceSuspendedEvent");
            ManagementScope Listener = new ManagementScope("root\\MicrosoftBizTalkServer");

            watcher = new ManagementEventWatcher(Listener, query);

            // Set up a listener for events
            watcher.EventArrived += new EventArrivedEventHandler(HandleEvent);

            // Start listening
            watcher.Start();
        }

        protected override void OnStop()
        {
            watcher.Stop();
        }
         
        #region member
        private static ManagementEventWatcher watcher=null;
        #endregion


        private static Boolean IsConfigurationOK()
        {
            Boolean isOK = true;
            //check All AppSetting
            //file key
            if (ConfigParameter.FileActivated)
            {
                if (String.IsNullOrEmpty(ConfigParameter.FilePath))
                {
                    isOK = false;
                    LogHelper.WriteError(String.Format("Folder address is missing"));
                }

                if (!Directory.Exists(ConfigParameter.FilePath)) //folder not found
                {
                    isOK = false;
                    LogHelper.WriteError(String.Format("Folder for dump {0} is not found", ConfigParameter.FilePath));
                }
            }
            

            return isOK;
        }
    }
}
