using System;
using System.ComponentModel.Composition;

namespace CollectdClient.Core.Plugins
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginAttribute : ExportAttribute, IPluginMetadata
    {
        public PluginAttribute(string name) : this(name, 10)
        {
        }

        public PluginAttribute(string name, int interval) : base(typeof (IPlugin))
        {
            this.Name = name;
            this.Interval = interval;
        }

        public string Name { get; private set; }
        public int Interval { get; private set; }
    }
}
