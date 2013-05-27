using System;
using System.Linq;
using CollectdClient.Core;
using CollectdClient.Core.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CollectdClient.Tests.Plugins
{
    [TestClass]
    public class PluginManagerFixture
    {
        private PluginManager manager;
        private Mock<IPlugin> plugin;
        
        [TestInitialize]
        public void Init()
        {
            plugin = new Mock<IPlugin>();
            manager = new PluginManager(new Lazy<IPlugin, IPluginMetadata>[]
            {
                new Lazy<IPlugin, IPluginMetadata>(() => plugin.Object, new PluginAttribute("test"))
            });
        }

        [TestMethod]
        public void GetPlugin_Returns_Disabled_Plugins()
        {
            Assert.IsNotNull(manager.GetPlugin("test"));
        }
        
        [TestMethod]
        public void GetCurrentPlugins_Returns_Only_Enabled_Plugins()
        {
            Assert.AreEqual(0, manager.GetCurrentPlugins().Count());

            var toEnable = manager.GetPlugin("test");
            manager.EnablePlugin(toEnable);

            Assert.AreEqual(1, manager.GetCurrentPlugins().Count());
        }

        [TestMethod]
        public void GetPluginName_Works()
        {
            Assert.AreEqual("test", manager.GetPluginName(plugin.Object));
        }
    }
}
