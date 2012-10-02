using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace BizTalkZombieManagement.DAL
{
    public class MsmqAccess
    {
        private MessageQueue _messageQueue;


        public MsmqAccess(String msmqURI)
        {
            _messageQueue = new MessageQueue(msmqURI);
        }
    }
}
