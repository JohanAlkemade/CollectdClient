using System.Collections.Generic;
using System.Threading.Tasks;
using CollectdClient.Core.Plugins;
using System;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace CollectdClient.Core
{
    public class PluginRunner
    {
        private readonly IEnumerable<IPlugin> plugins;

        public PluginRunner(IPluginRepository repository)
        {
            this.plugins = repository.GetCurrentPlugins();
        }

        private CancellationTokenSource cts = new CancellationTokenSource();

        public void Run()
        {
            var tasks = new List<Task>();

            foreach (var plugin in plugins.OfType<IReadInterface>())
            {
                var runner = new ReadInterfaceRunner(plugin, cts.Token);
                tasks.Add(runner.Run());
            }

            Task.WaitAll(tasks.ToArray());
        }


        private class ReadInterfaceRunner
        {
            private readonly IReadInterface plugin;
            private readonly CancellationToken token;
            private Timer timer;

            public ReadInterfaceRunner(IReadInterface plugin, CancellationToken token)
            {
                this.plugin = plugin;
                this.token = token;
            }

            public Task Run()
            {
                return Task.Factory.StartNew(() =>
                {
                    //ctx.Init();
                    while (!token.IsCancellationRequested)
                    {
                        var sw = new Stopwatch();
                        sw.Start();
                        plugin.Read();
                        sw.Stop();

                        Console.WriteLine(string.Format("Skew: {0}", sw.ElapsedMilliseconds));

                        var toSleep = 10000 - sw.ElapsedMilliseconds;

                        if (toSleep > 0)
                            Thread.Sleep((int)toSleep);
                    }
                }, TaskCreationOptions.LongRunning);
            }



        }
    }
}
