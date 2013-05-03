using MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.IO;

namespace CollectdClient.Core
{
    public class CollectdPacketWriter : IDisposable
    {
        private const int HeaderLen = 2;
        private MemoryStream stream;

        public CollectdPacketWriter()
        {
            this.stream = new MemoryStream();
        }

        public int Size
        {
            get
            {
                return (int)stream.Length;
            }
        }

        public byte[] Bytes
        {
            get
            {
                return stream.ToArray();
            }
        }

        public void Write(PluginData data)
        {
            WriteString(Protocol.TypeHost, data.Host);
            WriteNumber(Protocol.TypeTime, (uint)(data.Time / 1000));
            WriteString(Protocol.TypePlugin, data.Plugin);
            WriteString(Protocol.TypePluginInstance, data.PluginInstance);
            WriteString(Protocol.TypeType, data.Type);
            WriteString(Protocol.TypeTypeInstance, data.TypeInstance);

            var vl = (ValueList)data;
            if (vl != null)
            {
                List<DataSource> ds = TypesDB.Instance.GetType(data.Type);
                var values = vl.Values;

                WriteNumber(Protocol.TypeInterval, vl.Interval);
                WriteValues(ds, values);
            }
        }

        private void WriteValues(List<DataSource> ds, IList<double> values)
        {
            int num = (short)values.Count;
            var len =
                Protocol.HeaderLen +
                Protocol.UInt16Len +
                (num * Protocol.UInt8Len) +
                (num * Protocol.UInt64Len);

            int ds_len = 0;
            if (ds != null)
            {
                ds_len = ds.Count;
            }

            byte[] types = new byte[num];
            for (int i = 0; i < num; i++)
            {
                if (ds_len == 0)
                {
                    //guess the datasource here
                    throw new NotImplementedException();
                }
                else
                {
                    types[i] = (byte)ds[i].Type;
                }
            }

            WriteHeader(Protocol.TypeValues, len);
            WriteBytes(EndianBitConverter.Big.GetBytes((short)num));
            WriteBytes(types);

            for (int i = 0; i < num; i++)
            {
                var value = values[i];
                if (types[i] == DataSource.TypeGauge)
                {
                    var bytes = EndianBitConverter.Little.GetBytes(value);
                    WriteBytes(bytes);
                }
                else if(types[i] == DataSource.TypeCounter)
                {
                    //to big endian
                    var bytes = EndianBitConverter.Big.GetBytes((ulong)value);
                    WriteBytes(bytes);
                }
            }
        }

        private void WriteBytes(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteNumber(int partType, ulong val)
        {
            short len = Protocol.HeaderLen + Protocol.UInt64Len;
            WriteHeader(partType, len);
            WriteBytes(EndianBitConverter.Big.GetBytes(val));
        }

        private void WriteString(int partType, string val)
        {
            if (string.IsNullOrEmpty(val))
                return;

            byte[] data = System.Text.Encoding.ASCII.GetBytes(val);

            var len = Protocol.HeaderLen + data.Length + 1;
            WriteHeader(partType, len);
            foreach (var ch in val)
            {
                stream.WriteByte((byte)ch);
            }
            stream.WriteByte((byte)'\0');
        }

        private void WriteHeader(int partType, int len)
        {
            WriteBytes(EndianBitConverter.Big.GetBytes((short)partType));
            WriteBytes(EndianBitConverter.Big.GetBytes((short)len));
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        internal void Reset()
        {
            stream.Close();
            stream = new MemoryStream();
        }

        public object values { get; set; }
    }
}
