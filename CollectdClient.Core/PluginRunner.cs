﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectdClient.Core.Extensions;
using CollectdClient.Core.Plugins;
using System.Threading;
using System.Diagnostics;

namespace CollectdClient.Core
{
    public class PluginRunner
    {
        private readonly PluginManager manager;

        public PluginRunner(PluginManager manager)
        {
            this.manager = manager;
        }

        private CancellationTokenSource cts = new CancellationTokenSource();

        public Task Run()
        {
            var tasks = new List<Task>();

            var runners = new List<ReadInterfaceRunner>();
            foreach (var plugin in manager.GetReadPlugins())
            {
                var pluginInstance = manager.GetPluginInstance(plugin);
                var runner = new ReadInterfaceRunner(pluginInstance, cts.Token);
                runners.Add(runner);
            }

            return Task.WhenAll(runners.Select(x => x.Run()));
        }


        private class ReadInterfaceRunner
        {
            private readonly IReadInterface plugin;
            private readonly int interval;
            private readonly CancellationToken token;

            public ReadInterfaceRunner(PluginInstance pluginInstance, CancellationToken token)
            {
                this.plugin = (IReadInterface)pluginInstance.Plugin;
                this.interval = pluginInstance.Metadata.Interval * 1000;
                this.token = token;
            }

            public Task Run()
            {
                return Task.Factory.StartNew(() => RunAsync());
            }

            public async Task RunAsync()
            {
                while (!token.IsCancellationRequested)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    await plugin.Read();
                    sw.Stop();
                    
                    var toSleep = interval - sw.ElapsedMilliseconds;

                    if (toSleep > 0)
                        Thread.Sleep((int)toSleep);
                }
            }
        }
    }
}
