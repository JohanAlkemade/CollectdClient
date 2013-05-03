using System;
using System.Collections.Generic;
using System.Text;

namespace CollectdClient.Core
{
    /// <summary>
    /// Explicitly designed to be immutable
    /// </summary>
    public class ValueList : PluginData
    {
        private IList<double> values = new List<double>();
        private IList<DataSource> ds = new List<DataSource>();

        public ValueList(string host, string plugin, string pluginInstance, string type, string typeInstance, long time, IList<double> values, IList<DataSource> ds) :
            base(host, plugin, pluginInstance, type, typeInstance, time)
        {
            this.values = values;
            this.ds = ds;
        }

        public uint Interval { get; set; }

        public IList<double> Values
        {
            get
            {
                return values;
            }
        }

        public IList<DataSource> DataSource
        {
            get { return ds; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("=[");

            int size = values.Count;
            for (int i =0 ; i < size; i++)
            {
                double val = values[i];
                string name;
                if (ds.Count == 0)
                {
                    name = "unknown" + i;
                }
                else
                {
                    name = ds[i].Name;
                }
                sb.Append(name).Append('=').Append(val);
                if (i < size - 1)
                {
                    sb.Append(';');
                }
            }

            sb.Append("]");
            return sb.ToString();
        }

        public static Builder Build()
        {
            return new Builder();
        }

        public class Builder
        {
            private string _host;
            private string _plugin;
            private string _pluginInstance;
            private string _type;
            private string _typeInstance;
            private long _time;
            private uint _interval;
            private IList<double> _values = new List<double>();
            private IList<DataSource> _ds = new List<DataSource>();

            public Builder AddValue(double value)
            {
                _values.Add(value);
                return this;
            }

            public Builder AddDataSource(DataSource ds)
            {
                _ds.Add(ds);
                return this;
            }

            public Builder Host(string host)
            {
                _host = host;
                return this;
            }

            public Builder Plugin(string plugin)
            {
                _plugin = plugin;
                return this;
            }

            public Builder PluginInstance(string pluginInstance)
            {
                _pluginInstance = pluginInstance;
                return this;
            }

            public Builder Type(string type)
            {
                _type = type;
                return this;
            }

            public Builder TypeInstance(string typeInstance)
            {
                _typeInstance = typeInstance;
                return this;
            }

            public Builder Time(long time)
            {
                _time = time;
                return this;
            }

            public Builder Interval(uint interval)
            {
                _interval = interval;
                return this;
            }

            private static Lazy<string> getHost = new Lazy<string>(System.Net.Dns.GetHostName);
            private static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            public ValueList Build()
            {
                if (string.IsNullOrEmpty(_host))
                {
                    _host = getHost.Value;
                }

                if (_time <= 0)
                {
                    _time = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
                }

                return new ValueList(_host, _plugin, _pluginInstance, _type, _typeInstance, _time, _values, _ds);
            }
        }
    }
}

