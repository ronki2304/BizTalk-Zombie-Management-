using System.Diagnostics;
using BizTalkZombieManagement.Entities.ConstantName;

namespace BizTalkZombieManagement.Dal
{
    public static class PerfCounterAccess
    {
        static PerformanceCounter ZombieCounter;
        
        /// <summary>
        /// create the counter category
        /// </summary>
        public static void CreateCategory()
        {
            if (!PerformanceCounterCategory.Exists(CounterKeyName.CategoryName))
            {
                CounterCreationData ConfigurationZombieCounter = new CounterCreationData();
                ConfigurationZombieCounter.CounterName = CounterKeyName.CounterName;
                ConfigurationZombieCounter.CounterHelp = "Total number of Zombie message back up";
                ConfigurationZombieCounter.CounterType = PerformanceCounterType.NumberOfItems32;
                


                PerformanceCounterCategory.Create(CounterKeyName.CategoryName, "How many zombie message are saved", PerformanceCounterCategoryType.SingleInstance, new CounterCreationDataCollection { ConfigurationZombieCounter });
            }
        }


        public static void DeleteCategory()
        {
            if (PerformanceCounterCategory.Exists(CounterKeyName.CategoryName))
                PerformanceCounterCategory.Delete(CounterKeyName.CategoryName);
        }
        /// <summary>
        /// initialize counter
        /// </summary>
        public static void InitPerfCounter()
        {
            ZombieCounter = new PerformanceCounter();
            ZombieCounter.CategoryName = CounterKeyName.CategoryName;
            ZombieCounter.CounterName = CounterKeyName.CounterName;
            ZombieCounter.MachineName = ".";
            ZombieCounter.ReadOnly = false;
            ZombieCounter.RawValue = 0;
        }

        /// <summary>
        /// increment the perf counter
        /// </summary>
        public static void UpdateStatistic()
        {
            ZombieCounter.Increment();
        }

        /// <summary>
        /// delete the counter
        /// </summary>
        public static void Close()
        {
            if (ZombieCounter != null)
                ZombieCounter.RemoveInstance();

            
        }
    }
}
