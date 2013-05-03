using System;
using System.Net.Sockets;

namespace CollectdClient.Core
{
    public class UdpDispatcher : Sender, IDisposable
    {
        private UdpClient client;
        private CollectdPacketWriter writer;

        public UdpDispatcher(string host, int port)
        {
            this.writer = new CollectdPacketWriter();
            this.client = new UdpClient(host, port);
            client.DontFragment = true;
            client.Ttl = 64;
        }
        
        protected override void Write(PluginData data)
        {
            int len = writer.Size;
            writer.Write(data);
            Send(writer.Bytes, writer.Bytes.Length);

            //if (writer.Size >= Protocol.BufferSize)
            //{
            //    writer.Reset();
            //    writer.Write(data);
            //}
        }

        private void Send(byte[] bytes, int length)
        {
            client.Send(bytes, length);
        }

        public void Dispose()
        {
            if (client != null)
                client.Close();
        }
    }
}
