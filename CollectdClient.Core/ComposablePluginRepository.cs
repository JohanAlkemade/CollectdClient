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
        private readonly IEnumerable<Lazy<IPlugin, IPluginMetadata>> plugins;

        [ImportingConstructor]
        public ComposablePluginRepository([ImportMany]IEnumerable<Lazy<IPlugin, IPluginMetadata>> plugins)
        {
            this.plugins = plugins;
        }

        public bool Exists(string pluginName)
        {
            return plugins.Any(x => x.Metadata.Name == pluginName);
        }

        public IPlugin GetPlugin(string pluginName)
        {
            return plugins.Where(x => x.Metadata.Name == pluginName).Select(x => x.Value).SingleOrDefault();
        }

        public IEnumerable<IPlugin> GetCurrentPlugins()
        {
            return plugins.Select(x => x.Value).ToArray();
        }
    }
}