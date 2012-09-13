using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


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
            this.ZombieManagementServiceInstaller.ServicesDependedOn = new String[] { "ENTSSO" }; //check if sso is install
            //Setting credential
            this.serviceProcessInstaller1.Account = ServiceAccount.User;

            this.AfterInstall += new InstallEventHandler(AfterInstallService);
        }

        private void AfterInstallService(object send, InstallEventArgs e)
        {
            //start the service
            using (ServiceController sc = new ServiceController("ZombieManagementService"))
            {
                sc.Start();
            }
        }
        public override void Commit(IDictionary savedState)
        {
            
        }
    }
}
