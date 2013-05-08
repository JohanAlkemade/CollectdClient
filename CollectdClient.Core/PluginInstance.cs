using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public class PluginInstance
    {
        public IPlugin Plugin { get; set; }
        public IPluginMetadata Metadata { get; set; }
        public bool Enabled { get; set; }
    }
}