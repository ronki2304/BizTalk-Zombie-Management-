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
        /// <summary>
        /// Getting specific key from setting file
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static String RetrieveSpecificKey(String keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }
    }
}
