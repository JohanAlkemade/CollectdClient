using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CollectdClient.Core.Plugins;
using Newtonsoft.Json.Linq;
using CollectdClient.Core;

namespace CollectdClient.Plugins
{
    [Plugin("interface")]
    public class InterfacePlugin : Plugin, IReadInterface, IConfigInterface
    {
        private string[] excluded;
        private string[] included;

        public Task<bool> Read()
        {
            foreach (var iface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (iface.OperationalStatus != OperationalStatus.Up)
                    continue;

                if (!InterfaceEnabled(iface))
                    continue;

                var statc = iface.GetIPv4Statistics();
                Submit(iface.Description, "if_octest", statc.BytesReceived / 8, statc.BytesSent / 8);
                Submit(iface.Description, "if_errors", statc.IncomingPacketsWithErrors, statc.OutgoingPacketsWithErrors);
                Submit(iface.Description, "if_packets", statc.NonUnicastPacketsReceived, statc.NonUnicastPacketsSent);
            }

            return Task.FromResult(false);
        }

        private void Submit(string ifaceName, string key, long rx, long tx)
        {
            var vlRx = ValueList.Build()
                .Plugin("interface")
                .PluginInstance(ifaceName)
                .Type(key)
                .TypeInstance("rx")
                .AddValue(rx)
                .Build();

            var vlTx = ValueList.Build()
                .Plugin("interface")
                .PluginInstance(ifaceName)
                .Type(key)
                .TypeInstance("tx")
                .AddValue(tx)
                .Build();

            Host.DispatchValues(vlRx);
            Host.DispatchValues(vlTx);
        }

        private bool InterfaceEnabled(NetworkInterface iface)
        {
            if (!excluded.Any() && !included.Any())
                return true;

            if (excluded.Contains(iface.Description))
                return false;

            if (included.Contains(iface.Description))
                return true;
            
            return false;
        }

        public void Config(IConfigProvider provider)
        {
            var config = provider.GetPluginConfigPart("interface");
            excluded = config["exclude"].Select(x => (string)x).ToArray();
            included = config["include"].Select(x => (string)x).ToArray();
        }
    }
}
