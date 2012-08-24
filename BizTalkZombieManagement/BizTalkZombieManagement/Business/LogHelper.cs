using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.DAL;

namespace BizTalkZombieManagement.Business
{
    public class LogHelper
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
