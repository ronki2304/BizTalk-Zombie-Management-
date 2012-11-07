using System.Threading.Tasks;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    public static class PerfCounterAsync
    {
        /// <summary>
        /// Initialize the performance counter
        /// </summary>
        public static void InitPerformanceCounter()
        {
#if DEBUG
            PerfCounterAccess.CreateCategory();
#endif

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
        /// clear the performance counter
        /// </summary>
        public static void Close()
        {
            PerfCounterAccess.Close();
#if DEBUG
            PerfCounterAccess.DeleteCategory();
#endif
        }

        /// <summary>
        /// create the performance counter
        /// </summary>
        public static void CreateCategory()
        {
            PerfCounterAccess.CreateCategory();
        }

        /// <summary>
        /// delete the performance counter
        /// </summary>
        public static void DeleteCategory()
        {
            PerfCounterAccess.DeleteCategory();
        }
    }
}
