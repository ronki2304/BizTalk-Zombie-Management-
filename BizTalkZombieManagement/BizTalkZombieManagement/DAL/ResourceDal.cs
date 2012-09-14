using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using BizTalkZombieManagement.Resource;


namespace BizTalkZombieManagement.Dal
{
    public static class ResourceDal
    {
        private static ResourceManager _rm = null;

        //accessor for the resource manager
        private static ResourceManager ResourceManager
        {
            get
            {
                if (_rm == null)
                {
                    _rm = new ResourceManager(typeof(ZombieResource));
                }
                return _rm;
            }
        }

        public static String GetString(String keyName)
        {
            return ResourceManager.GetString(keyName);
        }

    }

}
