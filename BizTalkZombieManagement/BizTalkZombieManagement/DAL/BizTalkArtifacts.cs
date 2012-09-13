using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.ExplorerOM;
using Microsoft.Win32;
using Microsoft.BizTalk.Operations;
using System.IO;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkZombieManagement.Dal
{
    public class BizTalkArtifacts
    {
        #region static private members
        private static String _StarshipURI = String.Empty;
        private static String _BtsConnectionString = String.Empty;
        #endregion

        #region private member
        private Dictionary<Guid,String> MessageDictionnary;
        #endregion


        public BizTalkArtifacts()
        {
            MessageDictionnary = new Dictionary<Guid, String>();
        }

        #region register key
        private const string REG_KEY_BTS_ADMINISTRATION = @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration";
        private const string _KeyMgmtDBName = "MgmtDBName";
        private const string _KeyMgmtDBServer = "MgmtDBServer";
        #endregion


        /// <summary>
        /// retrieve BTS connection
        /// </summary>
        private static string BtsConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_BtsConnectionString))
                {
                    RegistryKey hKey = Registry.LocalMachine.OpenSubKey(REG_KEY_BTS_ADMINISTRATION, false);
                    _BtsConnectionString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;"
                        , hKey.GetValue(_KeyMgmtDBServer)
                        , hKey.GetValue(_KeyMgmtDBName));
                }
                return _BtsConnectionString;
            }
        }

        /// <summary>
        /// retrieve message body by message ID
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public String GetMessageBodyByMessageID(Guid messageID, Guid InstanceID)
        {
            //check if it is the first message retrieving
            if (!MessageDictionnary.ContainsKey(messageID))
            {
                using (BizTalkOperations operations = new BizTalkOperations())
                {

                    IBaseMessage message = operations.GetMessage(messageID, InstanceID);
                    String body = String.Empty;
                    using (StreamReader streamReader = new StreamReader(message.BodyPart.Data))
                    {
                        body = streamReader.ReadToEnd();
                    }
                    //add the new message to the dictionnary
                    MessageDictionnary.Add(messageID, body);
                    return body;
                }
            }
            else
            {
                return MessageDictionnary[messageID];
            }
        }

        /// <summary>
        /// Retrieve message with a list of Message ID
        /// </summary>
        /// <param name="lGu"></param>
        /// <param name="InstanceID"></param>
        public void GetAllMessagesBody(IEnumerable<Guid> lGu, Guid InstanceID)
        {
            foreach (Guid gu in lGu)
            {
                GetMessageBodyByMessageID(gu, InstanceID);
            }
        }
    }
}
