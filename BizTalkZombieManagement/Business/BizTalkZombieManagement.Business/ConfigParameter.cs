using System;
using System.IO;
using System.Linq;
using BizTalkZombieManagement.Business.Transport;
using BizTalkZombieManagement.Contracts.CustomInterfaces;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.ConstantName;
using BizTalkZombieManagement.Entities.CustomEnum;
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
            switch (WhichDumpIsSelected())
            {
                case DumpType.File:
                    //file key
                    return IsFileConfigurationOK();


                case DumpType.Msmq:
                    //Msmq Key
                    return IsMsmqConfigurationOK();

                case DumpType.Wcf:
                    return true; //currently I found no test for validating the configuration
            }

            //Configuration Error
            LogHelper.WriteError(ResourceLogic.GetString(ResourceKeyName.ErrorNoLayerActivated));
            return false;
        }



        #region MSMQ Configuration
     

        private static String _MsmqPath = null;

        public static String MsmqPath
        {
            get
            {
                if (String.IsNullOrEmpty(_MsmqPath))
                {
                    _MsmqPath = AppSettingDal.RetrieveSpecificKey(AppKeyName.MsmqPath);
                }
                return _MsmqPath;
            }
        }


        private static Boolean IsMsmqConfigurationOK()
        {
            if (MsmqAccess.IsExist(ConfigParameter.MsmqPath))
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

        /// <summary>
        /// Retrieve the name of the specific layer which will be used by BizTalkZombieManagement
        /// </summary>
        /// <returns></returns>
        private static DumpType WhichDumpIsSelected()
        {
            return (DumpType)Enum.Parse(typeof(DumpType), AppSettingDal.RetrieveSpecificKey(AppKeyName.DumpLayer));
        }

        
        public static ITransportLayer GettingTheRightLayer()
        {
            switch(WhichDumpIsSelected())
            {
                case DumpType.File:
                    return new FileLogic();
                case DumpType.Msmq:
                    return new MsmqLayer();
                case DumpType.Wcf:
                    return new WcfLogic();
                default:
                    return null;
            }
        }
    }
}
