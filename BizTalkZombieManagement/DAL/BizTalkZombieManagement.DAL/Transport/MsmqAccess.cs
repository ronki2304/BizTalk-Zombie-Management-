using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace BizTalkZombieManagement.Dal.Transport
{
    public class MsmqAccess
    {

        private MessageQueue _messageQueue;

        public MsmqAccess(String Uri)
        {
            _messageQueue = new MessageQueue(Uri);
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
            return MessageQueue.Exists(path);
        }
    }
}
