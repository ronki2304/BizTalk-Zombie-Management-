using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    public static class LogHelper
    {
        public static void WriteInfo(String Message)
        {
            Logdiagnostic.LogInfo(Message);
        }

        public static void WriteError(String Message)
        {
            Logdiagnostic.LogError(Message);
        }

        public static void WriteError(Exception exception)
        {
            Logdiagnostic.LogError(exception);
        }

    }
}
