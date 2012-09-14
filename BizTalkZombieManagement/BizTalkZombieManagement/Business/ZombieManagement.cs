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



            WmiIAccess wmiAccess = new WmiIAccess();
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
                    UsingFileLayer(serviceInstanceId, wmiAccess, btArtifact);
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
        /// <param name="ServiceInstanceID"></param>
        /// <param name="wmiAccess"></param>
        /// <param name="btArtifact"></param>
        private static void UsingFileLayer(Guid ServiceInstanceID, WmiIAccess wmiAccess, BizTalkArtifacts btArtifact)
        {
            LogHelper.WriteInfo("Saving all message to file...");
            foreach (Guid gu in wmiAccess.ListMessageId)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageId(gu, ServiceInstanceID);
                SaveFile.SaveToFile(gu, sMessage, ConfigParameter.FilePath);
            }
            LogHelper.WriteInfo("All Messages saved to file !");
        }

        /// <summary>
        /// Extract domain and action from Message context
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="sAction"></param>
        /// <param name="sDomaine"></param>
        private static void ExtractDataFromContextMessage(XDocument xDoc, out String sAction, out String sDomaine)
        {
            XNamespace ns = xDoc.Root.Name.Namespace;

            XName elementName = XName.Get("Property");

            sAction = String.Empty;
            sDomaine = String.Empty;

            var nodes = xDoc.Descendants(elementName);
            //search property Domaine and Action and retrieve value
            foreach (var node in nodes)
            {
                if (node.Attributes().Where(p => String.Equals(p.Value, "Domaine")).Count() == 1)
                    sDomaine = node.Attributes().First(p => p.Name == "Value").Value;

                if (node.Attributes().Where(p => p.Value == "Action").Count() == 1)
                    sAction = node.Attributes().First(p => p.Name == "Value").Value;

            }
        }

       
    }
}
