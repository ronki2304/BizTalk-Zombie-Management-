using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entity;
using BizTalkZombieManagement.Entity.ConstantName;
using System.Messaging;

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

        private static Boolean IsFileConfigurationOK()
        {
            if (String.IsNullOrEmpty(ConfigParameter.FILEPath))
            {
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.AddressMissing)));
                return false;
            }

            if (!Directory.Exists(ConfigParameter.FILEPath)) //folder not found
            {
                LogHelper.WriteError(String.Format(ResourceLogic.GetString(ResourceKeyName.DumpFolderNotFound), ConfigParameter.FILEPath));
                return false;
            }
            return true;
        } 
        #endregion

        /// <summary>
        /// check AppSetting for the only one layer activated
        /// </summary>
        /// <returns></returns>
        public static Boolean IsConfigurationOK()
        {
            //file key
            if (ConfigParameter.FILEActivated)
            {
                return IsFileConfigurationOK();
            }

            //Msmq Key
            if (ConfigParameter.MSMQActivated)
            {
                return IsMsmqConfigurationOK();
            }

            //Configuration Error
            LogHelper.WriteError(ResourceLogic.GetString(ResourceKeyName.ErrorNoLayerActivated));
            return false;
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
            if (MessageQueue.Exists(ConfigParameter.MsmqPath))
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

    }
}
