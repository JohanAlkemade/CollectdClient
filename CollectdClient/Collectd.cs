using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CollectdClient.Core;
using CollectdClient.Core.Extensions;
using Newtonsoft.Json.Linq;
using CollectdClient.Core.Plugins;
using System.Collections.Specialized;
using NLog.Targets;
using NLog;
using NLog.Config;
using NLog.Conditions;
using System.Configuration.Install;
using System.Collections;

namespace CollectdClient
{
    public class Collectd
    {
        private readonly IConfigProvider configProvider;
        private readonly PluginManager manager;
        private readonly IPluginHost pluginHost;

        public Collectd(IConfigProvider provider, PluginManager manager, IPluginHost pluginHost)
        {
            this.configProvider = provider;
            this.manager = manager;
            this.pluginHost = pluginHost;
        }

        public void Init()
        {
            var config = configProvider.GetRootConfig();
            var enabledPlugins = config["EnabledPlugins"];
            foreach (string enabledPlugin in enabledPlugins)
            {
                var plugin = manager.GetPlugin(enabledPlugin);
                manager.EnablePlugin(plugin);
            }

            manager.InitPlugins(pluginHost);
            manager.ConfigurePlugins(configProvider);
        }

        public void RunLoops()
        {
            var runner = new PluginRunner(manager);
            var task = runner.Run();
            task.Wait();
        }

        public void Shutdown()
        {
            manager.Shutdown();
        }

        public static void Main(string[] args)
        {
            if (System.Environment.UserInteractive == false)
            {
                //run as a service
            }
            else
            {
                bool install = false, uninstall = false;
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                        case "-install":
                            install = true;
                            break;
                        case "-u":
                        case "-uninstall":
                            uninstall = true;
                            break;
                    }
                }

                if (install)
                {
                    Install(false, args);
                }
                else if (uninstall)
                {
                    Install(true, args);
                }
                else
                {
                    Run();
                }
            }
        }

        private static void Install(bool undo, string[] args)
        {
            using (var inst = new AssemblyInstaller(typeof(Collectd).Assembly, args))
            {
                IDictionary state = new Hashtable();
                inst.UseNewContext = true;

                try
                {

                    if (undo)
                    {
                        inst.Uninstall(state);
                    }
                    else
                    {
                        inst.Install(state);
                        inst.Commit(state);
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        inst.Rollback(state);
                    }
                    catch { }
                    throw;
                }
            }
        }


        private static void Run()
        {
            ConfigureLogging();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog(baseDir));

            string path = System.IO.Path.Combine(baseDir, "config.json");

            var provider = new DefaultConfigProvider(path);

            var container = new CompositionContainer(catalog);
            var manager = container.GetExportedValue<PluginManager>();
            var pluginHost = container.GetExportedValue<IPluginHost>();
            var collectd = new Collectd(provider, manager, pluginHost);

            collectd.Init();
            collectd.RunLoops();
            collectd.Shutdown();
        }

        private static void ConfigureLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var console = new ColoredConsoleTarget();
            config.AddTarget("console", console);

            var rule = new LoggingRule("*", NLog.LogLevel.Debug, console);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }


    }
}
