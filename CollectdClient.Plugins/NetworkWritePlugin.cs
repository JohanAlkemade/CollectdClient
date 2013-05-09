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

        public async Task<bool> Write(ValueList[] vl)
        {
            for (int i = 0; i < vl.Length; i++)
            {
                await WriteSingle(vl[i]);
            }

            //Write gets called every x seconds
            //to make sure data arrives as soon as possible, we flush the writer
            await Flush();
            return true;
        }

        private async Task Flush()
        {
            if (writer.Size > 0)
            {
                await client.SendAsync(writer.Bytes, writer.Size, host, port);
                writer.Reset();
            }
        }

        private async Task WriteSingle(ValueList vl)
        {
            int len = writer.Size;
            writer.Write(vl);

            if (writer.Size >= Protocol.BufferSize)
            {
                await client.SendAsync(writer.Bytes, len, host, port);

                writer.Reset();
                writer.Write(vl);
            }
        }
    }
}
