using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using BizTalkZombieManagement.Dal;
using System.IO;
using BizTalkZombieManagement.Entity.ConstantName;
using System.Threading.Tasks;

namespace BizTalkZombieManagement.Business
{
    public static class ZombieManagement
    {

        /// <summary>
        /// get back the zombie message, replay it and delete the zombie orchestration
        /// </summary>
        /// <param name="serviceInstanceId"></param>
        public static void ReplayZombieMessage(Guid serviceInstanceId)
        {
            Boolean DeleteOrchestrationAction = false;


            //initialize WMI
            WmiIAccess wmiAccess = new WmiIAccess();
            
            //retrieve all zombie message id
            wmiAccess.GetZombieMessage(serviceInstanceId);

            //Initialize artifact list
            BizTalkArtifacts btArtifact = new BizTalkArtifacts();

            // Loop over the returned messages from the query
            if (wmiAccess.MessageFound)
            {
                DeleteOrchestrationAction = true;
                LogHelper.WriteInfo(String.Format("New Message Zombie Found for service instance {0}", serviceInstanceId));
                
                
                //check for save zombie message to file
                if (ConfigParameter.FileActivated)
                {
                    UsingFileLayer(serviceInstanceId, wmiAccess.ListMessageId, btArtifact);
                }
            }
            else
            {
                LogHelper.WriteInfo(String.Format("No zombie message found, the instance {0} is not a zombie instance",serviceInstanceId));
            }

            //Now terminate the current orchestration 
            if (DeleteOrchestrationAction)
            {
                LogHelper.WriteInfo("Now delete zombie orchestration");
                WmiIAccess.TerminateOrchestration(serviceInstanceId);
            }
        }

        /// <summary>
        /// Saving all messages in directory
        /// </summary>
        /// <param name="ServiceInstanceID">Service instance concerned</param>
        /// <param name="MessagesID">list of all messages ID</param>
        /// <param name="btArtifact"></param>
        private static void UsingFileLayer(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BizTalkArtifacts btArtifact)
        {
            LogHelper.WriteInfo("Saving all message to file...");
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                SaveFile.SaveToFile(gu, sMessage, ConfigParameter.FilePath);
            }
            LogHelper.WriteInfo("All Messages saved to file !");
        }
    }
}
