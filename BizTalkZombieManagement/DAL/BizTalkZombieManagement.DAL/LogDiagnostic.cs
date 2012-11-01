using System;
using System.Diagnostics;
using BizTalkZombieManagement.Entities.ConstantName;

namespace BizTalkZombieManagement.Dal
{
    public static class LogDiagnostics
    {
        public static void LogError(String message)
        {
            TraceLine(String.Concat("ERROR : ", message));

           //write to eventlog
            LogEventLog(message, EventLogEntryType.Error);
        }

      

        public static void LogError(Exception ex)
        {
            if (ex != null)
            {
                LogError(ex.ToString());
            }
            else
            {
                throw new ArgumentNullException(typeof(LogDiagnostics).Name,"exception parameter is null !!");
            }
        }

        public static void LogInfo(String message)
        {
            TraceLine(String.Concat("INFO : ", message));
        }

        #region private member
        static EventLog _eventLog = null;
        #endregion



        #region Private Method
        private static void LogEventLog(String message, EventLogEntryType EvType)
        {
            if (_eventLog == null)
            {
                //initialisation
                _eventLog = new EventLog();
                _eventLog.Source = ConstEventLog.SourceName;
            }

            _eventLog.WriteEntry(message, EvType);

        }

        private static void TraceLine(String messageToTrace)
        {
            Trace.WriteLine(messageToTrace);
        } 
        #endregion
        
    }
}
