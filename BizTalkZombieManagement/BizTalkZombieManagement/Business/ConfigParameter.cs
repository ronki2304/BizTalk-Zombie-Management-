using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entity;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Business
{
    public static class ConfigParameter
    {
        #region File Configuration
        private static String _FilePath;
        public static String FILEPath
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


        public static Boolean FILEActivated
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

        private static Boolean IsFileConfigurationOK(Boolean isOK)
        {
            if (String.IsNullOrEmpty(ConfigParameter.FILEPath))
            {
                isOK = false;
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.AddressMissing)));
            }

            if (!Directory.Exists(ConfigParameter.FILEPath)) //folder not found
            {
                isOK = false;
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.DumpFolderNotFound), ConfigParameter.FILEPath));
            }
            return isOK;
        } 
        #endregion

        public static Boolean IsConfigurationOK()
        {
            Boolean isOK = true;
            //check All AppSetting
            //file key
            if (ConfigParameter.FILEActivated)
            {
                isOK = IsFileConfigurationOK(isOK);
            }
            return isOK;
        }

        #region MSMQ Configuration
        public static Boolean MSMQActivated
        {
            get
            {
                Boolean breturn;
                if (!Boolean.TryParse(AppSettingDal.RetrieveSpecificKey(AppKeyName.MSMQActivated), out breturn))
                {
                    LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.FileActivatedError), AppKeyName.FileActivated));
                    return false;
                }

                return breturn;
            }
        }
        #endregion

    }
}
