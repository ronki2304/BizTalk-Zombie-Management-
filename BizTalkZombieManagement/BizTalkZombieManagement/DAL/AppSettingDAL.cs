using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BizTalkZombieManagement.Entity;

namespace BizTalkZombieManagement.Dal
{
    public static class AppSettingDal
    {
        public static String RetrieveSpecificKey(String keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }
    }
}
