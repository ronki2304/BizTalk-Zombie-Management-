using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Dal;
using System.Threading.Tasks;

namespace BizTalkZombieManagement.Business
{
    public static class PerfCounterAsync
    {
        /// <summary>
        /// Initialize the performance counter
        /// </summary>
        public static void InitPerformanceCounter()
        {
            PerfCounterAccess.InitPerfCounter();
        }

        /// <summary>
        /// Increment the performance counter asynchronously for better performance
        /// </summary>
        public static void UpdateStatistic()
        {
            Task.Factory.StartNew(() => PerfCounterAccess.UpdateStatistic());
        }

        /// <summary>
        /// clear and delete the performance counter
        /// </summary>
        public static void Dispose()
        {
            PerfCounterAccess.Dispose();
        }
    }
}
