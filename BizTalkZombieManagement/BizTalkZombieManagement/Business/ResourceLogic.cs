using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Business
{
    /// <summary>
    /// call resource manager
    /// </summary>
    public static class ResourceLogic
    {
        /// <summary>
        /// retrieve specific text from resource file by key name
        /// </summary>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static String GetString(String KeyName)
        {
            if (!String.IsNullOrEmpty(ResourceDal.GetString(KeyName)))
            {
                return ResourceDal.GetString(KeyName);
            }
            else
            {
                return String.Format(ResourceDal.GetString(ResourceKeyName.ResourceKeyNotFound), KeyName);
            }
        }
    }
}
