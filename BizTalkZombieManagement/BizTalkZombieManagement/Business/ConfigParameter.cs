using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BizTalkZombieManagement.DAL;
using BizTalkZombieManagement.Entity;

namespace BizTalkZombieManagement.Business
{
    public class ConfigParameter
    {
        private static String _FilePath;
        public static String FilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_FilePath))
                {
                    //check illegal character
                    if (!AppSettingDAL.RetrieveSpecificKey(AppKeyName.FilePath).Any(it => Path.GetInvalidPathChars().Contains(it))
                        && !String.IsNullOrEmpty(AppSettingDAL.RetrieveSpecificKey(AppKeyName.FilePath)))
                    {
                        _FilePath = AppSettingDAL.RetrieveSpecificKey(AppKeyName.FilePath);
                    }
                    else
                    {
                        LogHelper.WriteError(String.Format("Path contain in {0} is not a valid path, service will not start until you correct it, please open configuration file and correct the DumpFolder key value", AppKeyName.FilePath));
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
                if (!Boolean.TryParse(AppSettingDAL.RetrieveSpecificKey(AppKeyName.FileActivated),out breturn))
                {
                    LogHelper.WriteError(String.Format("Format Exception {0} haven't got a boolean value \n Dump file won't be activate",AppKeyName.FileActivated));
                    return false;
                }

                return breturn;
            }
        }
    }
}
