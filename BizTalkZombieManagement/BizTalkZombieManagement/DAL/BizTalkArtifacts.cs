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
        private static String _BtsConnectionString = String.Empty;
        #endregion

        #region private member
        private Dictionary<Guid,String> _MessageDictionnary;
        #endregion


        public BizTalkArtifacts()
        {
            _MessageDictionnary = new Dictionary<Guid, String>();
        }

        #region register key
        private const string REG_KEY_BTS_ADMINISTRATION = @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration";
        private const string _KeyMgmtDBName = "MgmtDBName";
        private const string _KeyMgmtDBServer = "MgmtDBServer";
        #endregion


        /// <summary>
        /// retrieve BTS connection from register key
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
        /// retrieve message body by message ID and store it in the dictionary
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>the message content</returns>
        public String GetMessageBodyByMessageId(Guid messageId, Guid instanceId)
        {
            //check if it is the first message retrieving
            if (!_MessageDictionnary.ContainsKey(messageId))
            {
                using (BizTalkOperations operations = new BizTalkOperations())
                {

                    IBaseMessage message = operations.GetMessage(messageId, instanceId);
                    String body = String.Empty;
                    using (StreamReader streamReader = new StreamReader(message.BodyPart.Data))
                    {
                        body = streamReader.ReadToEnd();
                    }
                    //add the new message to the dictionnary
                    _MessageDictionnary.Add(messageId, body);
                    return body;
                }
            }
            else
            {
                return _MessageDictionnary[messageId];
            }
        }

        /// <summary>
        /// Retrieve message with a list of Message ID, they will be stored in dictionary
        /// </summary>
        /// <param name="lGu">list of message ID</param>
        /// <param name="instanceId">BizTalk instance id</param>
        public void GetAllMessagesBody(IEnumerable<Guid> messagesId, Guid instanceId)
        {
            if (messagesId != null && messagesId.Any())
            {
                foreach (Guid gu in messagesId)
                {
                    GetMessageBodyByMessageId(gu, instanceId);
                }
            }
        }
    }
}
