using System;
using System.Linq;
using CollectdClient.Core;
using CollectdClient.Core.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CollectdClient.Tests.Plugins
{
    [TestClass]
    public class ComposablePluginRepositoryFixture
    {
        private IPluginRepository repository;
        private Mock<IPlugin> plugin;
        
        [TestInitialize]
        public void Init()
        {
            plugin = new Mock<IPlugin>();
            repository = new ComposablePluginRepository(new Lazy<IPlugin, IPluginMetadata>[]
            {
                new Lazy<IPlugin, IPluginMetadata>(() => plugin.Object, new PluginAttribute("test"))
            });
        }

        [TestMethod]
        public void GetPlugin_Returns_Disabled_Plugins()
        {
            Assert.IsNotNull(repository.GetPlugin("test"));
        }
        
        [TestMethod]
        public void GetCurrentPlugins_Returns_Only_Enabled_Plugins()
        {
            Assert.AreEqual(0, repository.GetCurrentPlugins().Count());

            var toEnable = repository.GetPlugin("test");
            repository.EnablePlugin(toEnable);

            Assert.AreEqual(1, repository.GetCurrentPlugins().Count());
        }

        [TestMethod]
        public void GetPluginName_Works()
        {
            Assert.AreEqual("test", repository.GetPluginName(plugin.Object));
        }
    }
}
