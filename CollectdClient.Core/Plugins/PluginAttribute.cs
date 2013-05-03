using System;
using System.ComponentModel.Composition;

namespace CollectdClient.Core.Plugins
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginAttribute : ExportAttribute, IPluginMetadata
    {
        public PluginAttribute(string name) : base(typeof(IPlugin))
        {
            this.Name = name;
            this.Interval = 10;
        }

        public string Name { get; private set; }
        public int Interval { get; private set; }
    }
}
