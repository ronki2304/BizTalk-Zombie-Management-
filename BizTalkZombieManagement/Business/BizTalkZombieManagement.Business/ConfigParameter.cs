using System;
using System.IO;
using System.Linq;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.ConstantName;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Dal.Transport;

namespace BizTalkZombieManagement.Business
{
    public static class ConfigParameter
    {
        #region File Configuration
        private static String _FilePath;
        public static String FilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_FilePath))
                {
                    //check illegal character
                    if (!AppSettingDal.RetrieveSpecificKey(AppKeyName.FilePath).Any(it => Path.GetInvalidPathChars().Contains(it))
                        && !String.IsNullOrEmpty(AppSettingDal.RetrieveSpecificKey(AppKeyName.FilePath)))
                    {
                        _FilePath = AppSettingDal.RetrieveSpecificKey(AppKeyName.FilePath);
                    }
                    else
                    {
                        LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.FileInvalidPath), AppKeyName.FilePath));
                    }
                }

                return _FilePath;
            }
        }


        public static Boolean FileActivated
        {
            get
            {
                Boolean breturn;
                if (!Boolean.TryParse(AppSettingDal.RetrieveSpecificKey(AppKeyName.FileActivated), out breturn))
                {
                    LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.FileActivatedError), AppKeyName.FileActivated));
                    return false;
                }

                return breturn;
            }
        }

        private static Boolean IsFileConfigurationOK()
        {
            if (String.IsNullOrEmpty(ConfigParameter.FilePath))
            {
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.AddressMissing)));
                return false;
            }

            if (!Directory.Exists(ConfigParameter.FilePath)) //folder not found
            {
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.DumpFolderNotFound), ConfigParameter.FilePath));
                return false;
            }
            return true;
        } 
        #endregion

        /// <summary>
        /// check AppSetting for the only one layer activated
        /// </summary>
        /// <returns></returns>
        public static Boolean IsConfigurationOk()
        {
            //file key
            if (ConfigParameter.FileActivated)
            {
                return IsFileConfigurationOK();
            }

            //Msmq Key
            if (ConfigParameter.MsmqActivated)
            {
                return IsMsmqConfigurationOK();
            }

            //Configuration Error
            LogHelper.WriteError(ResourceLogic.GetString(ResourceKeyName.ErrorNoLayerActivated));
            return false;
        }

      

        #region MSMQ Configuration
        public static Boolean MsmqActivated
        {
            get
            {
                Boolean breturn;
                if (!Boolean.TryParse(AppSettingDal.RetrieveSpecificKey(AppKeyName.MSMQActivated), out breturn))
                {
                    LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.MsmqActivatedError), AppKeyName.FileActivated));
                    return false;
                }

                return breturn;
            }
        }

        private static String _MsmqPath=null;

        public static String MsmqPath
        {
            get
            {
                if (String.IsNullOrEmpty(_MsmqPath))
                {
                    _MsmqPath = AppSettingDal.RetrieveSpecificKey(AppKeyName.MSMQPath);
                }
                return _MsmqPath;
            }
        }


        private static Boolean IsMsmqConfigurationOK()
        {
            if (MsmqAccess.IsMsmqExist(ConfigParameter.MsmqPath))
            {
                return true;
            }
            else
            {
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.MsmqPathNotFound), ConfigParameter.MsmqPath));
                return false;
            }
        }
        #endregion

        #region WCF
        public static Boolean WcfActivated
        {
            get
            {
                Boolean breturn;
                if (!Boolean.TryParse(AppSettingDal.RetrieveSpecificKey(AppKeyName.WcfActivated), out breturn))
                {
                    LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.WCFActivatedError), AppKeyName.FileActivated));
                    return false;
                }

                return breturn;
            }
        }
        #endregion
    }
}
