using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Entities.ConstantName;
using System.Management;

namespace BizTalkZombieManagement.Dal
{
    public class WmiIAccess
    {
        #region member
        private List<ManagementObject> _ListZombieMessage;
        public Boolean MessageFound { get; private set; }
        #endregion

        /// <summary>
        /// Default constructor intialize the mesage list
        /// </summary>
        public WmiIAccess()
        {
            _ListZombieMessage = new List<ManagementObject>();
            MessageFound = false;
        }

        #region member
        /// <summary>
        /// return the number of message
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _ListZombieMessage.Count();
            }
        }
        /// <summary>
        /// retrieve a list of message GUID
        /// </summary>
        public IEnumerable<Guid> ListMessageId
        {
            get
            {
                     return _ListZombieMessage.Select(p=> Guid.Parse(p.Properties[WmiProperties.MessageInstanceId].Value.ToString()));
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
                    //check if the message type is not a system message type
                    if (!BizTalkArtifacts.IsSystemSchema(objServiceInstance.Properties[WmiProperties.MessageType].Value.ToString()))
                    {
                        //if not so add to the list
                        _ListZombieMessage.Add(objServiceInstance);

                        if (!MessageFound)
                            MessageFound = true;
                    }
                }
            }
        }

        /// <summary>
        /// All message will be saved plus their own context
        /// Message have .out extension
        /// Context have .xml extension
        /// </summary>
        /// <param name="filePath">Path folder</param>
        public void SaveAllMessageAndContextToFiles(String filePath)
        {

            foreach (ManagementObject message in _ListZombieMessage)
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
