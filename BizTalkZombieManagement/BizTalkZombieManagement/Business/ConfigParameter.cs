using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BizTalkZombieManagement.DAL;

namespace BizTalkZombieManagement.Business
{
    public class ConfigParameter
    {
        private static String _FilePath;
        public static String GetFilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_FilePath))
                {
                    //check illegal character
                    if (!AppSettingDAL.FilePath.Any(it => Path.GetInvalidPathChars().Contains(it))
                        && !AppSettingDAL.FilePath.Any(it => Path.GetInvalidFileNameChars().Contains(it))
                        && !String.IsNullOrEmpty(AppSettingDAL.FilePath))
                    {
                        _FilePath = AppSettingDAL.FilePath;
                    }
                }

                return _FilePath;
            }
        }
    }
}
