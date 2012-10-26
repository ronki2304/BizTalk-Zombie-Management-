using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal.Configuration;

namespace BizTalkZombieManagement.Business.Configuration
{
    public class ConfigurationFileEditor
    {
        private ConfigurationFileAccess configFile;

        public ConfigurationFileEditor()
        {
            configFile = new ConfigurationFileAccess();
        }
    }
}
