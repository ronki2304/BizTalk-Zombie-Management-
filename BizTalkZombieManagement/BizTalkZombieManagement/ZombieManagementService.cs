﻿using System;
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
using BizTalkZombieManagement.Entity.ConstantName;

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
                if (ConfigParameter.IsConfigurationOK())
                {
#if DEBUG
                    LogHelper.WriteInfo("Debug mode enable");

                    //init counter
                    PerfCounterAsync.InitPerformanceCounter();

                    InitializeWatcher();
                    Console.ReadLine();
                    PerfCounterAsync.Dispose();
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
                    LogHelper.WriteError(ResourceLogic.GetString(ResourceKeyName.ErrorValidation));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ResourceLogic.GetString(ResourceKeyName.StopService));
                LogHelper.WriteError(ex);
            }
            
        }
        protected override void OnStart(string[] args)
        {
            //init counter
            PerfCounterAsync.InitPerformanceCounter();

            //start watcher
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
            watcher.EventArrived += new EventArrivedEventHandler(HandleZombieEvent);

            // Start listening
            watcher.Start();
        }

        protected override void OnStop()
        {
            watcher.Stop();
            
            PerfCounterAsync.Dispose();
        }
         
        #region member
        private static ManagementEventWatcher watcher=null;
        #endregion


        
    }
}
