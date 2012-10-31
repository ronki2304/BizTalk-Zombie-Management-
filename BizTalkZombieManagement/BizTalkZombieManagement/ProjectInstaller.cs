using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using BizTalkZombieManagement.Entities.ConstantName;


namespace BizTalkZombieManagement
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            
            //Configuration service

            this.ZombieManagementServiceInstaller.StartType = ServiceStartMode.Automatic;
            this.ZombieManagementServiceInstaller.Description = "Service to handle BizTalk Zombie instance";
            this.ZombieManagementServiceInstaller.ServicesDependedOn = new String[] { WindowsServiceKey.DependedService }; //check if sso is install

#if DEBUG
#else


            //Setting credential
            this.serviceProcessInstaller1.Account = ServiceAccount.User;
#endif
            this.AfterInstall += new InstallEventHandler(AfterInstallService);
        }

        private void AfterInstallService(object send, InstallEventArgs e)
        {
            //Boolean runConfig = Convert.ToBoolean(Context.Parameters["CHECKBOXA1"]);
            //if (runConfig)
            //{
            //    System.Diagnostics.Process.Start("BizTalkZombieManagement.UI.Configuration.exe");
            //}
            //start the service
            //using (ServiceController sc = new ServiceController("ZombieManagementService"))
            //{
            //    sc.Start();
            //}
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            System.Diagnostics.Process.Start(Context.Parameters["AssemblyPath"]+@"\BizTalkZombieManagement.UI.Configuration.exe");
            
        }
    }
}
