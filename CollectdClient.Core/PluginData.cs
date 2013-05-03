namespace CollectdClient.Core
{
    public class PluginData
    {
        private string _host;
        private string _plugin;
        private string _pluginInstance;
        private string _type;
        private string _typeInstance;
        private long _time;

        public PluginData(string host, string plugin, string pluginInstance, string type, string typeInstance, long time)
        {
            _host = host;
            _plugin = plugin;
            _pluginInstance = pluginInstance;
            _type = type;
            _typeInstance = typeInstance;
            _time = time;
        }

        public string Host { get { return _host; } }
        public string Plugin { get { return _plugin; } }
        public string PluginInstance { get { return _pluginInstance; } }
        public string Type { get { return _type; } }
        public string TypeInstance { get { return _typeInstance; } }
        public long Time { get { return _time;  } }
    }
}
