using System;
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
        /// <summary>
        /// use in the service windows to determine if the uri store is ok
        /// </summary>
        /// <returns></returns>
        public static Boolean IsExist()
        {
           return MsmqAccess.IsExist(getMsmqPath());
        }

        /// <summary>
        /// use in the UI to check if the new Uri is available
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Boolean IsExist(String uri)
        {
            return MsmqAccess.IsExist(uri);
        }
    }
}
