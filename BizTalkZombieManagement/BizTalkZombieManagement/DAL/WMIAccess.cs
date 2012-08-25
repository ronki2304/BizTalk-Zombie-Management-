using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Entity.ConstanteName;
using System.Management;
using BizTalkZombieManagement.Business;

namespace BizTalkZombieManagement.DAL
{
    public class WMIAccess
    {
        #region member
        private List<ManagementObject> ListZombieMessage;
        public Boolean MessageFound { get; private set; }
        #endregion

        public WMIAccess()
        {
            ListZombieMessage = new List<ManagementObject>();
            MessageFound = false;
        }

        #region member
        public Int32 Count
        {
            get
            {
                return ListZombieMessage.Count();
            }
        }

        public IEnumerable<Guid> ListMessageID
        {
            get
            {
                     return ListZombieMessage.Select(p=> Guid.Parse(p.Properties[WMIProperties.MessageInstanceID].Value.ToString()));
            }
        }
        #endregion


        #region method
        /// <summary>
        /// retrieve all zombie message for one biztalk orchestration service instance
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        public void GetZombieMessage(Guid ServiceInstanceID)
        {
            
            String sQuery = String.Format(WMIQuery.SelectZombieMessage, ServiceInstanceID.ToString("B"));
            ManagementObjectSearcher searchZombieMessages =
                   new ManagementObjectSearcher(new ManagementScope(WMIQuery.WMIScope), new ObjectQuery(sQuery), null);



            foreach (ManagementObject objServiceInstance in searchZombieMessages.Get())
            {
                ListZombieMessage.Add(objServiceInstance);

                if (!MessageFound)
                    MessageFound = true;
            }

        }

        /// <summary>
        /// All message will be saved plus their own context
        /// Message have .out extension
        /// Context have .xml extension
        /// </summary>
        /// <param name="FilePath"></param>
        public void SaveAllMessageAndContextToFiles(String FilePath)
        {

            foreach (ManagementObject message in ListZombieMessage)
            {
                message.InvokeMethod("SaveToFile", new Object[] { FilePath });
            }
        }
        #endregion


        #region static method
        /// <summary>
        /// Terminate Orchestration which have created zombie message
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        public static void TerminateOrchestration(Guid ServiceInstanceID)
        {
            String sQuery = String.Format(WMIQuery.SelectOrchestration, ServiceInstanceID.ToString("B"));
            // à reecrire sans object searcher
            ManagementObjectSearcher searchZombieMessages =
                  new ManagementObjectSearcher(new ManagementScope(WMIQuery.WMIScope), new ObjectQuery(sQuery), null);

            foreach (ManagementObject Mob in searchZombieMessages.Get())
                Mob.InvokeMethod("Terminate", new Object[] { ServiceInstanceID.ToString("B") });

        }
        #endregion
    }
}
