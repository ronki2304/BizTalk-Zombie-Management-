using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using BizTalkZombieManagement.Dal;
using System.IO;
using BizTalkZombieManagement.Entity.ConstanteName;
using System.Threading.Tasks;

namespace BizTalkZombieManagement.Business
{
    public static class ZombieManagement
    {

        /// <summary>
        /// get back the zombie message, replay it and delete the zombie orchestration
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        public static void ReplayZombieMessage(Guid ServiceInstanceID)
        {
            Boolean DeleteOrchestrationAction = false;



            WMIAccess wmiAccess = new WMIAccess();
            wmiAccess.GetZombieMessage(ServiceInstanceID);

            //Initialize artifact list
            BizTalkArtifacts btArtifact = new BizTalkArtifacts();

            // Loop over the returned messages from the query
            if (wmiAccess.MessageFound)
            {
                DeleteOrchestrationAction = true;
                LogHelper.WriteInfo(String.Format("New Message Zombie Found for service instance {0}", ServiceInstanceID));
                
                
                //check for save zombie message to file
                if (ConfigParameter.FileActivated)
                {
                    UsingFileLayer(ServiceInstanceID, wmiAccess, btArtifact);
                }
            }
            else
            {
                LogHelper.WriteInfo(String.Format("No zombie message found, the instance {0} is not a zombie instance",ServiceInstanceID));
            }

            //Now terminate the current orchestration 
            if (DeleteOrchestrationAction)
            {
                LogHelper.WriteInfo("Now delete zombie orchestration");
                WMIAccess.TerminateOrchestration(ServiceInstanceID);
            }
        }

        /// <summary>
        /// Saving all messages in directory
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        /// <param name="wmiAccess"></param>
        /// <param name="btArtifact"></param>
        private static void UsingFileLayer(Guid ServiceInstanceID, WMIAccess wmiAccess, BizTalkArtifacts btArtifact)
        {
            LogHelper.WriteInfo("Saving all message to file...");
            foreach (Guid gu in wmiAccess.ListMessageID)
            {
                String sMessage = btArtifact.GetMessageBodyByMessageID(gu, ServiceInstanceID);
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
