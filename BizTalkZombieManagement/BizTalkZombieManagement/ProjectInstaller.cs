using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using BizTalkZombieManagement.Entities.ConstantName;
using System.Windows.Forms;


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
            this.ZombieManagementServiceInstaller.DelayedAutoStart = true;
            this.ZombieManagementServiceInstaller.Description = "Service to handle BizTalk Zombie instance";
            this.ZombieManagementServiceInstaller.ServicesDependedOn = new String[] { WindowsServiceKey.DependedService }; //check if sso is install

#if DEBUG
            this.serviceProcessInstaller1.Account = ServiceAccount.LocalService;
#else


            //Setting credential
            this.serviceProcessInstaller1.Account = ServiceAccount.User;
#endif
            
        }

       
        public override void Commit(IDictionary savedState)
        {
            MessageBox.Show("BizTalk Zombie Management is now installed, to use it, please open the configuration tool located in the start menu for starting service");
            base.Commit(savedState);
        }

    }
}
