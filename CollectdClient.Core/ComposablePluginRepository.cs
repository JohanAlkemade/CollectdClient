using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    [Export(typeof(IPluginRepository))]
    public class ComposablePluginRepository : IPluginRepository
    {
        private readonly IList<PluginInstance> instances;

        [ImportingConstructor]
        public ComposablePluginRepository([ImportMany]IEnumerable<Lazy<IPlugin, IPluginMetadata>> plugins)
        {
            this.instances = plugins.Select(x => new PluginInstance
            {
                Enabled = false,
                Plugin = x.Value,
                Metadata = x.Metadata
            }).ToList();
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
    }
}