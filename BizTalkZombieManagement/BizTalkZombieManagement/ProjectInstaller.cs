using System;
using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using System.Windows.Forms;
using BizTalkZombieManagement.Entities.ConstantName;
using BizTalkZombieManagement.Business;


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

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            PerfCounterAsync.CreateCategory();
        }
       
        public override void Commit(IDictionary savedState)
        {
            MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.EndInstallation));
            base.Commit(savedState);
        }


        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            PerfCounterAsync.DeleteCategory();
        }
    }
}
