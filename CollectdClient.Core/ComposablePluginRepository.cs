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
        private readonly IDictionary<string, PluginInstance> plugins;

        [ImportingConstructor]
        public ComposablePluginRepository([ImportMany]IEnumerable<Lazy<IPlugin, IPluginMetadata>> plugins)
        {
            this.plugins = plugins.ToDictionary(x => x.Metadata.Name, x => new PluginInstance()
            {
                Enabled = false,
                Plugin = x.Value,
                Metadata = x.Metadata
            });
        }

        public bool Exists(string pluginName)
        {
            return plugins.ContainsKey(pluginName);
        }

        public void EnablePlugin(string pluginName)
        {
            if (!Exists(pluginName))
                return;

            plugins[pluginName].Enabled = true;
        }

        public IPlugin GetPlugin(string pluginName)
        {
            return (from plugin in plugins
                    where plugin.Key == pluginName && plugin.Value.Enabled
                    select plugin.Value.Plugin).SingleOrDefault();
        }

        public PluginInstance GetPluginInstance(IPlugin plugin)
        {
            return plugins.Where(x => x.Value.Plugin == plugin).Select(x => x.Value).SingleOrDefault();
        }

        public IEnumerable<IPlugin> GetCurrentPlugins()
        {
            return plugins.Where(x => x.Value.Enabled).Select(x => x.Value.Plugin).ToArray();
        }

        public string GetPluginName(IPlugin plugin)
        {
            return plugins.Where(x => x.Value.Plugin == plugin).Select(x => x.Key).SingleOrDefault();
        }
    }
}