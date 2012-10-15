using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal.Transport;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.Enum;
using BizTalkZombieManagement.Contracts.Interface;

namespace BizTalkZombieManagement.Business
{
    public class WcfLogic : ItransportLayer
    {
        private WCFAccess _access = null;

        public WcfLogic()
        {
            _access = new WCFAccess(GetEndpointName().ToString());
        }

        private WcfTypes GetEndpointName()
        {
            return (WcfTypes) Enum.Parse(typeof(WcfTypes), AppSettingDal.RetrieveSpecificKey(AppKeyName.WcfType));
        }

        public void SendMessage(String message, Guid messageInstanceID)
        {
            _access.sendMessage(message);
        }
    }
}
