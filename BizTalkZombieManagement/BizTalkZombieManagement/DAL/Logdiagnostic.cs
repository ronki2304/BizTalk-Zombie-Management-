using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BizTalkZombieManagement.Entity.ConstanteName;

namespace BizTalkZombieManagement.DAL
{
    public class Logdiagnostic
    {
        public static void LogError(String Message)
        {
            TraceLine(String.Concat("ERROR : ", Message));

           //write to eventlog
            LogEventLog(Message, EventLogEntryType.Error);
        }

      

        public static void LogError(Exception ex)
        {
            LogError(ex.ToString());
        }

        public static void LogInfo(String Message)
        {
            TraceLine(String.Concat("INFO : ", Message));
        }

        #region private member
        static EventLog _eventLog = null;
        #endregion



        #region Private Method
        private static void LogEventLog(String Message, EventLogEntryType EvType)
        {
            if (_eventLog == null)
            {
                //initialisation
                _eventLog = new EventLog();
                _eventLog.Source = ConstEventLog.SourceName;
            }

            _eventLog.WriteEntry(Message, EvType);

        }

        private static void TraceLine(String Message)
        {
            Trace.WriteLine(Message);
        } 
        #endregion
        
    }
}
