using System;
using System.Configuration;

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
