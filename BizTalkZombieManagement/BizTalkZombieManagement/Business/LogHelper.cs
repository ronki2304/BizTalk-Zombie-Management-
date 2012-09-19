using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    /// <summary>
    /// use to call Log Manager 
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Write detail from String message
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInfo(String message)
        {
            LogDiagnostics.LogInfo(message);
        }
        /// <summary>
        /// Write error from String message
        /// </summary>
        /// <param name="message"></param>
        public static void WriteError(String message)
        {
            LogDiagnostics.LogError(message);
        }

        /// <summary>
        /// Write message from exception
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteError(Exception exception)
        {
            LogDiagnostics.LogError(exception);
        }

    }
}
