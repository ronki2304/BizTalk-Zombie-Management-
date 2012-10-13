using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Entities.ConstantName
{
    public static class ResourceKeyName
    {
        public const String EventArrived = "EventArrived";
        public const String ResourceKeyNotFound = "Error_ResourceKeyNotFound";
        public const String ErrorValidation = "Error_Validation";
        public const String StopService = "Error_Stop";
        public const String DumpFolderNotFound = "Error_Folder";
        public const String AddressMissing = "Error_AddressMissing";
        public const String FileSaving = "File_Saving";
        public const String FileSaved = "File_Saved";
        public const String ZombieFound = "Zombie_Found";
        public const String NoZombieFound = "No_Zombie_Found";
        public const String DeleteZombieOrchestration = "Delete_Zombie_Orchestration";
        public const String FileInvalidPath = "File_Invalid_Path";
        public const String FileActivatedError = "File_Activated_Error";
        public const String MsmqPathNotFound = "Msmq_PathNotFound";
        public const String MsmqActivatedError = "Msmq_Activated_Error";
        public const String ErrorNoLayerActivated = "Error_NoLayerActivated";
        public const String MsmqSaving = "Msmq_SavingMessage";
        public const String MsmqSaved = "Msmq_SavedMessage";
        public const String DebugMode = "Debug_Mode";

        public const String WCFActivatedError = "WCF_Activated_Error";
    }
}
