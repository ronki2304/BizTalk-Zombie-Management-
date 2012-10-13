using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.Enum;

namespace BizTalkZombieManagement.Business
{
    public class WcfLogic
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

        public void SendMessage(String message)
        {
            _access.sendMessage(message);
        }
    }
}
