using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    public static class LogHelper
    {
        public static void WriteInfo(String message)
        {
            Logdiagnostics.LogInfo(message);
        }

        public static void WriteError(String message)
        {
            Logdiagnostics.LogError(message);
        }

        public static void WriteError(Exception exception)
        {
            Logdiagnostics.LogError(exception);
        }

    }
}
