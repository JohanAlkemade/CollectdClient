using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollectdClient.Core;
using CollectdClient.Core.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectdClient.Tests
{
    [TestClass]
    public class PluginRunnerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var plugins = new List<Lazy<IPlugin, IPluginMetadata>>();
            plugins.Add(new Lazy<IPlugin, IPluginMetadata>(() => new CpuPlugin(), new PluginAttribute("cpu")));

            var repository = new ComposablePluginRepository(plugins);

            var runner = new PluginRunner(repository);

            runner.Run();
        }
    }
}
