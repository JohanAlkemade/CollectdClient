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
        [Import(typeof(IPluginRepository))]
        public IPluginRepository Repository { get; set; }

        private readonly JObject config;

        public Collectd(JObject config)
        {
            this.config = config;
        }

        private void Configure()
        {
            var enabledPlugins = config["EnabledPlugins"];
            enabledPlugins.ForEach(x => Repository.EnablePlugin(x.Value<string>()));

            var pluginsObj = config["Plugins"];
            //for each enabled plugin
            foreach (var plugin in Repository.GetConfigPlugins())
            {
                string pluginName = Repository.GetPluginName(plugin);
                var configPart = pluginsObj[pluginName];
                plugin.Config(configPart);
            }
        }

        public void Init()
        {
            Configure();

            foreach (var writer in Repository.GetWritePlugins())
            {
                CollectdClient.Core.Collectd.Pipeline.RegisterWriter(writer);
            }
        }

        public void RunLoops()
        {
            var runner = new PluginRunner(Repository);
            var task = runner.Run();
            task.Wait();
        }

        public void Shutdown()
        {
            
        }

        private void InitPlugins()
        {
            foreach (var plugin in Repository.GetInitPlugins())
            {
                plugin.Init();
            }
        }

        private void ShutdownPlugins()
        {
            foreach (var plugin in Repository.GetShutdownPlugins())
            {
                plugin.Shutdown();
            }
        }

        public static void LogError(string error)
        {
        }


        public static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog(baseDir));

            string path = System.IO.Path.Combine(baseDir, "config.json");
            var config = JObject.Parse(System.IO.File.ReadAllText(path));

            var collectd = new Collectd(config);

            var container = new CompositionContainer(catalog);
            container.ComposeParts(collectd);

            collectd.Init();
            collectd.InitPlugins();
            collectd.RunLoops();
            collectd.ShutdownPlugins();
            collectd.Shutdown();
        }

        
    }
}
