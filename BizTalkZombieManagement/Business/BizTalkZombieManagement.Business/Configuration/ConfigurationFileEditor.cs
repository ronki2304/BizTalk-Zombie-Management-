using System;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Dal.Configuration;
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
        /// <param name="value"></param>
        /// <param name="wcfType"></param>
        public void updateConfigurationFile(DumpType dumpType, String value, WcfType? wcfType = null)
        {
            SetDumpConfig(dumpType);

            switch (dumpType)
            {
                case DumpType.File:
                    configFile.UpdateAppSetting(AppKeyName.FilePath, value);
                    break;
                case DumpType.Msmq:
                    configFile.UpdateAppSetting(AppKeyName.MsmqPath, value);
                    break;
                case DumpType.Wcf:
                    configFile.UpdateAppSetting(AppKeyName.WcfType, wcfType.Value.ToString());
                    configFile.UpdateUri(new Uri(value), wcfType.Value);
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
        public DumpType CurrentDumpLayer
        {
            get
            {
                return (DumpType)Enum.Parse(typeof(DumpType), configFile.GetAppSetting(AppKeyName.DumpLayer.ToString()));
            }
        }
        /// <summary>
        /// getting the saved folder
        /// </summary>
        /// <returns></returns>
        public String FolderPath
        {
            get
            {
                return configFile.GetAppSetting(AppKeyName.FilePath);
            }
        }

        /// <summary>
        /// getting the saved msmq 
        /// </summary>
        /// <returns></returns>
        public String MsmqPath
        {
            get
            {
                return configFile.GetAppSetting(AppKeyName.MsmqPath);
            }
        }

        /// <summary>
        /// getting the current wcf layer
        /// </summary>
        /// <returns></returns>
        public String WcfType
        {
            get
            {
                return configFile.GetAppSetting(AppKeyName.WcfType);
            }
        }

        /// <summary>
        /// Retrieve the current WCF uri
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Uri GetWcfUri(WcfType type)
        {
            return configFile.GetWcfUri(type);
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
