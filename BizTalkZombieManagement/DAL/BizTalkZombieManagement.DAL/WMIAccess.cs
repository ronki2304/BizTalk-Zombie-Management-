using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Entities.ConstantName;
using System.Management;
using BizTalkZombieManagement.Entities.Entities;

namespace BizTalkZombieManagement.Dal
{
    public class WmiIAccess
    {
        

        /// <summary>
        /// Default constructor intialize the mesage list
        /// </summary>
        public WmiIAccess()
        {
            
        }

        


        #region method
        /// <summary>
        /// retrieve all zombie message for one biztalk orchestration service instance
        /// </summary>
        /// <param name="serviceInstanceId"></param>
        public static List<WmiResult> GetZombieMessage(Guid serviceInstanceId)
        {
            List<WmiResult> listToReturn = new List<WmiResult>();
            String sQuery = String.Format(WmiQuery.SelectZombieMessage, serviceInstanceId.ToString("B"));
            using (ManagementObjectSearcher searchZombieMessages =
                   new ManagementObjectSearcher(new ManagementScope(WmiQuery.WmiScope), new ObjectQuery(sQuery), null))
            {
                foreach (ManagementObject objServiceInstance in searchZombieMessages.Get())
                {
                    listToReturn.Add(new WmiResult
                        {
                            InstanceID = Guid.Parse(objServiceInstance.Properties[WmiProperties.ServiceInstanceId].Value.ToString())
                           ,
                            MessageType = objServiceInstance.Properties[WmiProperties.MessageType].Value.ToString()
                           ,
                            MessageInstanceId = Guid.Parse(objServiceInstance.Properties[WmiProperties.MessageInstanceId].Value.ToString())

                        });
                }
            }
            return listToReturn;
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
