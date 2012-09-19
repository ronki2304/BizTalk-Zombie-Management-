using System.Management;
using BizTalkZombieManagement.Business;
using System;
using System.Resources;
using BizTalkZombieManagement.Entity.ConstantName;

[assembly: CLSCompliant(true)]
namespace BizTalkZombieManagement
{
    partial class ZombieManagementService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "ZombieManagementService";
        }

        #endregion


        #region event
        /// <summary>
        /// handle the zombie creation event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HandleZombieEvent(object sender, EventArrivedEventArgs e)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.EventArrived));
            ZombieManagement.ReplayZombieMessage(Guid.Parse(e.NewEvent.Properties[WmiProperties.InstanceId].Value.ToString()));

        }
        #endregion
    }
}
