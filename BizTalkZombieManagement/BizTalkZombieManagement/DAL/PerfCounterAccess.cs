using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Dal
{
    public static class PerfCounterAccess
    {
        static PerformanceCounter ZombieCounter;
        
        /// <summary>
        /// initialize counter
        /// </summary>
        public static void InitPerfCounter()
        {
            //check if the counter category already exist
            if (!PerformanceCounterCategory.Exists(CounterKeyName.CategoryName) )
            {
                CounterCreationData ConfigurationZombieCounter = new CounterCreationData();
                ConfigurationZombieCounter.CounterName = CounterKeyName.CounterName;
                ConfigurationZombieCounter.CounterHelp = "Total number of Zombie message back up";
                ConfigurationZombieCounter.CounterType = PerformanceCounterType.NumberOfItems32;


                PerformanceCounterCategory.Create(CounterKeyName.CategoryName, "How many zombie message are saved", PerformanceCounterCategoryType.SingleInstance, new CounterCreationDataCollection { ConfigurationZombieCounter });
            }

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
        public static void Dispose()
        {
            if (ZombieCounter != null)
                ZombieCounter.RemoveInstance();

            if (PerformanceCounterCategory.Exists(CounterKeyName.CategoryName))
                PerformanceCounterCategory.Delete(CounterKeyName.CategoryName);
        }
    }
}
