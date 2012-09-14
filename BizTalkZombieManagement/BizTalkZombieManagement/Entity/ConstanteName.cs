using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Entity.ConstantName
{
    public static class ConstEventLog
    {
        public const String SourceName = "Zombie Management";
    }

    public static class WmiQuery
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


        public const String WmiScope = "root\\MicrosoftBizTalkServer";

        #endregion

    }

    public static class ResourceKeyName
    {
        public const String EventArrived = "EventArrived";

        public const String ResourceKeyNotFound = "Error_ResourceKeyNotFound";
        public const String ErrorValidation = "Error_Validation";
        public const String StopService = "Error_Stop";
        public const String DumpFolderNotFound = "Error_Folder";
        public const String AddressMissing = "Error_AddressMissing";
    }
}
