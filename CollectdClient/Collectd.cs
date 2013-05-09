using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CollectdClient.Core;
using CollectdClient.Core.Extensions;
using Newtonsoft.Json.Linq;

namespace CollectdClient
{
    public class Collectd
    {
        private IPluginRepository repository;
        private readonly JObject config;
        private readonly PluginManager manager;

        public Collectd(JObject config, IPluginRepository repository)
        {
            this.config = config;
            this.repository = repository;
            this.manager = new PluginManager(repository);
        }

        private void Configure()
        {
            var enabledPlugins = config["EnabledPlugins"];
            foreach (string enabledPlugin in enabledPlugins)
            {
                var plugin = repository.GetPlugin(enabledPlugin);
                repository.EnablePlugin(plugin);
            }

            var pluginsObj = config["Plugins"];
            //for each enabled plugin
            foreach (var plugin in repository.GetConfigPlugins())
            {
                string pluginName = repository.GetPluginName(plugin);
                var configPart = pluginsObj[pluginName];
                plugin.Config(configPart);
            }
        }

        public void Init()
        {
            Configure();

            manager.Init();
        }

        public void RunLoops()
        {
            var runner = new PluginRunner(repository);
            var task = runner.Run();
            task.Wait();
        }

        public void Shutdown()
        {
            manager.Shutdown();
        }


        public static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog(baseDir));

            string path = System.IO.Path.Combine(baseDir, "config.json");
            var config = JObject.Parse(System.IO.File.ReadAllText(path));

            

            var container = new CompositionContainer(catalog);
            var repository = container.GetExportedValue<IPluginRepository>();
            var collectd = new Collectd(config, repository);
            
            collectd.Init();
            collectd.RunLoops();
            collectd.Shutdown();
        }

        
    }
}
