using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Contracts.CustomInterfaces;
using BizTalkZombieManagement.Dal.Transport;

namespace BizTalkZombieManagement.Business.Transport
{
    public class MsmqLayer : ITransportLayer
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
        public void SendMessage(string message, Guid messageInstanceId)
        {
            access.SendMessage(message);
        }

        public static Boolean IsExist()
        {
           return MsmqAccess.IsExist(getMsmqPath());
        }

    }
}
