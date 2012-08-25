using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Entity.ConstanteName
{
    public class ConstEventLog
    {
        public const String SourceName = "Zombie Management";
    }

    public class WMIQuery
    {
         #region WMI query
        /// <summary>
        /// Serivce Instance  : orcherstration Instance ID 
        /// ReferenceType 8 zombie message
        /// </summary>
        public const String SelectZombieMessage = "SELECT * FROM MSBTS_MessageInstance WHERE ServiceInstanceID like '{0}' and ReferenceType=8";

        /// <summary>
        /// Retrieve orchestration suspended informations
        /// </summary>
        public const String SelectOrchestration = "SELECT * FROM MSBTS_ServiceInstance WHERE InstanceID like '{0}'";


        public const String WMIScope = "root\\MicrosoftBizTalkServer";

        #endregion

    }
}
