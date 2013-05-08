using System.Net.Sockets;
using System.Threading.Tasks;
using CollectdClient.Core;
using CollectdClient.Core.Plugins;
using Newtonsoft.Json.Linq;

namespace CollectdClient.Plugins
{
    [Plugin("network")]
    public class NetworkWritePlugin : IWriteInterface, IConfigInterface
    {
        private CollectdPacketWriter writer = new CollectdPacketWriter();
        private UdpClient client;
        private int port;
        private string host;

        public NetworkWritePlugin()
        {
            client = new UdpClient();
            client.Ttl = 64;
            client.DontFragment = true;
        }

        public void Register(PluginContext context)
        {
        }

        public void Config(JToken config)
        {
            var server = config["server"].Value<string>();
            int idx = server.IndexOf(':');
            if (idx > 0)
            {
                host = server.Substring(0, idx);
                port = int.Parse(server.Substring(idx + 1));
            }
            else
            {
                host = server;
                port = 25826;
            }
        }

        public async Task<bool> Write(ValueList vl)
        {
            int len = writer.Size;
            writer.Write(vl);

            if (writer.Size >= Protocol.BufferSize)
            {
                await client.SendAsync(writer.Bytes, len, host, port);

                writer.Reset();
                writer.Write(vl);
            }

            return true;
        }

    }
}
