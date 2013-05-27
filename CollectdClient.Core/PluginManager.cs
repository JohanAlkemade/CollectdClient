using CollectdClient.Core.Plugins;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core
{
    [Export]
    public class PluginManager
    {
        private readonly IList<PluginInstance> instances;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        [ImportingConstructor]
        public PluginManager([ImportMany]IEnumerable<Lazy<IPlugin, IPluginMetadata>> plugins)
        {
            this.instances = plugins.Select(x => new PluginInstance
            {
                Enabled = false,
                Plugin = x.Value,
                Metadata = x.Metadata
            }).ToList();
        }

        public void InitPlugins(IPluginHost host)
        {
            foreach (var plugin in GetCurrentPlugins())
            {
                if (!plugin.Register(host))
                {
                    DisablePlugin(plugin);
                    continue;
                }

                var initPlugin = plugin as IInitInterface;
                if (initPlugin != null)
                {
                    InitPlugin(initPlugin);
                }
            }

            
            //At this point every plugin is initialized and enabled
            //so we can register the writers on our pipeline
            foreach (var writer in GetWritePlugins())
            {
                Pipeline.Current.AddWriter(writer);
            }
        }

        public void ConfigurePlugins(IConfigProvider configProvider)
        {
            var config = configProvider.GetRootConfig();

            //for each enabled plugin
            foreach (var plugin in GetConfigPlugins())
            {
                plugin.Config(configProvider);
            }
        }

        private void InitPlugin(Plugins.IInitInterface plugin)
        {
            try
            {
                plugin.Init();
            }
            catch (PluginException)
            {
                DisablePlugin(plugin);
            }
        }

        public void Shutdown()
        {
            foreach (var plugin in GetShutdownPlugins())
            {
                plugin.Shutdown();
            }
        }

        public bool Exists(string pluginName)
        {
            return GetPlugin(pluginName) != null;
        }

        public void EnablePlugin(IPlugin plugin)
        {
            EnablePlugin(plugin, true);
        }

        private void EnablePlugin(IPlugin plugin, bool enabled)
        {
            var instance = instances.SingleOrDefault(x => x.Plugin == plugin);
            if (instance != null)
            {
                if(enabled)
                    log.Info("Enabling {0} logger", instance.Metadata.Name);
                else
                    log.Info("Disabling {0} logger", instance.Metadata.Name);

                instance.Enabled = enabled;
            }
        }

        public void DisablePlugin(IPlugin plugin)
        {
            EnablePlugin(plugin, false);
        }

        public IPlugin GetPlugin(string pluginName)
        {
            return (from instance in instances
                    where instance.Metadata.Name == pluginName
                    select instance.Plugin).SingleOrDefault();
        }

        public PluginInstance GetPluginInstance(IPlugin plugin)
        {
            return instances.SingleOrDefault(x => x.Plugin == plugin);
        }

        public IEnumerable<IPlugin> GetCurrentPlugins()
        {
            return instances.Where(x => x.Enabled).Select(x => x.Plugin).ToArray();
        }

        public string GetPluginName(IPlugin plugin)
        {
            var instance = GetPluginInstance(plugin);
            return instance == null ? "" : instance.Metadata.Name;
        }

        public IEnumerable<IReadInterface> GetReadPlugins()
        {
            return GetCurrentPlugins().OfType<IReadInterface>();
        }

        public IEnumerable<IWriteInterface> GetWritePlugins()
        {
            return GetCurrentPlugins().OfType<IWriteInterface>();
        }

        public IEnumerable<IInitInterface> GetInitPlugins()
        {
            return GetCurrentPlugins().OfType<IInitInterface>();
        }

        public IEnumerable<IShutdownInterface> GetShutdownPlugins()
        {
            return GetCurrentPlugins().OfType<IShutdownInterface>();
        }

        public IEnumerable<IConfigInterface> GetConfigPlugins()
        {
            return GetCurrentPlugins().OfType<IConfigInterface>();
        }
    }
}
