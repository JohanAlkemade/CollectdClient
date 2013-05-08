using System.Collections.Generic;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public interface IPluginRepository
    {
        bool Exists(string pluginName);

        void EnablePlugin(string pluginName);
        IPlugin GetPlugin(string pluginName);
        PluginInstance GetPluginInstance(IPlugin plugin);

        IEnumerable<IPlugin> GetCurrentPlugins();
        string GetPluginName(IPlugin plugin);
    }
}
