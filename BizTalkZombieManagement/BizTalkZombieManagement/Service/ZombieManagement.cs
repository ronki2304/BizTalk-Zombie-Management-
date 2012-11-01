using System;
using System.Collections.Generic;
using BizTalkZombieManagement.Business;
using BizTalkZombieManagement.Contracts.CustomInterfaces;
using BizTalkZombieManagement.Entities.ConstantName;

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
            Boolean deleteOrchestrationAction = false;


            //initialize WMI
            WmiLogic wmiAccess = new WmiLogic();

            //Initialize artifact list
            BTArtifactLogic btArtifact = new BTArtifactLogic();

            //retrieve all zombie message id
            wmiAccess.GetZombieMessage(serviceInstanceId);


            // Loop over the returned messages from the query
            if (wmiAccess.MessageFound)
            {
                //need to destroy the orchestration after
                deleteOrchestrationAction = true;
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.ZombieFound), serviceInstanceId));
                //saving all messages
                SaveMessages(serviceInstanceId, wmiAccess.ListMessageId, btArtifact);
            }
            else
            {
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.NoZombieFound), serviceInstanceId));
            }

            //Now terminate the current orchestration 
            if (deleteOrchestrationAction)
            {
                LogHelper.WriteInfo(String.Format(ResourceLogic.GetString(ResourceKeyName.DeleteZombieOrchestration)));
                WmiLogic.TerminateOrchestration(serviceInstanceId);
            }
        }

        /// <summary>
        /// save all message in the selected dump layer
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        /// <param name="MessagesID"></param>
        /// <param name="btArtifact"></param>
        private static void SaveMessages(Guid ServiceInstanceID, IEnumerable<Guid> MessagesID, BTArtifactLogic btArtifact)
        {
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MessageSaving));
            //retrieve the right layer
            ITransportLayer accessLayer = ConfigParameter.GettingTheRightLayer();
            foreach (Guid gu in MessagesID)
            {
                //retrieving the message
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                //save the message
                accessLayer.SendMessage(sMessage, gu);
                //updatecounter
                PerfCounterAsync.UpdateStatistic();
            }
            LogHelper.WriteInfo(ResourceLogic.GetString(ResourceKeyName.MessageSaved));
        }

    }
}
