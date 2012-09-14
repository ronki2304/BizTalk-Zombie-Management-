using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Entity.ConstantName;
using System.Management;
using BizTalkZombieManagement.Business;

namespace BizTalkZombieManagement.Dal
{
    public class WmiIAccess
    {
        #region member
        private List<ManagementObject> ListZombieMessage;
        public Boolean MessageFound { get; private set; }
        #endregion

        public WmiIAccess()
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

        public IEnumerable<Guid> ListMessageId
        {
            get
            {
                     return ListZombieMessage.Select(p=> Guid.Parse(p.Properties[WmiProperties.MessageInstanceId].Value.ToString()));
            }
        }
        #endregion


        #region method
        /// <summary>
        /// retrieve all zombie message for one biztalk orchestration service instance
        /// </summary>
        /// <param name="serviceInstanceId"></param>
        public void GetZombieMessage(Guid serviceInstanceId)
        {
            
            String sQuery = String.Format(WmiQuery.SelectZombieMessage, serviceInstanceId.ToString("B"));
            using (ManagementObjectSearcher searchZombieMessages =
                   new ManagementObjectSearcher(new ManagementScope(WmiQuery.WmiScope), new ObjectQuery(sQuery), null))
            {
                foreach (ManagementObject objServiceInstance in searchZombieMessages.Get())
                {
                    ListZombieMessage.Add(objServiceInstance);

                    if (!MessageFound)
                        MessageFound = true;
                }
            }
        }

        /// <summary>
        /// All message will be saved plus their own context
        /// Message have .out extension
        /// Context have .xml extension
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveAllMessageAndContextToFiles(String filePath)
        {

            foreach (ManagementObject message in ListZombieMessage)
            {
                message.InvokeMethod("SaveToFile", new Object[] { filePath });
            }
        }
        #endregion


        #region static method
        /// <summary>
        /// Terminate Orchestration which have created zombie message
        /// </summary>
        /// <param name="serviceInstanceId"></param>
        public static void TerminateOrchestration(Guid serviceInstanceId)
        {
            String sQuery = String.Format(WmiQuery.SelectOrchestration, serviceInstanceId.ToString("B"));
            // à reecrire sans object searcher
            using (ManagementObjectSearcher searchZombieMessages =
                  new ManagementObjectSearcher(new ManagementScope(WmiQuery.WmiScope), new ObjectQuery(sQuery), null))
            {
                foreach (ManagementObject Mob in searchZombieMessages.Get())
                    Mob.InvokeMethod("Terminate", new Object[] { serviceInstanceId.ToString("B") });

            }
        }
        #endregion
    }
}
