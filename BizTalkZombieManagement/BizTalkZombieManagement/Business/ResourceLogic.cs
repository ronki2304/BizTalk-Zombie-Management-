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
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static String GetString(String keyName)
        {
            if (!String.IsNullOrEmpty(ResourceDal.GetString(keyName)))
            {
                return ResourceDal.GetString(keyName);
            }
            else
            {
                return String.Format(ResourceDal.GetString(ResourceKeyName.ResourceKeyNotFound), keyName);
            }
        }
    }
}
