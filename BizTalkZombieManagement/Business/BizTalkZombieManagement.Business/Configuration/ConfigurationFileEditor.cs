using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal.Configuration;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.CustomEnum;

namespace BizTalkZombieManagement.Business.Configuration
{
    public class ConfigurationFileEditor
    {
        private ConfigurationFileAccess configFile;

        public ConfigurationFileEditor()
        {
            configFile = new ConfigurationFileAccess(AppSettingDal.RetrieveSpecificKey(AppKeyName.ConfigFilePath));
        }
        #region Public method

        /// <summary>
        /// saving Wcf Configuration
        /// </summary>
        /// <param name="dumpType"></param>
        /// <param name="svalue"></param>
        /// <param name="wcfType"></param>
        public void updateConfigurationFile(DumpType dumpType, String svalue, WcfType? wcfType = null)
        {
            SetDumpConfig(dumpType);

            switch (dumpType)
            {
                case DumpType.File:
                    configFile.UpdateAppSetting(AppKeyName.FilePath, svalue);
                    break;
                case DumpType.Msmq:
                    configFile.UpdateAppSetting(AppKeyName.MsmqPath, svalue);
                    break;
                case DumpType.Wcf:
                    configFile.UpdateAppSetting(AppKeyName.WcfType, wcfType.Value.ToString());
                    configFile.UpdateUri(new Uri(svalue), wcfType.Value);
                    break;
                default:
                    break;
            }

            configFile.Save();
        } 

        /// <summary>
        /// return the current dump layer used
        /// </summary>
        /// <returns></returns>
        public DumpType GetTheCurrentDumpLayer()
        {
            return (DumpType) Enum.Parse(typeof(DumpType), configFile.GetAppSetting(AppKeyName.DumpLayer.ToString()));
        }
        /// <summary>
        /// getting the saved folder
        /// </summary>
        /// <returns></returns>
        public String GetFolderPath()
        {
            return configFile.GetAppSetting(AppKeyName.FilePath);
        }

        /// <summary>
        /// getting the saved msmq 
        /// </summary>
        /// <returns></returns>
        public String GetMsmqPath()
        {
            return configFile.GetAppSetting(AppKeyName.MsmqPath);
        }

        /// <summary>
        /// getting the current wcf layer
        /// </summary>
        /// <returns></returns>
        public String GetWcfType()
        {
            return configFile.GetAppSetting(AppKeyName.WcfType);
        }

        /// <summary>
        /// Retrieve the current WCF uri
        /// </summary>
        /// <param name="wcftype"></param>
        /// <returns></returns>
        public String GetWcfUri(WcfType wcftype)
        {
            return configFile.GetWcfUri(wcftype).ToString();
        }
        #endregion

        #region private method
         /// <summary>
        /// apply the new dump type
        /// </summary>
        private void SetDumpConfig(DumpType dumptype)
        {
            configFile.UpdateAppSetting(AppKeyName.DumpLayer, dumptype.ToString());
        }
        #endregion
    }
}
