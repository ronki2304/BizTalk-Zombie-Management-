using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using BizTalkZombieManagement.Resources;


namespace BizTalkZombieManagement.Dal
{
    /// <summary>
    /// Access to resource file
    /// </summary>
    public static class ResourceDal
    {
        private static ResourceManager _rm = null;

        /// <summary>
        /// return the specified resource text from the key name
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static String GetString(String keyName)
        {
            if (_rm == null)
            {
                _rm = new ResourceManager(typeof(ZombieResource));
            }
            return _rm.GetString(keyName);
        }

    }

}
