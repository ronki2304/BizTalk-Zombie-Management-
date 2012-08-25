using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BizTalkZombieManagement.Entity;

namespace BizTalkZombieManagement.DAL
{
    public static class AppSettingDAL
    {
       
       

        public static String RetrieveSpecificKey(String KeyName)
        {
            return ConfigurationManager.AppSettings[KeyName];
        }
    }
}
