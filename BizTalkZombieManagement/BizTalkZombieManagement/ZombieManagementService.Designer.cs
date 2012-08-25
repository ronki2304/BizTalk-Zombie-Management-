using System.Management;
using BizTalkZombieManagement.Business;
using System;
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
        private static void HandleEvent(object sender, EventArrivedEventArgs e)
        {
            LogHelper.WriteInfo("Event arrived");
            ZombieManagement.ReplayZombieMessage(Guid.Parse(e.NewEvent.Properties[WMIProperties.InstanceID].Value.ToString()));

        }
        #endregion
    }
}
