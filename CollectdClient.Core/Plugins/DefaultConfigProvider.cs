using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public class DefaultConfigProvider : IConfigProvider
    {
        private JObject rawConfig;

        public DefaultConfigProvider(string configFile)
        {
            rawConfig = JObject.Parse(System.IO.File.ReadAllText(configFile));
        }

        public T GetPluginConfigPart<T>(string name)
        {
            throw new NotImplementedException();
        }

        public Newtonsoft.Json.Linq.JToken GetPluginConfigPart(string name)
        {
            return GetPluginConfigRoot()[name];
        }

        public Newtonsoft.Json.Linq.JToken GetRootConfig()
        {
            return rawConfig;
        }

        private JToken GetPluginConfigRoot()
        {
            return rawConfig["Plugins"];
        }
    }
}
