using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using System.IO;
using BizTalkZombieManagement.Entities.ConstantName;
using System.Threading.Tasks;
using BizTalkZombieManagement.Business;
using BizTalkZombieManagement.Business.Transport;

namespace BizTalkZombieManagement.Service
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
            WmiLogic wmiAccess = new WmiLogic();

            //Initialize artifact list
            BtArtifactLogic btArtifact = new BtArtifactLogic();

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
                WmiLogic.TerminateOrchestration(serviceInstanceId);
            }
        }

        /// <summary>
        /// Saving all messages in directory
        /// </summary>
        /// <param name="ServiceInstanceID">Service instance concerned</param>
        /// <param name="MessagesID">list of all messages ID</param>
        /// <param name="btArtifact"></param>
        private static void UsingFileLayer(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BtArtifactLogic btArtifact)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.FileSaving));
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                FileLogic fl = new FileLogic();
                fl.SendMessage(sMessage, gu);
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
        private static void UsingMsmqLayer(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BtArtifactLogic btArtifact)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MsmqSaving));
            MsmqLayer msmq = new MsmqLayer();
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                msmq.SendMessage(sMessage,gu);
                //updatecounter
                PerfCounterAsync.UpdateStatistic();
            }
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MsmqSaved));
        }

        private static void UsingWcfLayer(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BtArtifactLogic btArtifact)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.WCFSaving));
            WcfLogic WcfTransport = new WcfLogic();
            foreach (Guid gu in MessagesID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                WcfTransport.SendMessage(sMessage,gu);
                //updatecounter
                PerfCounterAsync.UpdateStatistic();
            }
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.WCFSaved));
        }
    }
}
