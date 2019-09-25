﻿
namespace System.Threading
{
    
    using System;
    using System.Runtime.CompilerServices;
#pragma warning disable CS0657 // 'field' is not a valid attribute location for this declaration. Valid attribute locations for this declaration are 'type'. All attributes in this block will be ignored.
    [field: NonSerialized]
#pragma warning restore CS0657 // 'field' is not a valid attribute location for this declaration. Valid attribute locations for this declaration are 'type'. All attributes in this block will be ignored.
    internal static class PlatformHelper
    {
        
        private const int PROCESSOR_COUNT_REFRESH_INTERVAL_MS = 0x7530;
        private static volatile int s_lastProcessorCountRefreshTicks;
        private static volatile int s_processorCount;

        internal static bool IsSingleProcessor
        {
            get
            {
                return (ProcessorCount == 1);
            }
        }

        internal static int ProcessorCount
        {
            get
            {
                int tickCount = Environment.TickCount;
                int num2 = s_processorCount;
                if ((num2 == 0) || ((tickCount - s_lastProcessorCountRefreshTicks) >= 0x7530))
                {
                    s_processorCount = num2 = Environment.ProcessorCount;
                    s_lastProcessorCountRefreshTicks = tickCount;
                }
                return num2;
            }
        }
    }
}