using CollectdClient.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core
{
    public class PluginManager
    {
        private readonly IPluginRepository repository;

        public PluginManager(IPluginRepository repository)
        {
            this.repository = repository;
        }

        public void Init()
        {
            foreach (var plugin in repository.GetInitPlugins())
            {
                InitPlugin(plugin);
            }

            foreach (var writer in repository.GetWritePlugins())
            {
                CollectdClient.Core.Collectd.Pipeline.AddWriter(writer);
            }
        }

        private void InitPlugin(Plugins.IInitInterface plugin)
        {
            try
            {
                plugin.Init();
            }
            catch(PluginException)
            {
                repository.DisablePlugin(plugin);
            }
        }

        public void Shutdown()
        {
            foreach (var plugin in repository.GetShutdownPlugins())
            {
                plugin.Shutdown();
            }            
        }
    }
}
