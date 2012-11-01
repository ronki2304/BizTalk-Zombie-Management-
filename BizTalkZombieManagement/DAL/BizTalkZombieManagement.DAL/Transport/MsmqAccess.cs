using System;
using System.Messaging;
using System.Xml.Linq;

namespace BizTalkZombieManagement.Dal.Transport
{
    public class MsmqAccess
    {

        private MessageQueue _messageQueue;

        public MsmqAccess(String uri)
        {
            _messageQueue = new MessageQueue(uri);
        }

        /// <summary>
        /// Send message over MSMQ
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(String message)
        {
            _messageQueue.Send(XElement.Parse(message));
        }

        ~MsmqAccess()
        {
            _messageQueue.Close();
            _messageQueue.Dispose();
        }

        public static Boolean IsExist(String path)
        {
            try
            {
                return !String.IsNullOrEmpty(path) && MessageQueue.Exists(path);
            }
            catch
            {
                return false;
            }
        }
    }
}
