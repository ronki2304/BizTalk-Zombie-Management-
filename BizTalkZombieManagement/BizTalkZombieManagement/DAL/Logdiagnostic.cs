﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Dal
{
    public static class Logdiagnostics
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
                throw new ArgumentNullException("exception parameter is null !!");
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
