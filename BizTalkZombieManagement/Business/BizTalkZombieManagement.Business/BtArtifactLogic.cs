using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    public class BtArtifactLogic
    {
        #region private static members
        private static List<String> AvoidSchemasType = null;
        private static Object AntiConcurential = new Object();
        private BizTalkArtifacts artifact = null;
        #endregion

        #region private member
        private Dictionary<Guid, String> _MessageDictionnary;
        #endregion

        #region constructor
        public BtArtifactLogic()
        {
            _MessageDictionnary = new Dictionary<Guid, String>();
            artifact = new BizTalkArtifacts();
        }

        static BtArtifactLogic()
        {
            InitializeAvoidSchemaTypeList();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// retrieve all system message type to avoid process on it
        /// </summary>
        private static void InitializeAvoidSchemaTypeList()
        {
            lock (AntiConcurential)
            {
                if (AvoidSchemasType == null)
                {
                    AvoidSchemasType = BizTalkArtifacts.InitializeAvoidSchemaTypeList();
                }
            }
        } 
        #endregion

        #region public static method

        /// <summary>
        /// check if the zombie message is not due to a unhandle external fault  like a WCF fault
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static Boolean IsSystemSchema(String messageType)
        {
            if (AvoidSchemasType.Contains(messageType))
                return true;
            else
                return false;
        }

        #endregion

        #region public method

        /// <summary>
        /// retrieve message body by message ID and store it in the dictionary
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>the message content</returns>
        public String GetMessageBodyByMessageId(Guid messageId, Guid instanceId)
        {
            String messagecontent;
             //check if it is the first message retrieving
            if (!_MessageDictionnary.ContainsKey(messageId))
            {
                messagecontent = artifact.GetMessageBodyByMessageId(messageId, instanceId);
                _MessageDictionnary.Add(messageId, messagecontent);
                return messagecontent;
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
        #endregion
    }
}
