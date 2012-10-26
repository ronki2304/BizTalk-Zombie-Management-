using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.CustomEnum;

namespace BizTalkZombieManagement.Dal.Configuration
{
    public class ConfigurationFileAccess
    {
        public ConfigurationFileAccess()
        {
            var file =ConfigurationManager.OpenExeConfiguration(@"C:\Program Files\Jeremy RONK\BizTalk Zombie Management\BizTalkZombieManagement.exe.config");
            file.AppSettings[AppKeyName.DumpLayer] = DumpType.File;
        }
    }
}
