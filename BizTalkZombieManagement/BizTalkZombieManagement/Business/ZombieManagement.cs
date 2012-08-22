using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using BizTalkZombieManagement.DAL;
using System.IO;

namespace BizTalkZombieManagement.Business
{
    public static class ZombieManagement
    {

        #region private member

        #region WMI query
        /// <summary>
        /// Serivce Instance  : orcherstration Instance ID 
        /// ReferenceType 8 zombie message
        /// </summary>
        private const String _SelectZombieMessage = "SELECT * FROM MSBTS_MessageInstance WHERE ServiceInstanceID like '{0}' and ReferenceType=8";

        /// <summary>
        /// Retrieve orchestration suspended informations
        /// </summary>
        private const String _SelectOrchestration = "SELECT * FROM MSBTS_ServiceInstance WHERE InstanceID like '{0}'";
        #endregion


        private const String _WMIScope = "root\\MicrosoftBizTalkServer";

        #endregion





        /// <summary>
        /// get back the zombie message, replay it and delete the zombie orchestration
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        public static void ReplayZombieMessage(Guid ServiceInstanceID)
        {
            Boolean DeleteOrchestrationAction = false;

            String sQuery = String.Format(_SelectZombieMessage, ServiceInstanceID.ToString("B"));


            ManagementObjectSearcher searchZombieMessages =
                    new ManagementObjectSearcher(new ManagementScope(_WMIScope), new ObjectQuery(sQuery), null);



            // Loop over the returned messages from the query
            // Write the message and context to the supplied folder
            foreach (ManagementObject objServiceInstance in searchZombieMessages.Get())
            {
                DeleteOrchestrationAction = true;
                Console.WriteLine("New Message Zombie Found");
                //getting body message
                String Message = BizTalkArtifact.GetMessageBodyByMessageID(Guid.Parse(objServiceInstance.Properties[WMIProperties.MessageInstanceID].Value.ToString())
                                , Guid.Parse(objServiceInstance.Properties[WMIProperties.ServiceInstanceID].Value.ToString()));



                String sAction;
                String sDomaine;
                ////Getting mesage context
                //ExtractDataFromContextMessage(XDocument.Parse(objServiceInstance.Properties[WMIProperties.Context].Value.ToString()), out sAction, out sDomaine);
                //StarshipDB.insertEvenement(Message, sDomaine, sAction);

                //String path = @"c:\Bts_Test\{0}.xml";
                //SaveToFile(Message,path);

            }

            //Now terminate the current orchestration 
            if (DeleteOrchestrationAction)
            {
                TerminateOrchestration(ServiceInstanceID);
            }
        }

        private static void SaveToFile(String Message,String sPath)
        {
            using (FileStream fs = File.Create(String.Format(sPath,Guid.NewGuid())))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(Message);
                fs.Write(info, 0, info.Length);
            }

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

        /// <summary>
        /// Terminate Orchestration which have created zombie message
        /// </summary>
        /// <param name="ServiceInstanceID"></param>
        private static void TerminateOrchestration(Guid ServiceInstanceID)
        {
            String sQuery = String.Format(_SelectOrchestration, ServiceInstanceID.ToString("B"));

            ManagementObjectSearcher searchZombieMessages =
                  new ManagementObjectSearcher(new ManagementScope(_WMIScope), new ObjectQuery(sQuery), null);

            foreach (ManagementObject Mob in searchZombieMessages.Get())
                Mob.InvokeMethod("Terminate", new Object[] { ServiceInstanceID.ToString("B") });

        }
    }
}
