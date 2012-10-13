using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using BizTalkZombieManagement.Dal;
using System.IO;
using BizTalkZombieManagement.Entities.ConstantName;
using System.Threading.Tasks;
using BizTalkZombieManagement.DAL;

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

            //Initialize artifact list
            BizTalkArtifacts btArtifact = new BizTalkArtifacts();

            //retrieve all zombie message id
            wmiAccess.GetZombieMessage(serviceInstanceId);


            // Loop over the returned messages from the query
            if (wmiAccess.MessageFound)
            {
                DeleteOrchestrationAction = true;
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.ZombieFound), serviceInstanceId));
                
                
                //check for save zombie message to file
                if (ConfigParameter.FileActivated)
                {
                    UsingFileLayer(serviceInstanceId, wmiAccess.ListMessageId, btArtifact);
                }

                //check for send message on MSMQ
                if (ConfigParameter.MsmqActivated)
                {
                    UsingMsmqLayer(serviceInstanceId, wmiAccess.ListMessageId, btArtifact);
                }
            }
            else
            {
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.NoZombieFound), serviceInstanceId));
            }

            //Now terminate the current orchestration 
            if (DeleteOrchestrationAction)
            {
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.DeleteZombieOrchestration)));
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
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.FileSaving));
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                SaveFile.SaveToFile(gu, sMessage, ConfigParameter.FilePath);
                //updatecounter
                PerfCounterAsync.UpdateStatistic();
            }
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.FileSaved));
        }

        /// <summary>
        /// Send all message in MSMQ
        /// </summary>
        /// <param name="ServiceInstanceID">Service instance concerned</param>
        /// <param name="MessagesID">list of all messages ID</param>
        /// <param name="btArtifact"></param>
        private static void UsingMsmqLayer(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BizTalkArtifacts btArtifact)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MsmqSaving));
            MsmqAccess msmqAccess = new MsmqAccess(ConfigParameter.MsmqPath);
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                msmqAccess.SendMesage(sMessage);
                //updatecounter
                PerfCounterAsync.UpdateStatistic();
            }
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MsmqSaved));
        }
    }
}
