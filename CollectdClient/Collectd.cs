using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using CollectdClient.Core;

namespace CollectdClient
{
    public class Collectd
    {
        [Import(typeof(IPluginRepository))]
        public IPluginRepository Repository { get; set; }


        public void Init()
        {
            foreach (var writer in Repository.GetWritePlugins())
            {
                CollectdClient.Core.Collectd.Pipeline.RegisterWriter(writer);
            }
        }

        public void RunLoops()
        {
            var runner = new PluginRunner(Repository);
            runner.Run();
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
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)));
            
            var collectd = new Collectd();
            var batch = new CompositionBatch();

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
