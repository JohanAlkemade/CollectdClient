using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace CollectdClient.Core.Plugins
{
    public abstract class Plugin : IPlugin
    {
        public IPluginHost Host { get; private set; }

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Logger Log { get { return logger; } }

        public bool Register(IPluginHost host)
        {
            return (this.Host = host) != null;
        }

    }
}
