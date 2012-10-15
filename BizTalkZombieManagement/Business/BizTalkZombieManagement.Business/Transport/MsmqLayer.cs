using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Contracts.Interface;
using BizTalkZombieManagement.Dal.Transport;

namespace BizTalkZombieManagement.Business.Transport
{
    public class MsmqLayer : ItransportLayer
    {
        private MsmqAccess access;
        public MsmqLayer()
        {
            access = new MsmqAccess(getMsmqPath());
        }

        private static String getMsmqPath()
        {
            return ConfigParameter.MsmqPath;
        }
        public void SendMessage(string message, Guid messageInstanceID)
        {
            access.SendMessage(message);
        }

        public static Boolean IsMsmqExist()
        {
           return MsmqAccess.IsMsmqExist(getMsmqPath());
        }

    }
}
