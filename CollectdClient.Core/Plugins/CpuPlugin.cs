﻿using System.Collections.Generic;
using System.Diagnostics;

namespace CollectdClient.Core.Plugins
{
    [Plugin("cpu")]
    public class CpuPlugin : IReadInterface, IInitInterface, IShutdownInterface
    {
        private readonly IList<PerformanceCounter> counters = new List<PerformanceCounter>();

        public void Destroy()
        {
            foreach (var counter in counters)
            {
                counter.Dispose();
            }
        }

        public bool CpuRead()
        {
            foreach (var counter in counters)
            {
                DispatchCounter( counter);
            }

            return true;
        }

        private void DispatchCounter(PerformanceCounter counter)
        {
            string typeInstance;
            
            if (counter.CounterName == "% Processor Time")
                typeInstance = "system";
            else if (counter.CounterName == "% User Time")
                typeInstance = "user";
            else if (counter.CounterName == "% Idle Time")
                typeInstance = "idle";
            else if (counter.CounterName == "% Interrupt Time")
                typeInstance = "wait";
            else
                return;

            var vl = ValueList.Build()
                              .Plugin("cpu")
                              .PluginInstance(counter.InstanceName)
                              .Type("cpu")
                              .TypeInstance(typeInstance)
                              .AddValue(counter.NextValue())
                              .Build();
            
            Collectd.DispatchValues(vl);
        }

        public void Register(PluginContext context)
        {
        }

        public bool Read()
        {
            return CpuRead();
        }

        public bool Init()
        {
            int numberOfProcessors = System.Environment.ProcessorCount;

            //init counters
            for (int i = 0; i < numberOfProcessors; i++)
            {
                var systemCounter = new PerformanceCounter(
                    "Processor",
                    "% Processor Time",
                    i.ToString()
                );

                var userCounter = new PerformanceCounter(
                    "Processor",
                    "% User Time",
                    i.ToString()
                );

                var idleCounter = new PerformanceCounter(
                    "Processor",
                    "% Idle Time",
                    i.ToString()
                );

                var interruptCounter = new PerformanceCounter(
                    "Processor",
                    "% Interrupt Time",
                    i.ToString()
                );

                counters.Add(systemCounter);
                counters.Add(userCounter);
                counters.Add(idleCounter);
                counters.Add(interruptCounter);
            }
            return true;
        }

        public bool Shutdown()
        {
            this.counters.Clear();
            return true;
        }
    }
}