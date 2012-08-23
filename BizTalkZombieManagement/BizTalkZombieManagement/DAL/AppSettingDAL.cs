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
        private static String _FilePath=String.Empty;

        public static String FilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_FilePath))
                {
                    _FilePath = ConfigurationManager.AppSettings[AppKeyName.FilePath];
                }
                return _FilePath;
            }
        }
    }
}
