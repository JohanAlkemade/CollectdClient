using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CollectdClient.Core.Plugins;
using Newtonsoft.Json.Linq;

namespace CollectdClient.Plugins
{
    [Plugin("interface")]
    public class InterfacePlugin : IReadInterface , IConfigInterface
    {
        private string[] excluded;
        private string[] included;

        public Task<bool> Read()
        {
            foreach (var iface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(iface.OperationalStatus != OperationalStatus.Up)
                    continue;

                var statc = iface.GetIPv4Statistics();

            }
            
            return Task.FromResult(false);
        }

        public void Register(PluginContext context)
        {
        }

        public void Config(JToken config)
        {
            excluded = config["exclude"].Select(x => (string) x).ToArray();
            included = config["include"].Select(x => (string)x).ToArray();
        }
    }
}
