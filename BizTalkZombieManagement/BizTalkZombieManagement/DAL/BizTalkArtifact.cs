using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.ExplorerOM;
using Microsoft.Win32;
using Microsoft.BizTalk.Operations;
using System.IO;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkZombieManagement.DAL
{
    class BizTalkArtifact
    {
        #region private members
        private static String _StarshipURI = String.Empty;
        private static String _BtsConnectionString = String.Empty;
        #endregion

        


        #region register key
        private const string REG_KEY_BTS_ADMINISTRATION = @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration";
        private const string _KeyMgmtDBName = "MgmtDBName";
        private const string _KeyMgmtDBServer = "MgmtDBServer";
        #endregion

        public static string BtsConnectionString
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
        /// retrieve message body
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public static String GetMessageBodyByMessageID(Guid messageID, Guid InstanceID)
        {
            BizTalkOperations operations = new BizTalkOperations();

            IBaseMessage message = operations.GetMessage(messageID, InstanceID);
            String body = String.Empty;
            using (StreamReader streamReader = new StreamReader(message.BodyPart.Data))
            {
                body = streamReader.ReadToEnd();
            }
            return body;
        }
    }
}
