using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Business
{
    public static class ResourceLogic
    {
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
