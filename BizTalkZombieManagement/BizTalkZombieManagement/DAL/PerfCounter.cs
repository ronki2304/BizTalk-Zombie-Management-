using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BizTalkZombieManagement.Entity.ConstantName;

namespace BizTalkZombieManagement.Dal
{
    public static class PerfCounter
    {
        static PerformanceCounter ZombieCounter;
        static Int32 TotalZombie;
        static PerfCounter()
        {
            ZombieCounter = new PerformanceCounter(CounterKeyName.CategoryName, CounterKeyName.CounterName);
            TotalZombie = 0;
        }

        public static void UpdateStatistic()
        {
            TotalZombie++;
            ZombieCounter.RawValue = TotalZombie;
        }

        public static void Dispose()
        {
            ZombieCounter.RemoveInstance();
        }
    }
}
