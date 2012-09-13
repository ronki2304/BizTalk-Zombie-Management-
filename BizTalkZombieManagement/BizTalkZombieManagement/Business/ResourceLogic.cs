using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.DAL;

namespace BizTalkZombieManagement.Business
{
    public static class ResourceLogic
    {
        public static String GetString(String sKeyName)
        {
            return ResourceDAL.GetString(sKeyName);
        }
    }
}
