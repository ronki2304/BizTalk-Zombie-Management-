using System;
using BizTalkZombieManagement.Contracts.CustomInterfaces;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Dal.Transport;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.CustomEnum;

namespace BizTalkZombieManagement.Business
{
    public class WcfLogic : ITransportLayer
    {
        private WcfAccess _access = null;

        public WcfLogic()
        {
            _access = new WcfAccess(GetEndpointName().ToString());
        }

        private static WcfType GetEndpointName()
        {
            return (WcfType) Enum.Parse(typeof(WcfType), AppSettingDal.RetrieveSpecificKey(AppKeyName.WcfType));
        }

        public void SendMessage(String message, Guid messageInstanceId)
        {
            _access.SendMessage(message);
        }
    }
}
